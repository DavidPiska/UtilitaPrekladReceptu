/*
 * Created by SharpDevelop.
 * User: DavidPiska
 * Date: 11.11.2014
 * Time: 9:21
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System.Drawing;

//#using <mscorlib.dll>;
using System.Windows.Forms;


using System.Reflection;
using System.Runtime.InteropServices;
using System.Drawing.Text;
using System.Drawing.Imaging;

namespace UtilitaPrekladReceptu
{
	/// <summary>
	/// Description of A.
	/// </summary>
	public class A
	{
		//public Metody.Jazyky jazyky=new VisionCombiSW2.Metody.Jazyky();
		public A()
		{	//jazyky=new VisionCombiSW2.Metody.Jazyky();
			//LoadFont();
			//MyFont14= new Font(private_fonts.Families[0], 14, FontStyle.Bold);
			//MyFont9 = new Font(private_fonts.Families[0], 9, FontStyle.Regular);
			//MyFont9B = new Font(private_fonts.Families[0], 10, FontStyle.Bold);
			
			MyFont9 = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));;
			MyFont9B = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));;
			MyFont14= new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
		}
		
		
		public static int BluePrvniPiktogram=3;
		
		public static void dispose()
		{	Marshal.FreeHGlobal(unmanagedPointer);	// uvolni font
		}
		
		public static AutoScaleMode aAutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
		public static Color BackColor=Color.FromArgb(0,67,135);
		public static Color LightBlueColor=Color.FromArgb(1,125,197);
		public static Color LightBlueColor2=Color.FromArgb(30,171,242);
		public static Color WhiteColor=Color.FromArgb(255,255,255);
		public static Color GreyColor=Color.FromArgb(229,229,229);
		public static Color Red=Color.FromArgb(255,150,150);
		public static Color White=Color.FromArgb(255,255,255);
		
		public static Color BlackColor=Color.FromArgb(0,0,0);
		public static Color BlackColorPasive=Color.FromArgb(100,100,100);
		
		public static Font MyFont14;
		public static Font MyFont9;
		public static Font MyFont9B;
		
		
		public static string VratText(string s)
		{	s= s.TrimEnd();
			if (s == "")
			{
				return s;
			}
            return Jazyky.Instance.VratText(s);
		}
		
		public static int CelsiusToFarenheit(int Celsius)
		{	return Celsius*18/10+32;
		}
		
		public static float CelsiusToFarenheit(float Celsius)
		{	return Celsius*18/10+32;
		}
		
		public static int FarenheitToCelsius(int Farenheit)
		{	return (Farenheit-32)*10/18;
		}
			
		public static float FarenheitToCelsius(float Farenheit)
		{	return (Farenheit-32)*10/18;
		}
		
		
		public static string IntToTemperatureDes(int i)	// prevede integer na string °C nebo °F
		{	/*if(Metody.Nastaveni.Instance.PosledniNastaveni.NastavenaJednotka== VisionCombiSW2.Metody.Nastaveni.Jednotky.Celsius)
			{	return (((float)i)/10).ToString("F1")+"°C";
			}
			else
			{	return ((((float)i)/10*1.8)+32).ToString("F1")+"°F";
			}*/
			return "";
		}
		
		public static string IntToTemperature(int i)	// prevede integer na string °C nebo °F
		{	//if(Metody.Nastaveni.Instance.PosledniNastaveni.NastavenaJednotka== VisionCombiSW2.Metody.Nastaveni.Jednotky.Celsius)
			{	return (((float)i)/10).ToString("F0")+"°C";
			}
			/*else
			{	return ((((float)i)/10*1.8)+32).ToString("F0")+"°F";
			}*/
		}
		
		public static string IntToProcenta(int i)	// prevede integer na procenta
		{	if(i>100) i=100;
			if(i<0)i=0;
			return i.ToString()+"%";
		}
		
		public static int StringToTemperatureInt(string temp, int oldTemp)
		{	try
			{	if(temp==null) return oldTemp;
				if(temp.Contains("°C"))		// hleda a odstrani °C
				{	int n=temp.IndexOf("°C");
					{	if(n>-1)
						{	temp=temp.Substring(0,n);
						}
					}
					return Convert.ToInt32(temp)*10;
				}
				if(temp.Contains("°F"))		// hleda a odstrani °F
				{	int n=temp.IndexOf("°F");
					{	if(n>-1)
						{	temp=temp.Substring(0,n);
						}
					}
					return Convert.ToInt32(temp)*100/18+32;
				}
				return int.Parse(temp)*10;
			}
			catch(Exception e)
			{	string s=e.ToString();
				return oldTemp;
			}
			
		}
		
		public static int StringToHumidityInt(string hh, int oldTemp)
		{	try
			{	if(hh==null) return oldTemp;
				if(hh.Contains("%"))		// hleda a odstrani %
				{	int n=hh.IndexOf("%");
					{	if(n>-1)
						{	hh=hh.Substring(0,n);
						}
					}
				}
				return Convert.ToInt32(hh);
			}
			catch(Exception e)
			{	string s=e.ToString();
				return oldTemp;
			}
			
		}
		
		public static int BoolToInt(bool b)	// prevede bool na int
		{	if(b)return 1;
			else return 0;
		}
		
		
		/// <summary>
		/// prevede string na integer, kdyz budou povolene znaky na konci tak je odstrani, jinak vrati puvodni hodnotu
		/// </summary>
		/// <param name="temp"></param>
		/// <param name="oldTemp"></param>
		/// <returns></returns>
		public static int StringToInt(string temp, int oldTemp)
		{	try
			{	if(temp==null) return oldTemp;
				if(temp.IndexOf("%")>=0) temp=temp.Substring(0,temp.IndexOf("%"));
				if(temp.IndexOf("°")>=0) temp=temp.Substring(0,temp.IndexOf("°"));
				if(temp.IndexOf("s")>=0) temp=temp.Substring(0,temp.IndexOf("s"));
				if(temp.IndexOf("S")>=0) temp=temp.Substring(0,temp.IndexOf("S"));
				if(temp.IndexOf("m")>=0) temp=temp.Substring(0,temp.IndexOf("m"));
				
				return Convert.ToInt32(temp);
			}
			catch(Exception e)
			{	string s=e.ToString();
			}
			return oldTemp;
		}
		
		
		public static string IntToTime(int i,UtilitaPrekladReceptu.ReceptStruct.StepType st)	// prevede integer na string casu v sekundach
		{	
			switch(st)
			{ 	case UtilitaPrekladReceptu.ReceptStruct.StepType.stHotAir:
				case UtilitaPrekladReceptu.ReceptStruct.StepType.stCombi:
				case UtilitaPrekladReceptu.ReceptStruct.StepType.stSteam: //hh:mm
						i/=60;	// zobrazi minuty a hodiny, ne sekundy
						return (i/60).ToString("D2")+":"+(i-i/60*60).ToString("D2");
				
				case UtilitaPrekladReceptu.ReceptStruct.StepType.stPause:	// ss
				case UtilitaPrekladReceptu.ReceptStruct.StepType.stInjection:	//ss
						return i.ToString()+"s";
						
						
					//case Okna.Programy.Program.StepType.stCookAndHold:
					//case Okna.Programy.Program.StepType.stNote:
					//case Okna.Programy.Program.StepType.stGoldenTouch:
			}
			return i.ToString()+"s";
		}
		
		/// <summary>
		/// prevede int na HH:MM:SS
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public static string IntToHHMMSSstring(int i)	// prevede sekundy na HH:MM:SS
		{	return 	(i/3600).ToString("D2")+":"+				//HH
					((i-i/3600*3600 )/60).ToString("D2")+":"+	//MM
					(i-i/60*60).ToString("D2");					//SS
		}
		
		
		public static int StringToTimeInt(string time, int oldTime, UtilitaPrekladReceptu.ReceptStruct.StepType st)
		{	string min="0", sec="0";
			try
			{	if(time==null) return oldTime;
				
				switch(st)
				{ 	case UtilitaPrekladReceptu.ReceptStruct.StepType.stHotAir:
					case UtilitaPrekladReceptu.ReceptStruct.StepType.stCombi:
					case UtilitaPrekladReceptu.ReceptStruct.StepType.stSteam: //hh:mm
							
							if(time.Contains(":"))		// hleda minuty
							{	int n=time.IndexOf(":");
								{	if(n>-1)
									{	min=time.Substring(0,n);
										if(time.Length-n-1>0)	// pokud je neco na konci za dvojteckou
										{	sec=time.Substring(n+1,time.Length-n-1);
										}
									}
								}
							}
							else	
							{	sec=time;
							}
							return (Convert.ToInt32(min)*60+Convert.ToInt32(sec))*60;
				
					case UtilitaPrekladReceptu.ReceptStruct.StepType.stPause:	// ss
					case UtilitaPrekladReceptu.ReceptStruct.StepType.stInjection:	//ss
							if(time.IndexOf("s")>=0) time=time.Substring(0,time.IndexOf("s"));
							if(time.IndexOf("S")>=0) time=time.Substring(0,time.IndexOf("S"));
							return Convert.ToInt32(time);
						
						
					//case Okna.Programy.Program.StepType.stCookAndHold:
					//case Okna.Programy.Program.StepType.stNote:
					//case Okna.Programy.Program.StepType.stGoldenTouch:
							
				
				}
					
				
			}
			catch(Exception e)
			{	string s=e.ToString();
			}
			return oldTime;
		}
		
		
		
		
		
	public static void CopyTreeNodes(TreeView treeviewSource, TreeView treeviewDest)
	{ 	//DateTime t1=DateTime.Now;
		treeviewDest.Font=treeviewSource.Font;
		TreeNode newTn;
        
        foreach (TreeNode tn in treeviewSource.Nodes)
        {  
        	//newTn = (TreeNode)tn.Clone();
        	newTn = new TreeNode(tn.Text, tn.ImageIndex, tn.SelectedImageIndex);
        	newTn.NodeFont=tn.NodeFont;
        	newTn.Tag=tn.Tag;
        	if(tn.IsExpanded)newTn.Expand();	// je-li expanze, preda na zkopirovany treenode
        	setChildrenExpansion(newTn,tn);
        	//CopyChildren(newTn, tn);
            treeviewDest.Nodes.Add(newTn);
            //treeviewDest.Nodes.Add(newTn.Nodes[0]);
        }
        //DateTime t2=DateTime.Now;
        //TimeSpan diff1 = t2.Subtract(t1);
    }
	
	private static void setChildrenExpansion(TreeNode parent, TreeNode original)
    {   TreeNode newTn;
		foreach (TreeNode tn in original.Nodes)
		{   //newTn = (TreeNode)tn.Clone();
			newTn = new TreeNode(tn.Text, tn.ImageIndex, tn.SelectedImageIndex);
			newTn.NodeFont=tn.NodeFont;
        	newTn.Tag=tn.Tag;
        	
            parent.Nodes.Add(newTn);
           	if(tn.IsExpanded) parent.Nodes[tn.Index].Expand();// je-li expanze, preda na zkopirovany treenode
        	// setChildrenExpansion(parent.Nodes[tn.Index], tn);
            setChildrenExpansion(newTn, tn);
        }
    }
	 
    /*private static void CopyChildren(TreeNode parent, TreeNode original)
    {   TreeNode newTn;
        foreach (TreeNode tn in original.Nodes)
        {   //newTn = (TreeNode)tn.Clone();
            if(tn.IsExpanded)newTn.Expand();// je-li expanze, preda na zkopirovany treenode
        	
           // parent.Nodes.Add(newTn);
            CopyChildren(newTn, tn);
        }
    } */
    
    
    public static void SetFontToAllTreeNodes(TreeView treeview, Font font )	// refresuje font ve vsech uzlech treenode
	{ 	//DateTime t1=DateTime.Now;
    	Font bfont=new Font(font, FontStyle.Bold);
        foreach (TreeNode tn in treeview.Nodes)
        {  	if(A.JeDIR(tn))tn.NodeFont= bfont;	// slozky tucne
        	else  tn.NodeFont=font;					// ostatni normalne
            SetFontToChildren(tn, font, bfont);
        }
        //DateTime t2=DateTime.Now;
        //TimeSpan diff1 = t2.Subtract(t1);
        //int i=0;
    }
    private static void SetFontToChildren(TreeNode original, Font font, Font bfont)
    {   foreach (TreeNode tn in original.Nodes)
    	{   if(tn!=null)
    		{	if(tn.Tag!=null)
    			{	if(tn.Tag.GetType()==typeof(ReceptStruct.DirT))tn.NodeFont= bfont;	// slozky tucne
        			else  tn.NodeFont=font;					// ostatni normalne
    				SetFontToChildren(tn,font,bfont);
    			}
    		}
        }
    }
		
    
    
    
    
    
    public static void SetImageIndexToTreenode(TreeView treeview, string path, int imageindex )	// nastavi image index podle jmena cesty
	{ 	foreach (TreeNode tn in treeview.Nodes)
        {   SetImageIndexToTreenodeChildren(tn, path, imageindex);
        }
    }
    private static void SetImageIndexToTreenodeChildren(TreeNode original, string path, int imageindex)
    {   foreach (TreeNode tn in original.Nodes)
    	{   if(tn.FullPath==path)
    		{	tn.ImageIndex=imageindex;
    			tn.SelectedImageIndex=imageindex;
    		}
        }
    } 
    
    
		
/*	public static void CopyTreeNodes(TreeView treeview1, TreeView treeview2)
	{ 	//DateTime t1=DateTime.Now;
        TreeNode newTn;
        foreach (TreeNode tn in treeview1.Nodes)
        {   newTn = new TreeNode(tn.Text, tn.ImageIndex, tn.SelectedImageIndex,);
            CopyChildren(newTn, tn);
            treeview2.Nodes.Add(newTn);
        }
        //DateTime t2=DateTime.Now;
        //TimeSpan diff1 = t2.Subtract(t1);
        //int i=0;
    }
    public static void CopyChildren(TreeNode parent, TreeNode original)
    {   TreeNode newTn;
        foreach (TreeNode tn in original.Nodes)
        {   newTn = new TreeNode(tn.Text, tn.ImageIndex, tn.SelectedImageIndex);
            parent.Nodes.Add(newTn);
            CopyChildren(newTn, tn);
        }
    } 
*/		
		
		
		
	 public static T DeepTreeCopy<T>(T obj)
    {
        object result = null;
        using (var ms = new MemoryStream())
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);
            ms.Position = 0;
            result = (T)formatter.Deserialize(ms); ms.Close();
        }
        return (T)result;
    } 
        
        
    
	
/*		public static T DeepCopy(T other)
{
    using (MemoryStream ms = new MemoryStream())
    {
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(ms, other);
        ms.Position = 0;
        return (T)formatter.Deserialize(ms);
    }
}*/

	
	
	
	
	
	
public static int MezeTeplotyProstoru(int i, UtilitaPrekladReceptu.ReceptStruct.StepType st)
{	switch(st)
	{	case UtilitaPrekladReceptu.ReceptStruct.StepType.stHotAir:
				if(i<300)return 300;
				if(i>3000)return 3000;
				break;
		case UtilitaPrekladReceptu.ReceptStruct.StepType.stCombi:
				if(i<300)return 300;
				if(i>3000)return 3000;
				break;
		case UtilitaPrekladReceptu.ReceptStruct.StepType.stSteam:
				if(i<300)return 300;
				if(i>1300)return 1300;
				break;
	}
	return i;
}

public static int MezeTeplotyJadra(int i)
{	if(i<300)return 300;
	if(i>1100)return 1100;
	return i;
}
public static int MezeTeplotyDeltaT(int i)
{	if(i<200)return 200;
	if(i>700)return 700;
	return i;
}
public static int MezeCasuMinuty(int i)
{	if(i>24*60*60-1)return (24*60*60-1);
	if(i<1)return 1;
	return i;
}
public static int MezeCasuSekundy(int i)
{	if(i>900)return 900;
	if(i<1)return 1;
	return i;
}

public static int EasyCookingMezeVratMaximum(string s)	// meze pro easy cooking
{	switch(s)
	{	case "Time":		return (23*60+59)*60;
		case "ChamberT":	return 3000;
		case "DeltaT":		return 700;
		case "CoreT":		return 1100;
		case "Humidity":	return 100;
	}
	return 0;
}
public static int EasyCookingMezeVratMinimum(string s)
{	switch(s)
	{	case "Time":		return 1;
		case "ChamberT":	return 300;
		case "DeltaT":		return 200;
		case "CoreT":		return 300;
		case "Humidity":	return 0;
	}
	return 0;
}

		public static string EasyCookingVratString(int value, string typ)
		{	switch(typ)
			{	case "Time":	 return A.IntToTime(value, UtilitaPrekladReceptu.ReceptStruct.StepType.stHotAir);
				case "DeltaT": 	 return A.IntToTemperature(value);
				case "ChamberT": return A.IntToTemperature(value);
				case "CoreT":	 return A.IntToTemperature(value);
				case "Humidity": return A.IntToProcenta(value);
			}
			return "";
		}
		
		public static int EasyCookingStringToInt(string val, int oldVal, string typ)
		{	switch(typ)
			{	case "Time":		return A.StringToTimeInt(val, oldVal,UtilitaPrekladReceptu.ReceptStruct.StepType.stHotAir);			
				case "ChamberT":	return A.StringToTemperatureInt(val, oldVal);	
				case "CoreT":		return A.StringToTemperatureInt(val, oldVal);	
				case "DeltaT":		return A.StringToTemperatureInt(val, oldVal);	
				case "Humidity":	return A.StringToHumidityInt(val, oldVal);		
			}
			return 0;
		}

		public static Color EasyCookingVratEAColor(int n)
		{	switch(n)
			{	case 0: return Color.Red;
				case 1:	return Color.Green;
			}
			return Color.Blue;
		}
/*
int ConfTempAirMin   = 300;       // min., max. a vychozi teploty, vychozi vlhkost
int ConfTempAirMax   = 3000;
int ConfTempAirDef   = 1800;
int ConfTempCombiMin = 300;
int ConfTempCombiMax = 3000;
int ConfTempCombiDef = 1600;
int ConfTempSteamMin = 300;
int ConfTempSteamMax = 1300;
int ConfTempSteamDef = 990;
int ConfTempCoreMin  = 300;
int ConfTempCoreMax  = 1100;
int ConfTempCoreDef  = 750;
int ConfWetDef       = 500;

delta t 20-70°C default 50°C 
	 */
	
	
	
	
	
	public static int PrepoctiStarouRychlostVentilatoru(string stupenVent)
	{	switch(stupenVent)
		{	case "0":	return 0; 
			case "1":	return 50; 
			case "2":	return 70; 
			case "3":	return 85; 
			case "4":	return 100; 
			case "5":	return 110; 
		}
		return 0;
	}
	
	public static int PrepoctiNovouRychlostVentilatoruOrange(string stupenVent)
	{	switch(stupenVent)
		{	case "0":	return 0; 
			case "1":	return 40; 
			case "2":	return 50; 
			case "3":	return 60; 
			case "4":	return 70; 
			case "5":	return 80; 
			case "6":	return 90; 
			case "7":	return 100; 
		}
		try{	return int.Parse(stupenVent);}
		catch{}
		return 0;
	}
	
	
		// nakopiruje font z resources
	 	private static PrivateFontCollection private_fonts = new PrivateFontCollection();
	 	public static IntPtr unmanagedPointer;
        private void LoadFont()
        {  /*	byte[] fontdataSource =global::VisionCombiSW2.Resource.Roboto_Light;
        	IntPtr unmanagedFontPointer = Marshal.AllocHGlobal(fontdataSource.Length);
			Marshal.Copy(fontdataSource, 0, unmanagedFontPointer, fontdataSource.Length);
			private_fonts.AddMemoryFont(unmanagedFontPointer,fontdataSource.Length);*/
        }
        
        public static ToolTip NewToolTip()
        {	var tt= new ToolTip();
        	tt.InitialDelay=200;
			tt.AutoPopDelay=5000;
			tt.ReshowDelay=500;
			tt.ShowAlways=true;
        	return tt;
        }
        
        
        public static void DisableButton(Button b, Image OrigImg)
        {
        	Image img=SetImageOpacity((Image)OrigImg.Clone(),0.3F);
        	b.BackgroundImage=img;
			//b.Refresh();
        }
        
		public static Image SetImageOpacity(Image image, float opacity)  
		{	try  
		    {	Bitmap bmp = new Bitmap(image.Width, image.Height);  
		 		using (Graphics gfx = Graphics.FromImage(bmp)) 
		 		{   ColorMatrix matrix = new ColorMatrix();      
					matrix.Matrix33 = opacity;  
				   	ImageAttributes attributes = new ImageAttributes();      
					attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);    
					gfx.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
		        }
		        return bmp;  
		    }  
		    catch (Exception ex)  
		    {   MessageBox.Show(ex.Message);  
		        return null;  
		    }  
		} 
		
public static bool JeDIR(TreeNode tn)	// zjisti jestli se v reenode jedna o adresar
{	if(tn.Tag==null) return true;	// stara verze
	if(tn.Tag.GetType()==typeof(ReceptStruct.DirT))
	{	return true;
	}
	return false;
}


		
		
		
		public static void SetImageListDefaultProperty(ImageList im)
		{	im.ColorDepth=ColorDepth.Depth32Bit;;
			im.TransparentColor = System.Drawing.Color.Transparent;
			im.ImageSize=new Size(80,80);
		}
		
		public static Image ResizeImageToList(Image im)	// to musi byt kvuli win, aby to chodilo bo listbox se seka kdyz se do nej da obr se stejnou velikosti
		{ return (Image)new Bitmap(im,new Size(81,81));
		}
		
		
		
		public static string VratDalsiNazevNovaSlozka(TreeView tr, string newName)
		{	string s=newName;
			int ID=0;
			bool duplicita=false;
			
			if(tr.SelectedNode.Parent==null)	// koren
			{	foreach(TreeNode tnx in tr.Nodes)	// prohleda treenodes 
				{	//if((tnx.Text==newName) & (tnx!=tr.SelectedNode))	// duplicita
					if(tnx.Text==newName) 	// duplicita
					{	foreach(var tnx1 in tr.Nodes)	// prohleda treenodes - prvni cyklus
						{	ID++;
							s=newName+" ("+ID.ToString()+")"; // NOVY DALSI NAZEV
							duplicita=false;
							foreach(TreeNode tnx2 in tr.Nodes)	// prohleda treenodes - druhy cyklus
							{	if(tnx2.Text==s) duplicita=true;
							}
							if(!duplicita) return s;	// vrati novy nazev ktery neni v treeview
						}
					}
				}
			}
			else	// treenode
			{	var tn=tr.SelectedNode.Parent;
				foreach(TreeNode tnx in tn.Nodes)	// prohleda treenodes 
				{	if(tnx.Text==newName) 	// duplicita
					{	foreach(var tnx1 in tn.Nodes)	// prohleda treenodes - prvni cyklus
						{	ID++;
							s=newName+" ("+ID.ToString()+")"; // NOVY DALSI NAZEV
							duplicita=false;
							foreach(TreeNode tnx2 in tn.Nodes)	// prohleda treenodes - druhy cyklus
							{	if(tnx2.Text==s) duplicita=true;
							}
							if(!duplicita) return s;	// vrati novy nazev ktery neni v treeview
						}
					}
				}
			}
			return s;
		}
		
		public static string OdstranitMezery(string s)
		{	int i=-1;
			if(s!=null)
			{
				for(int x=0;x<s.Length;x++)	// zleva
				{	if(s.Substring(x,1)==" ")
					{	i=x;
					}
					else
					{	break;
					}
				}
				if(i>=0) // nasly se mezery
				{	s=s.Remove(0,i+1);
				}
				i=-1;
				for(int x=s.Length-1;x>=0;x--)	// zprava
				{	if(s.Substring(x,1)==" ")
					{	i=x;
					}
					else
					{	break;
					}
				}
				if(i>=0) // nasly se mezery
				{	s=s.Remove(i,s.Length-i);
				}
			}
			return s;
		}
		
	}
}
