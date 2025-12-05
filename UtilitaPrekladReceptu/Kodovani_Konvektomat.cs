/*
 * Created by SharpDevelop.
 * User: DavidPiska
 * Date: 18.9.2017
 * Time: 15:51
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;


namespace UtilitaPrekladReceptu
{
	/// <summary>
	/// Description of Kodovani_Konvektomat.
	/// </summary>
	public class Kodovani_Konvektomat
	{
		public Kodovani_Konvektomat()
		{
		}
		
		public static string VratKonvektomatCestu(string RefSoubor, string jazyk)
		{	string nsoubor=Path.GetFileName(RefSoubor);
			string s=Directory.GetCurrentDirectory();
			
		

			// rozdeleni do adresaru podle druhu programu
			if(nsoubor.StartsWith("Ref_EasyCooking"))
			{ s=s+"/__Konvektomat/EasyCooking";
			}
			else
			{	if(nsoubor.StartsWith("Ref_Programs"))
				{	s=s+"/__Konvektomat/Programs";
				}
				else
				{	if(nsoubor.StartsWith("Ref_Extras"))
					{	s=s+"/__Konvektomat/Extras";
					}
					else
					{ 	return "";	// nevi se co to je
					}
				}
			}
			
			
			
			// nazel jazyka
			s+="-"+jazyk;
			
			// koncovka
			s+=Path.GetExtension(RefSoubor);
			
			
			return s;
				
		}
	}
}
