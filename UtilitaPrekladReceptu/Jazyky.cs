/*
 * Created by SharpDevelop.
 * User: DavidPiska
 * Date: 10.11.2014
 * Time: 11:24
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace UtilitaPrekladReceptu
{
	/// <summary>
	/// Description of Jazyky.
	/// </summary>
	public sealed class Jazyky
	{	private static Jazyky instance=new Jazyky();
		
			
		public static Jazyky Instance
		{	get{ return instance;}
		}
	
	
		public Jazyky()
		{	Init();
		}
		
		
		
		public static int NewWords=0;	// nove slova
		private Dictionary<string,string[]> dictionary=new Dictionary<string, string[]>();
		
		/// <summary>
		/// nacte slovnik ze souboru
		/// </summary>
		public void Init()
		{	int i=0;
			if(File.Exists(MainForm.Cesta+"ProgramsDictionary.txt"))
			{	using (TextReader rdr = new StreamReader(MainForm.Cesta+"ProgramsDictionary.txt"))
				{	string line;
	    			while ((line = rdr.ReadLine()) != null) 
	    			{	
	    				if(line.Contains("Pečivo, Dezerty"))
	    				{
	    				}
	    				
	znova2:
	    	i=line.IndexOf("\"");	// odstrani uvozovky bo excel 2010 je doplnuje kam se mu zachce
			if(i>=0)
			{	line=line.Remove(i,1);
				goto znova2;
			}
			
	    				string[] fields = line.Split('\t');
	    				if(!dictionary.ContainsKey(fields[0]))
	    				{	dictionary.Add(fields[0],fields);
	    				}
	    				else
	    				{	//MyDebug.Add("!stejny slovnik: "+fields[0]);
	    				}
	    			}
				}
			}
			else
			{	MessageBox.Show("File ProgramsDictionary.txt was not found in main directory.");
				//MainForm.Odkaz.Dispose();	// ukonci se aplikace
			}
		}
		
		
		
		/// <summary>
		/// najde ve slovniku prislusny jazyk a vrati text
		/// </summary>
		/// <param name="s"></param>
		/// <returns> string </returns>
		public string VratText(string s)
		{ 
			if(s.Contains("Jablko"))
			{ int i3=0;
			}
			MainForm.infoCounter++;	// pocitadlo pruchodu
			
		znova:
			int i=s.IndexOf(Environment.NewLine);	// odstrani entery a rozdeli to na radky
			if(i>0)
			{	string s2=s.Substring(0,i);
				VratText(s2);
				s=s.Remove(0,i+2);
				goto znova;
			}
			
		znova2:
			i=s.IndexOf('"');	// odstrani uvozovky bo excel 2010 je doplnuje kam se mu zachce
			if(i>0)
			{	s=s.Remove(i,1);
				goto znova2;
			}
				
			
			if(dictionary.ContainsKey(s))	// klic existuje
			{	string[] ss=dictionary[s];
				if(ss.Length<=MainForm.Jazyk)	// kdyz neni dopsany jazyk
				{	MainForm.Log+="Language "+MainForm.Jazyk.ToString()+" does not exist ("+s+")"+Environment.NewLine;
					if(ss.Length>1){ return ss[1];}		// a existuje anglictina, tak ji vrati
					return s;					// vrati orig. cestinu
				}
				if(ss[MainForm.Jazyk]=="")
				{	if(s=="") return "";
					string[] ssL=dictionary["cs"];
					string[] ssL2=dictionary["Czech"];
					MainForm.Log+="Missing /"+ss[0]+"/ in language "+ssL[MainForm.Jazyk].ToString()+" "+ssL2[MainForm.Jazyk].ToString()+Environment.NewLine;
					if((ss.Length>1)&&(ss[1]!=""))	// kdyz je anglictina a neni prazdna ""
					{	return ss[1];				// vrati anglictinu
					}
					return ss[0];	// jinak vrati cestinu
				}
				return ss[MainForm.Jazyk];
			}
			else	// klic neexistuje ve stavajicim slovniku
			{						
				bool existuje=false;
				using (TextReader rdr = new StreamReader("ProgramsDictionary.txt"))
				{	string line;
	    			while ((line = rdr.ReadLine()) != null) 
	    			{	string[] fields = line.Split('\t');
	    				if(s==fields[0])
	    				{	existuje=true;
	    					break;
	    				}
	    			}
				}
				if(existuje)return s;
				
				MainForm.Log+="Dictionary missing - "+s+Environment.NewLine;
									
				NewWords++;
				File.AppendAllText(MainForm.Cesta+"ProgramsDictionary.txt", Environment.NewLine+s, Encoding.Unicode);	// prida novy retezec do slovniku
				if(s=="1")
				{ 
				}
			}
			return s;
		}	//*/
		/*public string VratText(string s)
		{ return "";
		}*/
	
		
		
		/// <summary>
		/// vrati vypis celeho slovniku
		/// </summary>
		/// <returns> string </returns>
		public string ReturnAllDictionary()
		{	string s="";
			List<string> list = new List<string>(dictionary.Keys);
			foreach (string k in list)
			{	s+=k.PadRight(20)+"  "+string.Join("; ",dictionary[k]);
				s+=Environment.NewLine;
			}
			return s;
		}
		
		public string[] ReturnAllLanguages()
		{	string[] s=dictionary["cs"];
			return s;
		}
		
		/// <summary>
		/// vybere a nastavi index aktualniho jazyku
		/// </summary>
		/// <param name="SelectedLanguage"></param>
		/// <returns> bool jestli se to povedlo</returns>
		public bool SelectLanguage(string SelectedLanguage)
		{	string[] s=dictionary["cs"];
			for(int x=0;x<s.Length;x++)
			{	if(s[x]==SelectedLanguage)	// musi sedet se slovnikem
				{	//MainForm.Jazyk=x;
					MainForm.Jazyk=x;	// vybranz jazyk se zapise do posledniho nastaveni
					return true;	// nasel se
				}
			}
			return false;	// nenasel se
		}
		
		
		
		
		
		
		
		
		
		public void ConvertFileToDifferentLanguage(string cesta, int jazyk)
		{
			string[] ds=new string[dictionary.Count];	// vytvori seznam ceskych ref vyrazu
			dictionary.Keys.CopyTo(ds, 0);
			
			/*for(int x=0;x>dictionary.Count;x++)
			{	string[] sx=dictionary.Take(x);
				ds[x]=sx[0];
			}*/
			
			
			string s=File.ReadAllText(cesta);
			
			foreach(string o1 in ds)
			{	if(o1!="")					
				{	
					s=s.Replace('"'+o1+'"','"'+VratText(o1)+'"');	// zkraje retezce uvozovky
					s=s.Replace('>'+o1+'<','>'+VratText(o1)+'<');	// zkraje retezce zavorky
					s=s.Replace('>'+o1+"\r\n",'>'+VratText(o1)+"\r\n");	// zkraje retezce zavorky
					s=s.Replace("\r\n"+o1+'<',"\r\n"+VratText(o1)+'<');	// zkraje retezce zavorky
					s=s.Replace("\r\n"+o1+"\r\n","\r\n"+VratText(o1)+"\r\n");	// zkraje retezce zavorky
					
				}
			}
			
			
			string[] sl=ReturnAllLanguages();
			
			string sdir=Path.GetDirectoryName(cesta);
			string sname=Path.GetFileName(cesta);
			sname=sl[jazyk]+" "+sname;
			string cesta2=Path.Combine(sdir,sname);
			
			File.WriteAllText(cesta2,s);
		
		}
		
		
		
		
		public string ConvertFileToDifferentLanguage(string cesta, int jazyk, string sdirDest)
		{
			string[] ds=new string[dictionary.Count];	// vytvori seznam ceskych ref vyrazu
			dictionary.Keys.CopyTo(ds, 0);
			
			string s=File.ReadAllText(cesta);
			
			foreach(string o1 in ds)
			{	if(o1!="")					
				{	s=s.Replace('"'+o1+'"','"'+VratText(o1)+'"');	// zkraje retezce uvozovky
					s=s.Replace('>'+o1+'<','>'+VratText(o1)+'<');	// zkraje retezce zavorky
					s=s.Replace('>'+o1+"\r\n",'>'+VratText(o1)+"\r\n");	// zkraje retezce zavorky
					s=s.Replace("\r\n"+o1+'<',"\r\n"+VratText(o1)+'<');	// zkraje retezce zavorky
					s=s.Replace("\r\n"+o1+"\r\n","\r\n"+VratText(o1)+"\r\n");	// zkraje retezce zavorky
				}
			}
		
			string[] sl=ReturnAllLanguages();
			string sname=Path.GetFileName(cesta);
			sname=sl[jazyk]+" "+sname;
			//string cesta2=Path.Combine(Path.GetDirectoryName( sdirDest),sname);
			
			File.WriteAllText(sdirDest+"/"+sname, s);
			
			return s;	// vrati soubor pro pozdejsi pouziti pro ISO
		}
		
		
		
		
		
		
		
	}
	
	
	

}
