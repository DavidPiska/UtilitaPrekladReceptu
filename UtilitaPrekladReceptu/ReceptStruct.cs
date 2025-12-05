/*
 * Created by SharpDevelop.
 * User: DavidPiska
 * Date: 15.4.2015
 * Time: 12:26
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace UtilitaPrekladReceptu
{
	/// <summary>
	/// Description of ReceptStruct.
	/// </summary>
	public class ReceptStruct
	{
		public ReceptStruct()
		{
		}
		
		public enum t_TypeOfProgram	// druh programu
		{	t_Program			=0,		// standardni program
			t_RackTiming		=1,		// casovani zasuvu
			t_Sosuse_vide		=2,		// vareni ve vakuu
			t_Confit 			=3,		// konfitovani
			t_Sterilization		=4,		// sterilizace
			t_Drying			=5,		// suseni
			t_Smoking			=6		// uzeni
		}
		
		
		public enum StepType	// typy jednotlivych kroku programu
		{	stHotAir      = 0,	// horky vzduch	//ukonceni StepEndBy
		 	stCombi       = 1,	// kombinovany	//ukonceni StepEndBy
		 	stSteam       = 2,	// para			//ukonceni StepEndBy
		  	stCookAndHold = 3,	// StepEndBy nema smysl
		  	stGoldenTouch = 4,	// StepEndBy nema smysl // zatim neni
		  	stInjection   = 5,	// StepEndBy nema smysl
		  	stPause       = 6,	// StepEndBy nema smysl
		  	stNote        = 7,	// StepEndBy nema smysl
		};

		public enum StepEndBy		// jak se ukonci krok
		{	seByTime      = 0,		// ukonceni kroku podle Time, ChamberT
	  		seByCoreProbe = 1,		// ukonceni az se dosahne teploty v jadre CoreT
	  		seByDeltaT    = 2,		// ukonceni az dosahne CoreT tak se ukonci krok, teplota v prostoru je vetsi o DeltaT
	 	};
		
		public enum RackTimingEndBy	// jak se ukonci rack timing
		{	seByTime      = 0,		// ukonceni kroku podle Time, ChamberT
	  		seByCoreProbe = 1,		// ukonceni az se dosahne teploty v jadre CoreT
	  	};
	
		public enum DirType		// zobrazeni adresarove struktury
		{	rows      	= 0,		// radky
	  		tiles 		= 1,		// dlazdice
	  		options    	= 2,		// menu
	 	};
		[Serializable]
		public class DirT:ICloneable
		{	public DirType dType;	// typ zobrazeni adresare
			public object Clone()
		    {	return this.MemberwiseClone();
		    }
		}
	
		[Serializable]
		public class Step:ICloneable
		{	public StepType Type;			// typ kroku
			public StepEndBy EndBy;		// ukonceni 
			public int Time;		// sekundy
			public int ChamberT;	// požadovana teplota prostoru komory
			public int CoreT;		// požadovaná uvnitr potraviny
			public int Humidity;	// požadovaná vlhkost
			public int FanSpeed;	// rychlost ventilátoru
			public int DeltaT;		// rozdíl teplot ChamberT=CoreT+DeltaT
			public bool Flap;		// klapka otevřeno/zavřeno
			public int E2;			// !
			public bool Beep;		// ! zvuková signalizace po u končení kroku - uz neni nikde
			public string Note;		// poznamka - pribyla
			public int Water;		// mnozstvi vody
			public int n;			// pocet vstriku
			public bool FanCycle;	// taktovani ventilatoru
			public bool DefaultniHodnoty=false;	// true - pri prvotnim vyberu rezimu se automaticky nastavuji defaultni hodnoty
			
			public EasyCooking ecTime;		// easy cooking bargraf
			public EasyCooking ecChamberT;
			public EasyCooking ecDeltaT;
			public EasyCooking ecCoreT;
			public EasyCooking ecHumidity;
			
			
			
			public object Clone()
		    {	return this.MemberwiseClone();
		    }
		}
		
		[Serializable]
		public class RackTimingTimer:ICloneable
		{	public string Name;			// jmeno
			public RackTimingEndBy EndBy;		// ukonceni 
			public int Time;		// sekundy
			public int CoreT;		// požadovaná uvnitr potraviny
			public bool DefaultniHodnoty=false;	// true - pri prvotnim vyberu rezimu se automaticky nastavuji defaultni hodnoty
			
			public object Clone()
		    {	return this.MemberwiseClone();
		    }
		}

		
		[Serializable]
		public class ProgStruct:ICloneable
		{	//public string Path;		// uplna cesta v treeview
			public t_TypeOfProgram t_typeOfProgram;	// typ programu
			public TreeNode Node;	// uzel treenode
			public string Name;		// jmeno programu
			public string comment;	// napsany recept pripravy jidla
			public int Index;		// pro orange, index receptu
			public bool Favorit;	// oblibene
			public int	Predehrev;			// predehrev -1 vypnuto, 0 zapnuto, hodnota se bere podle nastaveni v konvektomatu, 30-300 rucne nastavena teplota
			public int 	GastroNadobaIndex;		// jaka je u varneho programu doporucena gastronadoba
			public string GastroNadobaNazev;
			//public string PictogramName;	// jmeno piktogramu uz nebude
			public List<Step> steps;			// seznam kroku
			public List<RackTimingTimer> RackTimingTimerSteps;	// seznam kroku casovace Rack Timing
			public object Clone()
		    {	return this.MemberwiseClone();
		    }
		}
		
		
		// * ************* easy cooking  *********************
		
		[Serializable]
		public class EasyCooking:ICloneable  
		{	public string EasyCookingValueType;		// co je to za hodnotu, aby se mohly nastavit meze
			public sEasyCooking_Seek_Bar seek_bar;		// seek bar
			public sEasyCooking_Options options;		// options
			public sEasyCooking_Seek_Bar_With_Options seek_bar_with_options;	// seek bar with options	
			public System.Drawing.Color eaColor;
		    public object Clone()
		    {	return this.MemberwiseClone();
		    }
		}
		
		public enum Bt		// typ bargrafu
		{	seek_bar     = 0,		
	  		options		 = 1,		
	  		seek_bar_with_options    = 2,		
	 	};
		
		[Serializable]
		public class sEasyCooking_Seek_Bar:ICloneable  // ********************
		{	public string name;	// nazev 
			public int min;		// hodnota min
			public int max;		// hodnota max
			public string minc;	// nazev min		
			public string maxc;	// nazev max
			public object Clone()
		    {	return this.MemberwiseClone();
		    }
		}
		
		[Serializable]
		public class ssEasyCooking_options_Step:ICloneable	// krok pro easy cooking options
		{	public int v;		// value
			public string n;	// text
			public object Clone()
		    {	return this.MemberwiseClone();
		    }
		}
		
		[Serializable]
		public class sEasyCooking_Options:ICloneable	// ********************
		{	public string name;	// nazev 
			public int min;		// hodnota min
			public int max;		// hodnota max
			public List<ssEasyCooking_options_Step> steps;			// seznam kroku
			public object Clone()
		    {	return this.MemberwiseClone();
		    }
		}
		
		[Serializable]
		public class ssEasyCooking_seek_bar_with_options_Step:ICloneable	// krok pro easy cooking seek bar with options
		{	public int v;		// value
			public int min_v;	// value
			public int max_v;	// value
			public string n;	// text
			public object Clone()
		    {	return this.MemberwiseClone();
		    }
		}
		
		[Serializable]
		public class sEasyCooking_Seek_Bar_With_Options:ICloneable  // ********************
		{	public string name;	// nazev 
			public int min;		// hodnota min
			public int max;		// hodnota max
			public List<ssEasyCooking_seek_bar_with_options_Step> steps;			// seznam kroku
			public object Clone()
		    {	return this.MemberwiseClone();
		    }
		}
		
	}
}
