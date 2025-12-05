/*
 * Created by SharpDevelop.
 * User: DavidPiska
 * Date: 4.3.2015
 * Time: 9:30
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;


namespace UtilitaPrekladReceptu
{
	/// <summary>
	/// Description of ReceptParser.
	/// </summary>
	public class ReceptParser
	{	
		public List<Image> Piktogramy=new List<Image>();		// kolekce vsech piktogramu v originalni velikosti
		public List<string> PiktogramyName=new List<string>();		// jmena
		
		
		public ReceptParser()
		{	program1=null;
			treeView1=null;
		}
		
		public ReceptParser(mProgram prog1, TreeView tre1)
		{
			program1=prog1;	// predani referenci
			treeView1=tre1;
		}
		
		
		public enum tTypStroje
		{	nic,
			VisionI_Blue,
			VisionII_Blue,
			VisionI_Orange,
			VisionII_Orange,
		}
		
		public tTypStroje TypStroje=tTypStroje.nic;
		public mProgram program1;
		public TreeView treeView1;
			
		
		
		
	
			
			
		
	}
}
