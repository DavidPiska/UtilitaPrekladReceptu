/*
 * Created by SharpDevelop.
 * User: DavidPiska
 * Date: 27.11.2014
 * Time: 9:36
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.IO;

using System.Text;
using System.Drawing.Imaging;
using System.Media;


namespace UtilitaPrekladReceptu
{
	/// <summary>
	/// Description of PNGparser.
	/// </summary>
	public class PNGparser
	{
		public Image img;
		public PNGparser()
		{
		}
		
		private static int ii=0;
		public Image ReturnImage()
		{	//img=Image.FromFile(MainForm.Cesta+"/egg.png");
			ii++;
			img=returnNumberImage(ii.ToString());
			return img;
		}
		
		/// <summary>
		/// vrati image ze stringu
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public Image ReturnImage(string s)
		{	
			//s=File.ReadAllText("img");
			
			try
			{	byte[] data = Convert.FromBase64String(s);
				using(var stream = new MemoryStream(data, 0, data.Length))
				{ 	Image image = Image.FromStream(stream);
			  		return image;
				}
			}
			catch(Exception ex)
			{	//if(Debug)MyDebug.Add(ex.Message);
			}
			return new Bitmap(10,10);
		}
		
		/// <summary>
		/// vrati string z image 
		/// </summary>
		/// <param name="im"></param>
		/// <returns></returns>
		public string ReturnStringFromImage(Image im)
		{	if(im==null) return null;
			/*MemoryStream ms=new MemoryStream();
			im.Save(ms,System.Drawing.Imaging.ImageFormat.Png);
			byte[] array = ms.ToArray();
			return Convert.ToBase64String(array);*/
			//Metody.Images.SaveImage(im);
			
			byte[] bitmapBytes;
			string bitmapString = null;
			using (MemoryStream memoryStream = new MemoryStream())
			{   im.Save(memoryStream, ImageFormat.Png); 
			    bitmapBytes = memoryStream.GetBuffer();
			    bitmapString = Convert.ToBase64String(bitmapBytes, Base64FormattingOptions.None);
			}
			
			/*Image img = null;
			byte[] bitmapBytes2 = Convert.FromBase64String(bitmapString);
			using (MemoryStream memoryStream = new MemoryStream(bitmapBytes2))
			{   img = Image.FromStream(memoryStream);
			}
			
			File.WriteAllText("img",bitmapString);
			//File.WriteAllText("img",Environment.NewLine);*/
			
			//Metody.Images.SaveImage(im);
			
			return bitmapString;
		}
		
		
		private Image returnNumberImage(string s)
		{	Bitmap bmp = new Bitmap(18,12);
			RectangleF rectf = new RectangleF(0, 0, bmp.Width, bmp.Height);
        	Graphics g = Graphics.FromImage(bmp);
	    	g.DrawString(s, new Font("Tahoma",7), Brushes.Black, rectf);
        	g.Flush();
        	return bmp;
        
		}
		
	}
}
