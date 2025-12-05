/*
 * Created by SharpDevelop.
 * User: DavidPiska
 * Date: 22.2.2017
 * Time: 9:42
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace UtilitaPrekladReceptu
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{	public static string Cesta="";
		public static string Log="";
		private Jazyky jazyky=new Jazyky();
		ReceptParserBlueII receptParserBlueII=new ReceptParserBlueII();
		
		Thread prekT;
		
		
		public static int infoCounter;
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			if(!Directory.Exists("__Easy Cooking"))Directory.CreateDirectory("__Easy Cooking");
			if(!Directory.Exists("__Programy"))Directory.CreateDirectory("__Programy");
			if(!Directory.Exists("__Extras"))Directory.CreateDirectory("__Extras");
			if(!Directory.Exists("__Konvektomat"))Directory.CreateDirectory("__Konvektomat"); // ulozi ve tvaru nazvu souboru konvektomatu
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			jazyky.Init();	// nacte slovnik
			
			string[] s=jazyky.ReturnAllLanguages();
			foreach(string ss in s)
			{	comboBox1.Items.Add(ss);
			}
			this.comboBox1.SelectedIndex=0;
			
			
			
			//CestaReference="Ref Programy Vision II 6.3.17.br2";
			//receptParserBlueII.loadXML(CestaReference);
			
			//if(Jazyky.NewWords>0) // neco se pridalo do slovniku, je treba zkontrolovat
			//{	MessageBox.Show(Jazyky.NewWords.ToString()+ " slov se pridalo do slovniku, je potreba ho zkontrolovat !!", "Neco se pridalo do slovniku");
			//}
			Jazyk=1;
			this.comboBox1.SelectedIndex=Jazyk;
			
		}
		
		
		
		// load
		public static string CestaReference="";
		void Button1Click(object sender, EventArgs e)
		{
			//string s="EasyCookingCZ.br2";
			
			OpenFileDialog openFileDialog1 = new OpenFileDialog();

		    openFileDialog1.Filter = "br2 files (*.br2)|*.txt|All files (*.*)|*.*" ;
		    openFileDialog1.FilterIndex = 2 ;
		    openFileDialog1.RestoreDirectory = true ;
		
		    if(openFileDialog1.ShowDialog() == DialogResult.OK)
		    {	CestaReference=openFileDialog1.FileName;
		    	receptParserBlueII.loadXML(CestaReference);
		    }
			
			if(Jazyky.NewWords>0) // neco se pridalo do slovniku, je treba zkontrolovat
			{	MessageBox.Show(Jazyky.NewWords.ToString()+ " slov se pridalo do slovniku, je potreba ho zkontrolovat !!", "Neco se pridalo do slovniku");
			}
		}
		
		
		// vyber jazyka
		public static int Jazyk=0;
		void ComboBox1SelectedIndexChanged(object sender, EventArgs e)
		{	Jazyk=comboBox1.SelectedIndex;
		}
		
		// ulozit
		void Button3Click(object sender, EventArgs e)
		{
			jazyky.ConvertFileToDifferentLanguage(CestaReference,Jazyk);
	
		}
		
		
		bool AutomatVsechnoMaka=false;
		// automaticky vsechno
		void Button2Click(object sender, EventArgs e)
		{	if(AutomatVsechnoMaka)return;
			AutomatVsechnoMaka=true;
			
			this.timer1.Start();
			prekT=new Thread(new ThreadStart(AutomativkyVsechno));
			prekT.Start();
		}
		
		
		string tb1="";
		string lb1="";
		int pbv1=0;
		// thread automaticky vsechno
		void AutomativkyVsechno()
		{	pbv1=0;
			
			infoCounter=0;
			if(!Directory.Exists("_REF"))
			{	MessageBox.Show("Directory _REF not found", "Directory _REF not found",MessageBoxButtons.OK);
				return;
			}
			
			
			// ******************* kontrola slovniku a REF
			string[] ddd=Directory.GetFiles("_REF");
			int newWords2=0;
			foreach(var file in ddd)
			{	Jazyky.NewWords=0;
				receptParserBlueII.loadXML(file);
				if(Jazyky.NewWords>0)	// chybi slovo ve slovniku
				{	newWords2+=Jazyky.NewWords;
					Log+="----- "+Jazyky.NewWords.ToString()+"word in ("+file+") was added to Dictionary.";
				}
			}
			tb1=Log;
			if(newWords2>0) // neco se pridalo do slovniku, je treba zkontrolovat
			{	MessageBox.Show(newWords2.ToString()+ " slov se pridalo do slovniku, je potreba ho zkontrolovat !!", "Neco se pridalo do slovniku");
				return;
			}
			
			tb1="Dictionary OK ....";
			
			string[] s=jazyky.ReturnAllLanguages(); // vsechny jazykove verze
			int n=0;
			foreach(string ss in s)
			{	
				MainForm.Jazyk=n;
				//if(n!=0) // cestina se preskoci
				{	if(!Directory.Exists(ss))	// vytvori slozku
					{	Directory.CreateDirectory(ss);
						Thread.Sleep(100);
					}
				}
				
				int nn=0;
				string[] dd=Directory.GetFiles("_REF");
				foreach(var file in dd)
				{	lb1=ss+"-"+file;
					pbv1=(n*100/s.Length+100/s.Length*nn/dd.Length);	// PROGRESS bar
					//this.Refresh();
					//Thread.Sleep(100);
					
					string soubor="";
					//if(n!=0) // cestina se preskoci
					{	soubor=jazyky.ConvertFileToDifferentLanguage(file,n,ss);	// nakopiruje soubory do jednotlivych slozek
					}
					
					string sISO=Kodovani_ISO.VratISOCestu(file,ss);
					if(sISO!="")	// jestli se to povedlo, ulozi se soubor
					{	File.WriteAllText(sISO, soubor);
					}
					
					
					string sKonv=Kodovani_Konvektomat.VratKonvektomatCestu(file,ss);	// ulozi pod jinym nazvem pro konvektomat
					if(sKonv!="")	// jestli se to povedlo, ulozi se soubor
					{	File.WriteAllText(sKonv, soubor);
					}
					
				
					
					Xrefresh=true;
					
					nn++;
				}
				Xrefresh=true;
				
				n++;
			}
			pbv1=0;
			lb1="Finished."+infoCounter.ToString();
			tb1=Log;
			jzp=true;
			Xrefresh=true;
			AutomatVsechnoMaka=false;
			
		}
		
		
		bool jzp=false;
		bool Xrefresh=false;
		void Timer1Tick(object sender, EventArgs e)
		{
			if(Xrefresh)
			{	this.textBox1.Text=tb1;
				this.label1.Text=lb1;
				this.progressBar1.Value=pbv1;
				
			}
			Xrefresh=false;
			
			if(jzp)
			{	jzp=false;
				MainForm.Jazyk=comboBox1.SelectedIndex;	// jazyk zpatky
			}
		}
		
		
	}
}
