/*
 * Created by SharpDevelop.
 * User: DavidPiska
 * Date: 20.4.2017
 * Time: 14:25
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;

namespace UtilitaPrekladReceptu
{
	/// <summary>
	/// Description of Kodovani_ISO.
	/// </summary>
	public class Kodovani_ISO
	{
		public Kodovani_ISO()
		{
		}
		
		
		public static string VratISOCestu(string RefSoubor, string jazyk)
		{	string nsoubor=Path.GetFileName(RefSoubor);
			string s=Directory.GetCurrentDirectory();
			
			
			
			
///		PR15-0600-000-S001-cs.br2
///		-----						ISO zkratka
///			 --						Typ programu (05-easy cooking, 06-programy, 25-extras)
///			   --					cislo jazyka (00-cs,01-en)
///              ----				cislo zakaznika (000 - retigo)
///					 -S---			cislo verze
/// 					   --		kod jazyka textovy

			// rozdeleni do adresaru podle druhu programu
			if(nsoubor.StartsWith("Ref_EasyCooking"))
			{ s=s+"/__Easy Cooking/PR15-05";
			}
			else
			{	if(nsoubor.StartsWith("Ref_Programs"))
				{	s=s+"/__Programy/PR15-06";
				}
				else
				{	if(nsoubor.StartsWith("Ref_Extras"))
					{	s=s+"/__Extras/PR15-25";
					}
					else
					{ 	return "";	// nevi se co to je
					}
				}
			}
			
			// kod jazyka
			switch(jazyk)
			{	case "cs": s+="00"; break;
				case "en": s+="01"; break;
				case "de": s+="02"; break;
				case "ru": s+="03"; break;
				case "sk": s+="04"; break;
				case "pl": s+="05"; break;
				case "lt": s+="06"; break;
				case "bg": s+="07"; break;
				case "fr": s+="08"; break;
				case "no": s+="09"; break;
				case "da": s+="10"; break;
				case "hu": s+="11"; break;
				case "sr": s+="12"; break;
				case "it": s+="13"; break;
				case "es": s+="14"; break;
				case "nl": s+="15"; break;
				case "is": s+="16"; break;
				case "sl": s+="17"; break;
				case "ro": s+="18"; break;
				case "el": s+="19"; break;
				case "tr": s+="20"; break;
				case "zh-Hant": s+="21"; break;
				case "zh-Hans": s+="22"; break;
				case "sv": s+="23"; break;
				case "et": s+="24"; break;
				case "ko": s+="25"; break;
				case "hr": s+="26"; break;
				case "fi": s+="27"; break;
				case "he": s+="28"; break;
				case "vi": s+="29"; break;
				case "ja": s+="30"; break;
				case "lv": s+="31"; break;
				case "be": s+="32"; break;
				case "uk": s+="33"; break;
				case "kk": s+="34"; break;
				case "en-GB":	s+="35"; break;
				case "pt": s+="36"; break;
				case "nl-BE": s += "37"; break;
				case "fr_international": s += "38"; break;
				case "en_US": s += "39"; break;
				case "th": s += "40"; break;


				// cislovani vychaiz z tohoto souboru
				// t:\PDF_vykresy\ELEKTRO\PROGRAMY\PR15\PR15-03xx-Jazyky pro Blue Vision\TextsBlueVision-PR15-R2_105.xls
				// ja  lv  be  uk  kk  en_GB  pt nl_BE
				// 30  31  32  33  34  35     36  37



				default: return"";	// nenasel se jazyk
			}
			
			
			// cislo zakaznika
			s+="-000";
			
			// cislo verze
			string version=Path.GetFileName(RefSoubor);
			int i=version.LastIndexOf("_");
			int ii=version.LastIndexOf(".");
			if(i>0)
			{	version=version.Substring(i+1,ii-i-1);
			}
			else
			{	return "";	// neni cislo verze v ref.
			}
			s+="-S"+(int.Parse(version)).ToString("D3");
			
			// nazel jazyka
			s+="-"+jazyk;
			
			// koncovka
			s+=Path.GetExtension(RefSoubor);
			
			
			return s;
				
		}
		
	}
}
