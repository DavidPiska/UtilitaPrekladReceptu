/*
 * Created by SharpDevelop.
 * User: DavidPiska
 * Date: 16.1.2017
 * Time: 9:11
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
	/// Description of ParseProgram.
	/// </summary>
	public partial class ReceptParserBlueII
	{
		
		private bool LoadProgram(TreeNode nd, TreeView tr, XmlNode node, bool pridanaUrovenLoad)
		{	eaColorID=0;	// easy cooking color
			// novy program
			if(node.Attributes!=null)	// ma atributy
			{
				nd=new TreeNode();
			
		   		//nd.NodeFont=A.GetFontBySize();
		   		if(tr.SelectedNode!=null){	tr.SelectedNode.Nodes.Add(nd);	}
				else	{	tr.Nodes.Add(nd);}
 				tr.SelectedNode=nd;	// nastavi tuto uroven jako aktualni
 				pridanaUrovenLoad=true;
		   		if(tr.SelectedNode!=null)
				{	var ps=new ReceptStruct.ProgStruct();
		   			ps.t_typeOfProgram=ReceptStruct.t_TypeOfProgram.t_Program;	// typ programu
		   			ps.Node=tr.SelectedNode;
		   			
   					foreach (XmlAttribute att in node.Attributes)
			   	 	{	if(att.Name!=null)
   						{	switch(att.Name)
   							{	case "name":
												var cleaned = (att.Value ?? string.Empty).TrimEnd();
												ps.Name = cleaned;
												nd.Text = cleaned;
												A.VratText(cleaned);    // prida do slovniku receptu
                                    break;
   								case "f":		if(att.Value=="0")	ps.Favorit=false;	// favorit
   												else				ps.Favorit=true;
									break;
								case "preh":	ps.Predehrev=int.Parse(att.Value)*nasobekC;	// predehrev
									break;
   				 				case "gn":		ps.GastroNadobaNazev=att.Value;	// gastronadoba
   				 								ps.GastroNadobaIndex=0;	// obrazek se najde podle nazvu v seznamu pozdeji
									break;
   				 			}
   						}
   					}
						//ps.Path=tr.SelectedNode;
					if(pr!=null)
					{	pr.AddToDictionary(tr.SelectedNode, ps);	// prida program z treeview doprogramu
					}
				}
			}
			return pridanaUrovenLoad;
		}
		
		
		
		private void SaveParseProgram(XmlWriter w, TreeNode tn, ReceptStruct.ProgStruct ps )
		{
			w.WriteStartElement("Program");		//ex element
				w.WriteAttributeString("name",tn.Text);
				
				
				if(ps!=null)
				{	w.WriteAttributeString("f",A.BoolToInt(ps.Favorit).ToString());	// oblibeny
					w.WriteAttributeString("preh",ps.Predehrev.ToString());	// predehrev
					w.WriteAttributeString("gn",ps.GastroNadobaNazev);	// gastronadoba nazev
					if(ps.comment!=null	&& ps.comment!="")
					{	w.WriteStartElement("Comment");
						w.WriteString(ps.comment);
						w.WriteEndElement();
					}
				}
					
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
			
				if(ps!=null)
				{	// ******************* kroky programu
					if(ps.steps!=null)
					{	foreach(ReceptStruct.Step st in ps.steps)
						{	w.WriteStartElement("Step");	//element step
							
							w.WriteStartElement("Type");		w.WriteAttributeString("v",((int)st.Type).ToString()); w.WriteEndElement();
							w.WriteStartElement("EndBy");		w.WriteAttributeString("v",((int)st.EndBy).ToString()); w.WriteEndElement();
							w.WriteStartElement("Time");		w.WriteAttributeString("v",((int)st.Time).ToString()); 
																writeEasyCookingAtribute(w, st.ecTime);	// easy cooking
																w.WriteEndElement();
							w.WriteStartElement("ChamberT");	w.WriteAttributeString("v",((int)st.ChamberT).ToString()); 
																writeEasyCookingAtribute(w, st.ecChamberT);	// easy cooking
																w.WriteEndElement();
							w.WriteStartElement("CoreT");		w.WriteAttributeString("v",((int)st.CoreT).ToString()); 
																writeEasyCookingAtribute(w, st.ecCoreT);	// easy cooking
																w.WriteEndElement();
							w.WriteStartElement("Humidity");	w.WriteAttributeString("v",((int)st.Humidity).ToString()); 
																writeEasyCookingAtribute(w, st.ecHumidity);	// easy cooking
																w.WriteEndElement();
							w.WriteStartElement("FanSpeed");	w.WriteAttributeString("v",((int)st.FanSpeed).ToString()); w.WriteEndElement();
							w.WriteStartElement("FanTact");		w.WriteAttributeString("v",A.BoolToInt(st.FanCycle).ToString()); w.WriteEndElement();
							w.WriteStartElement("DeltaT");		w.WriteAttributeString("v",((int)st.DeltaT).ToString()); 
																writeEasyCookingAtribute(w, st.ecDeltaT);	// easy cooking
																w.WriteEndElement();
							w.WriteStartElement("Flap");		w.WriteAttributeString("v",A.BoolToInt(st.Flap).ToString()); w.WriteEndElement();
							w.WriteStartElement("E2");			w.WriteAttributeString("v",((int)st.E2).ToString()); w.WriteEndElement();
							w.WriteStartElement("Beep");		w.WriteAttributeString("v",A.BoolToInt(st.Beep).ToString()); w.WriteEndElement();
							if((st.Note!=null)&&(st.Note!=""))
							{	w.WriteStartElement("RingTone");		
										w.WriteAttributeString("v","1");
										w.WriteAttributeString("t",st.Note);
								w.WriteEndElement();
								
							}
							if(st.Water!=0)
							{	w.WriteStartElement("WaterVolume");		w.WriteAttributeString("v",((int)st.Water).ToString()); w.WriteEndElement();
								w.WriteStartElement("Injections");		w.WriteAttributeString("v",((int)st.n).ToString()); w.WriteEndElement();
							}
							
							w.WriteEndElement();	// element step konec
						}
					}
					
					
				}
				
		}
	}
}
