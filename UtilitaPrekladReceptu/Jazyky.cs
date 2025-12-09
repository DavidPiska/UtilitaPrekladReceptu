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
  {
    private static Jazyky instance = new Jazyky();

    public static Jazyky Instance
    {
      get { return instance; }
    }

    public Jazyky()
    {
      Init();
    }

    public static int NewWords = 0; // nove slova
    private Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();

    /// <summary>
    /// Normalizes dictionary keys and values (e.g. trims trailing spaces).
    /// </summary>
    private static string NormalizeKey(string value)
    {
      if (string.IsNullOrWhiteSpace(value))
        return string.Empty;

      // Replace non-breaking spaces and trim regular spaces
      return value
        .Replace('\u00A0', ' ')   // non-breaking space to normal space
        .Trim();                  // remove leading and trailing spaces
    }

    /// <summary>
    /// Loads the dictionary from file.
    /// Minimal change: register BOTH column 0 (Czech) and column 1 (English) as lookup keys,
    /// so the tool can translate when the source text is Czech OR English.
    /// </summary>
    public void Init()
    {
      int i = 0;
      if (File.Exists(MainForm.Cesta + "ProgramsDictionary.txt"))
      {
        using (TextReader rdr = new StreamReader(MainForm.Cesta + "ProgramsDictionary.txt"))
        {
          string line;
          while ((line = rdr.ReadLine()) != null)
          {
            if (line.Contains("Pečivo, Dezerty"))
            {
            }

          znova2:
            i = line.IndexOf("\""); // remove quotes that Excel might add
            if (i >= 0)
            {
              line = line.Remove(i, 1);
              goto znova2;
            }

            string[] fields = line.Split('\t');

            // Normalize all fields so that trailing spaces do not break lookups
            for (int j = 0; j < fields.Length; j++)
            {
              fields[j] = NormalizeKey(fields[j]);
            }

            if (fields.Length == 0)
              continue;

            // --- minimal changes start ---
            // Always register column 0 (Czech) as a key
            string keyCs = fields[0];
            if (!string.IsNullOrEmpty(keyCs) && !dictionary.ContainsKey(keyCs))
            {
              dictionary.Add(keyCs, fields);
            }
            else
            {
              //MyDebug.Add("!stejny slovnik: "+fields[0]);
            }

            // Additionally register column 1 (English) as an alternate key if present and different.
            if (fields.Length > 1)
            {
              string keyEn = fields[1];
              if (!string.IsNullOrEmpty(keyEn) && keyEn != keyCs && !dictionary.ContainsKey(keyEn))
              {
                // Map the English source phrase to the SAME translation row.
                dictionary.Add(keyEn, fields);
              }
            }
            // --- minimal changes end ---
          }
        }
      }
      else
      {
        MessageBox.Show("File ProgramsDictionary.txt was not found in main directory.");
        //MainForm.Odkaz.Dispose();	// ukonci se aplikace
      }
    }

    /// <summary>
    /// Looks up the proper language in the dictionary and returns the text.
    /// </summary>
    /// <param name="s"></param>
    /// <returns> string </returns>
    public string VratText(string s)
    {
      if (s.Contains("Jablko"))
      {
        int i3 = 0;
      }
      MainForm.infoCounter++; // pocitadlo pruchodu

    znova:
      int i = s.IndexOf(Environment.NewLine); // split by lines
      if (i > 0)
      {
        string s2 = s.Substring(0, i);
        VratText(s2);
        s = s.Remove(0, i + 2);
        goto znova;
      }

    znova2:
      i = s.IndexOf('"'); // remove quotes that Excel might add
      if (i > 0)
      {
        s = s.Remove(i, 1);
        goto znova2;
      }

      // Normalize after removing newlines and quotes to avoid issues with trailing spaces
      string original = s;
      s = NormalizeKey(s);

      if (s == "")
        return "";

      if (dictionary.ContainsKey(s))  // key exists (now for CZ OR EN source)
      {
        string[] ss = dictionary[s];
        if (ss.Length <= MainForm.Jazyk)  // target language column not present
        {
          MainForm.Log += "Language " + MainForm.Jazyk.ToString() + " does not exist (" + s + ")" + Environment.NewLine;
          if (ss.Length > 1) { return ss[1]; }    // fallback to English if present
          return s;         // fallback to source (Czech)
        }
        if (ss[MainForm.Jazyk] == "")
        {
          if (s == "") return "";
          string[] ssL = dictionary["cs"];
          string[] ssL2 = dictionary["Czech"];
          MainForm.Log += "Missing /" + ss[0] + "/ in language " + ssL[MainForm.Jazyk].ToString() + " " + ssL2[MainForm.Jazyk].ToString() + Environment.NewLine;
          if ((ss.Length > 1) && (ss[1] != "")) // English present and non-empty
          {
            return ss[1];       // return English
          }
          return ss[0]; // otherwise return Czech
        }
        return ss[MainForm.Jazyk];
      }
      else  // key does not exist in current in-memory dictionary
      {
        bool exists = false;
        // We check the raw dictionary file because it also contains
        // new words added during the current run.
        using (TextReader rdr = new StreamReader(MainForm.Cesta + "ProgramsDictionary.txt"))
        {
          string line;
          while ((line = rdr.ReadLine()) != null)
          {
            string[] fields = line.Split('\t');
            if (fields.Length == 0)
              continue;

            // Normalize columns before comparing (handles trailing spaces)
            string col0 = NormalizeKey(fields[0]);          // Czech
            string col1 = fields.Length > 1
              ? NormalizeKey(fields[1])                   // English
              : string.Empty;

            // 1) Original behavior: check first column (Czech reference)
            if (s == col0)
            {
              exists = true;
              break;
            }

            // 2) NEW behavior (kept): also check second column (English reference)
            if (s == col1)
            {
              exists = true;
              break;
            }
          }
        }

        // If the string already exists either as Czech (col 0) or English (col 1),
        // we do NOT add a duplicate row into the dictionary file.
        if (exists) return s;

        MainForm.Log += "Dictionary missing - " + original + Environment.NewLine;

        NewWords++;
        // Save the normalized key to avoid trailing spaces in future
        File.AppendAllText(MainForm.Cesta + "ProgramsDictionary.txt", Environment.NewLine + s, Encoding.Unicode); // add new string to the dictionary file
        if (s == "1")
        {
        }
      }
      return s;
    } //*/

    /// <summary>
    /// Returns the whole dictionary dump.
    /// </summary>
    /// <returns> string </returns>
    public string ReturnAllDictionary()
    {
      string s = "";
      List<string> list = new List<string>(dictionary.Keys);
      foreach (string k in list)
      {
        s += k.PadRight(20) + "  " + string.Join("; ", dictionary[k]);
        s += Environment.NewLine;
      }
      return s;
    }

    public string[] ReturnAllLanguages()
    {
      string[] s = dictionary["cs"];
      return s;
    }

    /// <summary>
    /// Selects and sets the current language index.
    /// </summary>
    /// <param name="SelectedLanguage"></param>
    /// <returns> bool if success</returns>
    public bool SelectLanguage(string SelectedLanguage)
    {
      string[] s = dictionary["cs"];
      for (int x = 0; x < s.Length; x++)
      {
        if (s[x] == SelectedLanguage) // must match the dictionary header row
        {
          //MainForm.Jazyk=x;
          MainForm.Jazyk = x; // persist selected language index
          return true;  // found
        }
      }
      return false; // not found
    }

    public void ConvertFileToDifferentLanguage(string cesta, int jazyk)
    {
      string[] ds = new string[dictionary.Count]; // list of reference phrases (now CZ + EN)
      dictionary.Keys.CopyTo(ds, 0);

      /*for(int x=0;x>dictionary.Count;x++)
			{	string[] sx=dictionary.Take(x);
				ds[x]=sx[0];
			}*/

      string s = File.ReadAllText(cesta);

      foreach (string o1 in ds)
      {
        if (o1 != "")
        {
          s = s.Replace('"' + o1 + '"', '"' + VratText(o1) + '"');  // quotes variant
          s = s.Replace('>' + o1 + '<', '>' + VratText(o1) + '<');  // angle brackets variant
          s = s.Replace('>' + o1 + "\r\n", '>' + VratText(o1) + "\r\n");  // with newline
          s = s.Replace("\r\n" + o1 + '<', "\r\n" + VratText(o1) + '<');  // with newline
          s = s.Replace("\r\n" + o1 + "\r\n", "\r\n" + VratText(o1) + "\r\n");  // between newlines
        }
      }

      string[] sl = ReturnAllLanguages();

      string sdir = Path.GetDirectoryName(cesta);
      string sname = Path.GetFileName(cesta);
      sname = sl[jazyk] + " " + sname;
      string cesta2 = Path.Combine(sdir, sname);

      File.WriteAllText(cesta2, s);

    }

    public string ConvertFileToDifferentLanguage(string cesta, int jazyk, string sdirDest)
    {
      string[] ds = new string[dictionary.Count]; // list of reference phrases (now CZ + EN)
      dictionary.Keys.CopyTo(ds, 0);

      string s = File.ReadAllText(cesta);

      foreach (string o1 in ds)
      {
        if (o1 != "")
        {
          s = s.Replace('"' + o1 + '"', '"' + VratText(o1) + '"');  // quotes variant
          s = s.Replace('>' + o1 + '<', '>' + VratText(o1) + '<');  // angle brackets variant
          s = s.Replace('>' + o1 + "\r\n", '>' + VratText(o1) + "\r\n");  // with newline
          s = s.Replace("\r\n" + o1 + '<', "\r\n" + VratText(o1) + '<');  // with newline
          s = s.Replace("\r\n" + o1 + "\r\n", "\r\n" + VratText(o1) + "\r\n");  // between newlines
        }
      }

      string[] sl = ReturnAllLanguages();
      string sname = Path.GetFileName(cesta);
      sname = sl[jazyk] + " " + sname;

      File.WriteAllText(sdirDest + "/" + sname, s);

      return s; // returns the translated file content (for later ISO use)
    }
  }
}
