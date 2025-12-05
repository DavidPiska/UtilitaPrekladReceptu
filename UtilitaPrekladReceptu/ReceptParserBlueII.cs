/*
 * Created by SharpDevelop.
 * User: DavidPiska
 * Date: 25.11.2014
 * Time: 8:25
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
	public partial class ReceptParserBlueII : IDisposable
	{
		private bool IsDisposed;
		public virtual void Dispose()
	    {   if (!this.IsDisposed)
	        {
				this.IsDisposed = true;
	        }
	    }
		
		
		private const int nasobekC=1;	// doslo ke zmene stupnu 10x takze pri pouziti konstanty lze prevest nacist stare recepty
		//private const int nasobekC=10;
		
		
		//public ImageList Piktogramy=new ImageList();		// kolekce vsech piktogramu v originalni velikosti
		//public List<string> PiktogramyName=new List<string>();		// jmena
		
		ReceptParser rp=new ReceptParser();
		public ReceptParserBlueII()
		{	
			
			// inicializace piktogramu treeview
			
			/*rp.Piktogramy.Images.Add(global::VisionCombiSW2.Resource.blankFile);
			rp.PiktogramyName.Add("Prazdny soubor");
			rp.Piktogramy.Images.Add(global::VisionCombiSW2.Resource.directory);
			rp.PiktogramyName.Add("slozka");*/
			
		}
		
		
		//public string vystup="";
		
		private int eaColorID;	// easy cooking color
			
		private void GetChildren(mProgram pr, TreeView tr, XmlNode node)
		{	var nd=new TreeNode();
			var p=new PNGparser();
			bool pridanaUrovenLoad=false;
			
		   	switch(node.LocalName)
		  	{		case "Programs":	
		   			
		   				break;
		   			case "Category":
		   				if(node.Attributes!=null)	// ma atributy
		   				{	var di=new ReceptStruct.DirT();	// kategorie slozky
		   					foreach (XmlAttribute att in node.Attributes)
					   	 	{	if(att.Name!=null)
		   				 		{	if(att.Name=="name")
		   							{	//eaColorID=0;	// easy cooking color
		   								if(att.Value!=null)
		   				 				{	//nd=new TreeNode(att.Value.ToString()+" ");
		   									if(att.Value=="")
		   									{	nd=new TreeNode("                               ");
		   										if(tr.SelectedNode!=null){	tr.SelectedNode.Nodes.Add(nd);	}
		   										else	{	tr.Nodes.Add(nd);}
		   										tr.SelectedNode=nd;	// nastavi tuto uroven jako aktualni
		   										tr.SelectedNode.ImageIndex=2;	// obrazek slozky
			   				 					tr.SelectedNode.SelectedImageIndex=2;// obrazek slozky
			   				 					tr.SelectedNode=null;
		   									}
		   									else 
		   									{	nd=new TreeNode(att.Value.ToString());
		   										A.VratText(att.Value.ToString());	// prida do slovniku receptu
		   										if(tr.SelectedNode!=null){	tr.SelectedNode.Nodes.Add(nd);	}
		   										else	{	tr.Nodes.Add(nd);}
			   									//lastTreeNode=nd;
			   									pridanaUrovenLoad=true;
			   				 					tr.SelectedNode=nd;	// nastavi tuto uroven jako aktualni
			   				 					tr.SelectedNode.ImageIndex=1;	// obrazek slozky
			   				 					tr.SelectedNode.SelectedImageIndex=1;// obrazek slozky
			   				 				//	tr.SelectedNode.NodeFont= A.GetFontBySize(FontStyle.Bold);
			   				 				//	tr.SelectedNode.NodeFont = new System.Drawing.Font(tr.SelectedNode.TreeView.Font, tr.SelectedNode.TreeView.Font.Style | FontStyle.Bold);
		   									}
		   									
		   				 				}
		   				 			}
		   							if(att.Name=="lt")	// kategorie slozky
		   				 			{	if(att.Value!=null && att.Value!="")
		   								{	switch(att.Value)
		   									{	case "rows": di.dType=ReceptStruct.DirType.rows; break;
		   										case "tiles": di.dType=ReceptStruct.DirType.tiles; break;
		   										case "options": di.dType=ReceptStruct.DirType.options; break;
		   									}
		   								}
		   							}
		   				 		}
		   				 	}
		   					if(nd!=null)
		   					{	nd.Tag=di;	// prida informace o zobrazeni adresare
		   					}
		   				 }
		   				break; 
		   				
		   				
		   			case "Program":		pridanaUrovenLoad=LoadProgram(nd ,tr, node, pridanaUrovenLoad);	// rozparsuje program
		   				break;
		   			case "RackTiming":	pridanaUrovenLoad=LoadRackTiming(nd ,tr, node, pridanaUrovenLoad);	// rozparsuje rack timing program
		   				break;
		   				
		   				
		   			case "Pictogram":
		   				if(node.Attributes!=null)	// ma atributy
			    		{	foreach (XmlAttribute att in node.Attributes)
					   	 	{	if(att.Name!=null)
		   				 		{	/*if(att.Name=="p")	// nazev piktogramu uz nebude
		   				 			{	pr.AddPictogramNameToDictionary(tr.SelectedNode,att.Value);
		   								PiktogramyName.Add(att.Value);
		   							}*/
		   							if(att.Name=="t")	// data PNG obrazku
		   							{ 	Image im=p.ReturnImage(att.OwnerElement.InnerText);
		   								//Metody.Images.SaveImage(im);
		   								rp.Piktogramy.Add(im);	// do piktogramu se ulozi v lne velikosti
		   								//Metody.Images.SaveImage(rp.Piktogramy.Images[rp.Piktogramy.Images.Count-1]);
		   								
		   								rp.PiktogramyName.Add(tr.SelectedNode.Text);
		   								if(tr.ImageList!=null)
		   								{	tr.ImageList.Images.Add(im);
		   									if(tr.SelectedNode!=null)
		   									{	tr.SelectedNode.ImageIndex=tr.ImageList.Images.Count-1;	// do poslednidniho uzlu ulozi index aktualniho obrazku
			   				 					tr.SelectedNode.SelectedImageIndex=tr.ImageList.Images.Count-1;	// do poslednidniho uzlu ulozi index aktualniho obrazku
		   									}
		   								}
		  							}
		   						}
		   					}
		   				}
		   				break;
		   				
		   			case "Comment":
		   				if(node.InnerText!=null)
		   				{	if(tr.SelectedNode!=null)
		   					{	pr.AddCommentToDictionary(tr.SelectedNode, node.InnerText);	// prida program z treeview doprogramu
		   						A.VratText( node.InnerText);	// prida do slovniku receptu
		   					}
		   				}
		   				break;
		   			
					case "Step":
		   				if(node.Attributes!=null)	// ma atributy
		   				{	ReceptStruct.Step step=new ReceptStruct.Step();
		   					foreach (XmlNode item in node.ChildNodes)
		   					{	if(item.Name!=null)
		   						{	switch(item.Name.ToString())
		   							{	case "Type": 		step.Type=(ReceptStruct.StepType)	int.Parse((string)returnNodeValue_v(item));	break;
		   								case "EndBy": 		step.EndBy=(ReceptStruct.StepEndBy)	int.Parse((string)returnNodeValue_v(item));	break;
		   								case "Time": 		step.Time=		int.Parse((string)returnNodeValue_v(item));	
		   													step.ecTime=	parseEasyCooking(item);	// dekoduje easycooking atributy
		   													break;
		   								case "ChamberT": 	step.ChamberT=	int.Parse((string)returnNodeValue_v(item))*nasobekC;	
		   													step.ecChamberT=parseEasyCooking(item);	// dekoduje easycooking atributy
		   													break;
		   								case "CoreT": 		step.CoreT=		int.Parse((string)returnNodeValue_v(item))*nasobekC;	
							   								step.ecCoreT=	parseEasyCooking(item);	// dekoduje easycooking atributy
							   								break;
		   								case "Humidity": 	step.Humidity=	int.Parse((string)returnNodeValue_v(item));	
							   								step.ecHumidity=parseEasyCooking(item);	// dekoduje easycooking atributy
							   								break;
		   								case "FanSpeed": 	step.FanSpeed=	int.Parse((string)returnNodeValue_v(item));	break;
		   								case "FanTact": 	if(int.Parse((string)returnNodeValue_v(item))==0)
							   								{	step.FanCycle=false;}
															else
															{	step.FanCycle=true;
															}	
															break;
										case "DeltaT": 		step.DeltaT=	int.Parse((string)returnNodeValue_v(item))*nasobekC;	
															step.ecDeltaT=	parseEasyCooking(item);	// dekoduje easycooking atributy
															break;
		   								case "Flap": 		if(int.Parse((string)returnNodeValue_v(item))==0)
							   								{	step.Flap=false;
		   													}
															else
															{	step.Flap=true;
															}	
															break;
		   								case "E2": 			step.E2=		int.Parse((string)returnNodeValue_v(item));	break;
		   								case "Beep": 		if(int.Parse((string)returnNodeValue_v(item))==0)
							   								{	step.Beep=false;}
															else
															{	step.Beep=true;
															}	
															break;
										case "Note": 		step.Note=(string)returnNodeValue_v(item);
															A.VratText((string)returnNodeValue_v(item));	// prida do slovniku receptu
															 break;
							   			case "RingTone": 	
										if((string)returnNodeValue_v(item)=="1")	// je poznamka
															{	step.Note=(string)returnNodeValue_t(item); 
																A.VratText((string)returnNodeValue_t(item));	// prida do slovniku receptu
															
															}
															break;
							   			case "WaterVolume": step.Water=		int.Parse((string)returnNodeValue_v(item));	break;
		   								case "Injections": 	step.n=			int.Parse((string)returnNodeValue_v(item));	break;
		   							}
		   						}
					       	}
		   					if(pr!=null )pr.AddStepToDictionary(tr.SelectedNode,step);
		   				}
		   				break;
		   				case "Timer":		// rack timing
		   				if(node.Attributes!=null)	// ma atributy
		   				{	ReceptStruct.RackTimingTimer rstep=new ReceptStruct.RackTimingTimer();
		   					
		   					foreach (XmlAttribute att in node.Attributes)
					   	 	{	if(att.Name!=null)
		   				 		{	if(att.Name=="name")	// nazev kroku racktimingu
		   							{	rstep.Name=		att.Value.ToString();
		   								A.VratText((string)att.Value.ToString());	// prida do slovniku receptu
									}
		   						}
		   					}
		   					foreach (XmlNode item in node.ChildNodes)
		   					{	if(item.Name!=null)
		   						{	switch(item.Name.ToString())
		   							{	//case "Type": 		step.Type=(ReceptStruct.StepType)	int.Parse((string)returnNodeValue(item));	break;
		   								case "EndBy": 		rstep.EndBy=(ReceptStruct.RackTimingEndBy)	int.Parse((string)returnNodeValue_v(item));	break;
		   								case "Time": 		rstep.Time=		int.Parse((string)returnNodeValue_v(item));	break;
		   								case "CoreT": 		rstep.CoreT=	int.Parse((string)returnNodeValue_v(item))*nasobekC;		break;
		   								//case "name": 		rstep.Name=		item.ToString();		break;
		   							}
		   						}
					       	}
		   					if(pr!=null )pr.AddStepToDictionary(tr.SelectedNode,rstep);
		   				}
		   				break;
		   				
		   	}
		   	
		   	
		   	
		    /* if(node.Name!=null)	vystup+=node.Name.ToString()+" ";
		    	//if(node.Value!=null)vystup+=node.Value.ToString();
		  	    if(node.Attributes!=null)	// ma atributy
			    {	vystup+="(";
			   		foreach (XmlAttribute att in node.Attributes)
			   	 	{	if(att.Name!=null)vystup+=att.Name.ToString()+"=";
			    		if(att.Value!=null)vystup+=att.Value.ToString()+",";
			    	}
			    	vystup+=")";
			    }
		    	vystup+=Environment.NewLine;*/
		    	
		       foreach (XmlNode item in node.ChildNodes)
		       {    GetChildren(pr, tr, item);
		             
		       }
		       
		      // if(lastTreeNode!=null)
		      if(pridanaUrovenLoad)
		      {	if(tr.SelectedNode!=null)tr.SelectedNode=tr.SelectedNode.Parent;	// o uroven zpet
		      }
		}
		
		mProgram pr;
		public void loadXML(string cesta)
		{	pr=rp.program1;
			if(pr==null)
		   	{	pr=new mProgram();
		   	}
			TreeView treeView=rp.treeView1;
			
			if(cesta!=null)
			{	if(!File.Exists(cesta)) 
				{	//MessageBox.Show(cesta,A.VratText("Soubor nebyl nalezen"),MessageBoxButtons.OK,MessageBoxIcon.Error);
					rp.treeView1=null;
					return;
				}
			}
			
			try
			{	
				/*rp.Piktogramy.Clear();
				rp.PiktogramyName.Clear();
				rp.Piktogramy.Add(global::VisionCombiSW2.Resource.blankFile);
				rp.PiktogramyName.Add("Prazdny soubor");
				rp.Piktogramy.Add(global::VisionCombiSW2.Resource.directory);
				rp.PiktogramyName.Add("slozka");
				rp.Piktogramy.Add(global::VisionCombiSW2.Resource.home);
				rp.PiktogramyName.Add("home");
				rp.Piktogramy.Add(global::VisionCombiSW2.IkonyTlacitek.CasovaniZasuvu_W);	// 3 racktiming
				rp.PiktogramyName.Add("IkonaRackTiming");*/
			
				if(treeView==null)treeView=new TreeView();
				treeView.Nodes.Clear();	// smaze stare data
				/*var tnroot=new TreeNode("...");	// hlavni adresar na zacatku stromu
				treeView.Nodes.Add(tnroot);*/
								
				//Font f=new Font("Microsoft Sans Serif",20,FontStyle.Bold);
				
				//treeView.Font= f;
				//treeView.Font=A.GetFontBySize(FontStyle.Bold);
				if(cesta==null) return;
				treeView.BeginUpdate();
					
				var xmlDoc = new XmlDocument();
			    xmlDoc.PreserveWhitespace=false;
			    xmlDoc.Load(cesta);
			    foreach (XmlNode item in xmlDoc.ChildNodes)
			    {	
			    	
			    	GetChildren(pr, treeView, item);
				}
			    //MessageBox.Show(counter.ToString());
			    treeView.CollapseAll();
				treeView.EndUpdate();
			    return;
			}
			catch(Exception e)
			{	MessageBox.Show(A.VratText("Soubor")+" "+cesta+" "+A.VratText("není ve správném formátu")+Environment.NewLine+Environment.NewLine+e.Message,A.VratText("Soubo není ve správném formátu"),MessageBoxButtons.OK,MessageBoxIcon.Error);
				treeView.EndUpdate();
				return;
			}//*/
	
		}
				
		
		
	
		
		private object returnNodeValue_t(XmlNode node)
		{	if(node.Attributes!=null)	// ma atributy
			{	foreach (XmlAttribute att in node.Attributes)
			 	{	if(att.Name!=null)
			 		{	if(att.Name=="t")	// hodnota
						{	return att.Value;
						}
					}
				}
			}
			return null;
		}
		
		private object returnNodeValue_v(XmlNode node)
		{	if(node.Attributes!=null)	// ma atributy
			{	foreach (XmlAttribute att in node.Attributes)
			 	{	if(att.Name!=null)
			 		{	if(att.Name=="v")	// hodnota
						{	return att.Value;
						}
					}
				}
			}
			return null;
		}
		
		
		
		/// <summary>
		/// ulozi XML
		/// </summary>
		/// <param name="pr"></param>
		/// <param name="treeView"></param>
		/// <param name="cesta"></param>
		public void SaveXML(string cesta)
		{	mProgram pr=rp.program1;
			TreeView tr=rp.treeView1;
		
			XmlWriterSettings settings=new XmlWriterSettings();
			settings.Indent=true;			//oddelovani
			settings.IndentChars=" ";		// oddelovac
			
			settings.Encoding=new System.Text.UTF8Encoding(false);	// false odmita UTF-8 BOM znacku na zacatku
			XmlWriter w = XmlWriter.Create(cesta,settings);
			
			
			w.WriteStartElement("Programs");
			w.WriteAttributeString("version","0.1");	//e1 verze souboru
			
			w.WriteStartElement("Category");		//e2 soubor zacina prazdnou kategorii
			
			w.WriteAttributeString("name","");
			
			foreach(TreeNode tn in tr.Nodes)	// prohleda vsechny uzly a synchronizuje s dictionary
			{	if(tn.Text=="                               ")	// domu je posunuta uroven
				{	if(tn.Tag.GetType()==typeof(ReceptStruct.DirT))	// slozka
					{	// w.WriteStartElement("Category");		//ex element
						switch(((ReceptStruct.DirT)tn.Tag).dType)
						{	case ReceptStruct.DirType.rows:		w.WriteAttributeString("lt","rows"); break;
							case ReceptStruct.DirType.tiles:	w.WriteAttributeString("lt","tiles"); break;
							case ReceptStruct.DirType.options:	w.WriteAttributeString("lt","options"); break;
						}
					}
				}
				else
				{	ParseNodeToXMLwriter(w,tn,tr,pr);	//ex3
					ProhledejVsechnyPodskupinyTreeview(w,tn,tr,pr);	// hleda do hloubky v treeview
					w.WriteEndElement();	// ex3 konec
				}
			}
			
			w.WriteEndElement();	// e2 konec
			w.WriteEndElement();	// e1 konec
		    w.Close();
		}
		
		private void ProhledejVsechnyPodskupinyTreeview(XmlWriter w, TreeNode tno,TreeView tr, mProgram pr)
		{	foreach(TreeNode tn in tno.Nodes)	// prohleda vsechny uzly 
			{	ParseNodeToXMLwriter(w,tn,tr,pr);	//ex4
				ProhledejVsechnyPodskupinyTreeview(w,tn,tr,pr);
				w.WriteEndElement();	// ex4 konec
			}
		}
		
		private void ParseNodeToXMLwriter(XmlWriter w, TreeNode tn,TreeView tr, mProgram pr)
		{
			//if(tn.ImageIndex==1)	// slozka
			if(tn.Tag.GetType()==typeof(ReceptStruct.DirT))	// slozka
			{	w.WriteStartElement("Category");		//ex element
				switch(((ReceptStruct.DirT)tn.Tag).dType)
				{	case ReceptStruct.DirType.rows:		w.WriteAttributeString("lt","rows"); break;
					case ReceptStruct.DirType.tiles:	w.WriteAttributeString("lt","tiles"); break;
					case ReceptStruct.DirType.options:	w.WriteAttributeString("lt","options"); break;
				}
				
				w.WriteAttributeString("name",tn.Text);
				
				if(tn.ImageIndex>A.BluePrvniPiktogram)	// **************** je nejaka bitmapa
				{	w.WriteStartElement("Pictogram");		//bitmap element
					w.WriteAttributeString("t","png");
					//w.WriteAttributeString("p",ps.PictogramName);	// nazev piktogramu se neuklada
					PNGparser pn=new PNGparser();
					//w.WriteAttributeString(pn.ReturnStringFromImage(tr.ImageList.Images[tn.ImageIndex]));
					if(rp.Piktogramy.Count>tn.ImageIndex)
					{	w.WriteRaw(pn.ReturnStringFromImage(rp.Piktogramy[tn.ImageIndex]));
						w.WriteEndElement();	// bitmap element konec
					}
				}
			}
			else	// recept / rack timing a dalsi
			{	ReceptStruct.ProgStruct ps=pr.FindInDictionary(tn);
				
				switch(ps.t_typeOfProgram)
				{	case ReceptStruct.t_TypeOfProgram.t_Program:	// standardni program
							SaveParseProgram(w,tn,ps);	// standardni program
							break;
					case ReceptStruct.t_TypeOfProgram.t_RackTiming:	// rack timing
							SaveParseRackTiming(w,tn,ps);	// rack timing
							break;
				}
			}
		}
		
		
		
		// ********************************** easy cooking read *******************************************
		
		private ReceptStruct.EasyCooking parseEasyCooking(XmlNode item)
		{	var c=new ReceptStruct.EasyCooking();
			if(item.HasChildNodes==true)	// ma deti
			{	foreach (XmlNode item2 in item.ChildNodes)
				{	if(item2.Name=="BarGraf")	// je to easy cooking
					{	if(item2.Attributes!=null)	// ma atributy
						{	foreach (XmlAttribute att in item2.Attributes)
						 	{	if(att.Name!=null)
						 		{	if(att.Name=="bt")
									{	switch(att.Value)
										{	case "seek_bar":	// ************* seek bar *****************
												c.seek_bar=parseEasyCookingSeekBar(item2);
												if(c.seek_bar!=null){ c.eaColor=A.EasyCookingVratEAColor(eaColorID); eaColorID++;}
												return c;
											case "options":		// ************* options *****************
												c.options=parseEasyCookingOptions(item2);
												if(c.options!=null){ c.eaColor=A.EasyCookingVratEAColor(eaColorID); eaColorID++;}
												return c;
											case"seek_bar_with_options":	// ************* seek bar with options *****************
												c.seek_bar_with_options=parseEasyCookingSeekBarWithOptions(item2);
												if(c.seek_bar_with_options!=null){ c.eaColor=A.EasyCookingVratEAColor(eaColorID); eaColorID++;}
												return c;
										}
									}
								}
							}
						}
					}
				}
			}
			return c;	// neni easy cooking
		}
		
		private ReceptStruct.sEasyCooking_Seek_Bar parseEasyCookingSeekBar(XmlNode item)	// seek bar - easy cooking
		{	var c=new ReceptStruct.sEasyCooking_Seek_Bar();
			if(item.Attributes!=null)	// ma atributy
			{	foreach (XmlAttribute att in item.Attributes)
				{	switch(att.Name)
					{	case "name":	c.name=(string)att.Value;	
										A.VratText((string)att.Value);	// prida do slovniku receptu
										break;
						case "max":		c.max=int.Parse((string)att.Value);	break;
						case "min":		c.min=int.Parse((string)att.Value);	break;
						case "maxc":	c.maxc=(string)att.Value;	
										A.VratText((string)att.Value);	// prida do slovniku receptu
										break;
						case "minc":	c.minc=(string)att.Value;	
										A.VratText((string)att.Value);	// prida do slovniku receptu
										break;
					}
				}
			}	return c;
		}
		
		private ReceptStruct.sEasyCooking_Options parseEasyCookingOptions(XmlNode item)	// options - easy cooking
		{	var c=new ReceptStruct.sEasyCooking_Options();
			if(item.Attributes!=null)	// ma atributy
			{	foreach (XmlAttribute att in item.Attributes)
				{	switch(att.Name)
					{	case "name":	c.name=(string)att.Value;	
										A.VratText((string)att.Value);	// prida do slovniku receptu
										break;
						case "max":		c.max=int.Parse((string)att.Value);	break;
						case "min":		c.min=int.Parse((string)att.Value);	break;
					}
				}
				if(item.HasChildNodes==true)	// ma deti
				{	foreach (XmlNode item2 in item.ChildNodes)
					{	if(item2.Name=="Value")	// je tam dalsi hodnota
						{	var c2=new ReceptStruct.ssEasyCooking_options_Step();
							if(item2.Attributes!=null)	// ma atributy
							{	foreach (XmlAttribute att in item2.Attributes)
						 		{
									switch(att.Name)
									{	case "v":		c2.v=int.Parse((string)att.Value);	break;
										case "n":		c2.n=(string)att.Value;	
														A.VratText((string)att.Value);	// prida do slovniku receptu
														break;
									}
								}
							}
							if(c.steps==null)c.steps=new List<ReceptStruct.ssEasyCooking_options_Step>();
							c.steps.Add(c2);	// proda polozku
						}
					}
				}
			}	return c;
		}
		
		private ReceptStruct.sEasyCooking_Seek_Bar_With_Options parseEasyCookingSeekBarWithOptions(XmlNode item)	// seek bar + options - easy cooking
		{	var c=new ReceptStruct.sEasyCooking_Seek_Bar_With_Options();
			if(item.Attributes!=null)	// ma atributy
			{	foreach (XmlAttribute att in item.Attributes)
				{	switch(att.Name)
					{	case "name":	c.name=(string)att.Value;	
										A.VratText((string)att.Value);	// prida do slovniku receptu
										break;
						case "max":		c.max=int.Parse((string)att.Value);	break;
						case "min":		c.min=int.Parse((string)att.Value);	break;
					}
				}
				if(item.HasChildNodes==true)	// ma deti
				{	foreach (XmlNode item2 in item.ChildNodes)
					{	if(item2.Name=="Value")	// je tam dalsi hodnota
						{	var c2=new ReceptStruct.ssEasyCooking_seek_bar_with_options_Step();
							if(item2.Attributes!=null)	// ma atributy
							{	foreach (XmlAttribute att in item2.Attributes)
						 		{
									switch(att.Name)
									{	case "v":		c2.v=int.Parse((string)att.Value);	break;
										case "min_v":	c2.min_v=int.Parse((string)att.Value);	break;
										case "max_v":	c2.max_v=int.Parse((string)att.Value);	break;
										case "n":		c2.n=(string)att.Value;	
														A.VratText((string)att.Value);	// prida do slovniku receptu
														break;
									}
								}
							}
							if(c.steps==null)c.steps=new List<ReceptStruct.ssEasyCooking_seek_bar_with_options_Step>();
							c.steps.Add(c2);	// proda polozku
						}
					}
				}
			}	return c;
		}
		
		
		
		// ********************************** easy cooking write to xml *******************************************
		private void writeEasyCookingAtribute(XmlWriter w, ReceptStruct.EasyCooking ec)
		{	if(ec==null) return;	
			if(ec.seek_bar!=null)
			{	w.WriteStartElement("BarGraf");		
				w.WriteAttributeString("bt","seek_bar");
				if(ec.seek_bar.name!=null)w.WriteAttributeString("name",ec.seek_bar.name.ToString());
				w.WriteAttributeString("min",ec.seek_bar.min.ToString());
				w.WriteAttributeString("max",ec.seek_bar.max.ToString());
				w.WriteAttributeString("minc",ec.seek_bar.minc.ToString());
				w.WriteAttributeString("maxc",ec.seek_bar.maxc.ToString());
				w.WriteEndElement();
				return;
			}
			if(ec.options!=null)
			{	w.WriteStartElement("BarGraf");		
				w.WriteAttributeString("bt","options");
				if(ec.options.name!=null)w.WriteAttributeString("name",ec.options.name.ToString());
				w.WriteAttributeString("min",ec.options.min.ToString());
				w.WriteAttributeString("max",ec.options.max.ToString());
				if(ec.options.steps!=null)
				{	if(ec.options.steps.Count>0)
					{	foreach(ReceptStruct.ssEasyCooking_options_Step st in ec.options.steps)
						{	w.WriteStartElement("Value");
							w.WriteAttributeString("v",st.v.ToString());
							w.WriteAttributeString("n",st.n.ToString());
							w.WriteEndElement();
						}
					}
				}
				w.WriteEndElement();
				return;
			}
			if(ec.seek_bar_with_options!=null)
			{	w.WriteStartElement("BarGraf");		
				w.WriteAttributeString("bt","seek_bar_with_options");
				if(ec.seek_bar_with_options.name!=null)w.WriteAttributeString("name",ec.seek_bar_with_options.name.ToString());
				w.WriteAttributeString("min",ec.seek_bar_with_options.min.ToString());
				w.WriteAttributeString("max",ec.seek_bar_with_options.max.ToString());
				if(ec.seek_bar_with_options.steps!=null)
				{	if(ec.seek_bar_with_options.steps.Count>0)
					{	foreach(ReceptStruct.ssEasyCooking_seek_bar_with_options_Step st in ec.seek_bar_with_options.steps)
						{	w.WriteStartElement("Value");
							w.WriteAttributeString("v",st.v.ToString());
							w.WriteAttributeString("min_v",st.min_v.ToString());
							w.WriteAttributeString("max_v",st.max_v.ToString());
							w.WriteAttributeString("n",st.n.ToString());
							w.WriteEndElement();
						}
					}
				}
				w.WriteEndElement();
				return;
			}
		}
		
		
		
	}
}
