/*
 * Created by SharpDevelop.
 * User: DavidPiska
 * Date: 22.2.2017
 * Time: 10:02
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
	/// Description of mProgram.
	/// </summary>
	public class mProgram
	{
		public mProgram()
		{
		}
		
		private Dictionary<TreeNode,ReceptStruct.ProgStruct> dictionary=new Dictionary<TreeNode, ReceptStruct.ProgStruct>();
		
		public void AddCommentToDictionary(TreeNode nd, string comment)	// prida comment do slovniku
		{	ReceptStruct.ProgStruct ps=null;
			if(nd.Tag!=null)
			{	ps=(ReceptStruct.ProgStruct)nd.Tag;
			}
			else
			{	if(dictionary.ContainsKey(nd))	// existuje jen se zmeni
				{	ps=dictionary[nd];
				}
			}
			if(ps!=null)
			{	ps.comment=comment;
				dictionary[nd]=ps;
			}
		}
		
		public void AddToDictionary(TreeNode nd, ReceptStruct.ProgStruct ps)	// prida program do slovniku
		{	nd.Tag=ps;	// reference na recept do treenode
			
			if(dictionary.ContainsKey(nd))	// existuje jen se zmeni
			{	dictionary[nd]=ps;
			}
			else // neexistuje vytvori se novy
			{	dictionary.Add(nd,ps);
			}
		}
		
		
		public void AddNameToDictionary(TreeNode nd, string name)	// prida comment do slovniku
		{	ReceptStruct.ProgStruct ps=null;
			if(!A.JeDIR(nd))
			{	ps=(ReceptStruct.ProgStruct)nd.Tag;
			}
			else
			{	if(dictionary.ContainsKey(nd))	// existuje jen se zmeni
				{	ps=dictionary[nd];
				}
			}
			if(ps!=null)
			{	ps.Name=name;
				dictionary[nd]=ps;
			}
		}
		
		public ReceptStruct.ProgStruct ReturnProgStruct(TreeNode nd)
		{	ReceptStruct.ProgStruct ps=null;
			if(nd.Tag!=null)
			{	ps=(ReceptStruct.ProgStruct)nd.Tag;
			}
			else
			{	if(dictionary.ContainsKey(nd))	// existuje jen se zmeni
				{	ps=dictionary[nd];
				}
			}
			if(ps!=null)
			{	return dictionary[nd];
			}
			return null;
		}
		
		public void AddStepToDictionary(TreeNode nd, ReceptStruct.RackTimingTimer rstep)	// prida rack timing krok programu do slovniku
		{	ReceptStruct.ProgStruct ps=null;
			if(nd.Tag!=null)
			{	ps=(ReceptStruct.ProgStruct)nd.Tag;
			}
			else
			{	if(dictionary.ContainsKey(nd))	// existuje jen se zmeni
				{	ps=dictionary[nd];
				}
			}
			if(ps!=null)
			{	if(ps.RackTimingTimerSteps==null)ps.RackTimingTimerSteps=new List<ReceptStruct.RackTimingTimer>();
				ps.RackTimingTimerSteps.Add(rstep);
				dictionary[nd]=ps;
			}
		}
		

		public void AddStepToDictionary(TreeNode nd, ReceptStruct.Step step)	// prida krok programu do slovniku
		{	ReceptStruct.ProgStruct ps=null;
			if(nd.Tag!=null)
			{	ps=(ReceptStruct.ProgStruct)nd.Tag;
			}
			else
			{	if(dictionary.ContainsKey(nd))	// existuje jen se zmeni
				{	ps=dictionary[nd];
				}
			}
			if(ps!=null)
			{	if(ps.steps==null)ps.steps=new List<ReceptStruct.Step>();
				ps.steps.Add(step);
				dictionary[nd]=ps;
			}
		}
		public ReceptStruct.ProgStruct FindInDictionary(TreeNode nd)	// hleda ve slovniku podle treenode
		{	if(nd.Tag!=null) return (ReceptStruct.ProgStruct)nd.Tag;
			if(dictionary.ContainsKey(nd))	// existuje jen se zmeni
			{	return (ReceptStruct.ProgStruct)dictionary[nd];
			}
			else return null;	// neexistuje 
		}
		
	}
}
