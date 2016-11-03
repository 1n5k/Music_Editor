using UnityEngine;
using System.Collections;
using System.Xml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine.UI;

public class MusicData
{
    public string Title;
    public string Subtitle;
    public string Artist;
    public double Bpm;
    public string MusicAddress;
    public string Jackect;
    public string[] Difficulty = new string[4];
    public string MovieAddress;
    public double Offset;
    public double SelectOffset;
    public double Selectlong;
    public string[] Notes = new string[4];
};

public class XMLLoader : MonoBehaviour
{

    string EditFile;

    public Text XMLData ;
    //public Text OpenFile;

    static Boolean DifficultyChecker = false;
    static Boolean NotesChecker = false;
    public static MusicData LoadedData = new MusicData();
    // Use this for initialization
    public InputField addr;

            
    public void OpenXml()
    {

        EditFile = @addr.text;
        Debug.Log("inputfield addr is "+EditFile);
        EditFile = @"D:\Music_Editor\Score\NightOfKnights.xml";//OpenFile.text;

        FileStream fs = null;
        XmlReader xmlReader = null;
        XmlReaderSettings settings = null;
        string Listener = "";
        
        try
        {
            Debug.Log(EditFile + "を読み込みます\r\n");
            fs = new FileStream(EditFile, FileMode.Open);
            Debug.Log("a");
            settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            settings.IgnoreWhitespace = true;

            xmlReader = XmlReader.Create(fs, settings);
            Debug.Log("b");
            Debug.Break();
            while (xmlReader.Read() == true)
            {
                XmlNodeType nType = xmlReader.NodeType;
                //Debug.Log("NodeType: " + nType.ToString() + "\r\n");
                // Debug.Log("LocalName: " + xmlReader.LocalName + "\r\n");
                //Debug.Log("Depth: " + Convert.ToString(xmlReader.Depth) + "\r\n");
                //Debug.Log("Name: " + xmlReader.Name + "\r\n");
               // Debug.Log("c");
                if (xmlReader.Name != "" && xmlReader != null && nType == XmlNodeType.Element)
                {
                    Listener = xmlReader.Name;
                    if (Listener == "DIFFICULTY")
                    {
                        DifficultyChecker = true;
                    }
                    else if (Listener == "NOTES")
                    {
                        NotesChecker = true;
                    }
                }
                else if (xmlReader.Name == "DIFFICULTY" && nType == XmlNodeType.EndElement)
                {
                    DifficultyChecker = false;
                }
                else if (xmlReader.Name == "NOTES" && nType == XmlNodeType.EndElement)
                {
                    NotesChecker = false;
                }

                if (xmlReader.HasValue == true)
                {
                    Type valueType = xmlReader.ValueType;
                    CheckNodeData(Listener, xmlReader.Value);
                    
                }

               
            }



            if (LoadedData.Notes[DiffChoice.diff] != null)
            { 
                NotesPadding(ref LoadedData.Notes[DiffChoice.diff]);
            }
            else
            {
                Debug.Log("春風亭昇太");
            }
            
        }
        catch (Exception exc)
        {
            Debug.Log(@"Error: "+exc.Message);
        }
        finally
        {
            if (fs != null)
            {
                fs.Close();
            }
            if (xmlReader != null)
            {
                xmlReader.Close();
            }
            //SendToGlobalValue(LoadedData);
        }
    }

    public double getBPM()
    {
        return LoadedData.Bpm;
    }
    public void Update()
    {
        
    }

    public void NotesPadding(ref string score)
    {
        string pad = @"
    |0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|";
        
        int i = 0;int m = 0;
        char[] spc = { ' ' };
        char[] rmsymbl = { ',', '|',';','@'};
        string send = "";
        string[] divscore = { };
        var linecnt = score.ToList().Where(c => c.Equals('\n')).Count() ;//kopipe
        Debug.Log(linecnt);
        StreamWriter log1 = new StreamWriter(@"D:\Music_Editor\log1.txt", true);
        int b = 0;
        divscore = score.Split('\n');
        for (int c = 0; c < linecnt; c++) { 
            foreach (char s in spc)
       
                {
                    divscore[c] = divscore[c].Replace(s.ToString(), "");
                    b++;
                }
        }

        for (; i < linecnt; i++)
        {
            //Debug.Log("divscore["+i+"] is " + divscore[i]);
            if (0 < divscore[i].IndexOf('@'))
            {
                Debug.Log("find @");
                int n = i + 1;
                int intrvl = n - m;
                switch (intrvl)
                {
                    case 4:
                        for (; m < n; m++)
                        {
                            if (m != n - 1)
                            {
                                for (int k = 0; k < 11; k++)
                                {
                                    int len = divscore[m].Length;
                                    divscore[m] = divscore[m].Insert(len - 1, pad);

                                }
                            }
                            else
                            {
                                for (int k = 0; k < 11; k++)
                                {
                                    int len = divscore[m].Length;
                                    divscore[m] = divscore[m].Insert(len - 1, pad);
                                }
                            }
                        }
                        break;
                    case 8:
                        for (; m < n; m++)
                        {
                            if (m != n - 1)
                            {
                                for (int k = 0; k < 5; k++)
                                {
                                    int len = divscore[m].Length;
                                    divscore[m] = divscore[m].Insert(len - 1, pad);

                                }
                            }
                            else
                            {
                                for (int k = 0; k < 5; k++)
                                {
                                    int len = divscore[m].Length;
                                    divscore[m] = divscore[m].Insert(len - 1, pad);
                                }
                            }
                        }
                        break;
                    case 12:
                        for (; m < n; m++)
                        {
                            if (m != n - 1)
                            {
                                for (int k = 0; k < 3; k++)
                                {
                                    int len = divscore[m].Length;
                                    divscore[m] = divscore[m].Insert(len - 1, pad);

                                }
                            }
                            else
                            {
                                for (int k = 0; k < 3; k++)
                                {
                                    int len = divscore[m].Length;
                                    divscore[m] = divscore[m].Insert(len - 1, pad);
                                }
                            }
                        }
                        break;
                    case 16:
                        for (; m<n; m++)
                        {
                            if (m != n - 1)
                            {
                                for (int k = 0; k < 2; k++)
                                {
                                    int len = divscore[m].Length;
                                    divscore[m] = divscore[m].Insert(len-1, pad);
                                }
                            }
                            else
                            {
                                for (int k = 0; k < 2; k++)
                                {
                                    int len = divscore[m].Length;
                                    divscore[m] = divscore[m].Insert(len-1, pad);
                                }
                            }
                            
                        }
                        break;
                    case 24:
                        for (; m < n; m++)
                        {
                            if (m != n - 1)
                            {
                                for (int k = 0; k < 1; k++)
                                {
                                    int len = divscore[m].Length;
                                    divscore[m] = divscore[m].Insert(len - 1, pad);

                                }
                            }
                            else
                            {
                                for (int k = 0; k < 1; k++)
                                {
                                    int len = divscore[m].Length;
                                    divscore[m] = divscore[m].Insert(len - 1, pad);
                                }
                            }
                        }
                        break;
                }
                m = i + 1;
                
            }
            
        }
        StringBuilder padednotes = new StringBuilder( string.Join("\n", divscore) );
        send = padednotes.ToString();
        
        Debug.Log(send);
        this.GetComponent<GlobalValue>().AnalyzingNotes(send);
        Debug.Log("Analyzed");
        foreach (char c in rmsymbl)
         {
             padednotes = padednotes.Replace(c.ToString(), "");

         }
        log1.WriteLine(padednotes);
        /*foreach (string s in padednotes)
        {
            log.WriteLine(s);
        }*/

        
        log1.Flush();
        log1.Close();

    }

    //タグを発見したときに判別を行う
    public void CheckNodeData(string checkdata, string datavalue)
    {
        if (checkdata == "TITLE")
        {
            LoadedData.Title = datavalue;
        }
        else if (checkdata == "SUBTITLE")
        {
            LoadedData.Subtitle = datavalue;
        }
        else if (checkdata == "ARTIST")
        {
            LoadedData.Artist = datavalue;
        }
        else if (checkdata == "BPM")
        {
            LoadedData.Bpm = Convert.ToDouble(datavalue);
        }
        else if (checkdata == "JACKET")
        {
            LoadedData.Jackect = datavalue;
        }
        else if (checkdata == "MUSIC")
        {
            LoadedData.MusicAddress = datavalue;
        }
        else if (DifficultyChecker == true)
        {
            if (checkdata == "EASY")
            {
                LoadedData.Difficulty[0] = Convert.ToString(datavalue);
            }
            else if (checkdata == "NORMAL")
            {
                LoadedData.Difficulty[1] = Convert.ToString(datavalue);
            }
            else if (checkdata == "HARD")
            {
                LoadedData.Difficulty[2] = Convert.ToString(datavalue);
            }
            else if (checkdata == "EXTRA")
            {
                LoadedData.Difficulty[3] = Convert.ToString(datavalue);
            }
        }
        else if (checkdata == "MOVIE")
        {
            LoadedData.MovieAddress = datavalue;
        }
        else if (checkdata == "OFFSET")
        {
            LoadedData.Offset = Convert.ToDouble(datavalue);
        }
        else if (checkdata == "SELECTOFFSET")
        {
            LoadedData.SelectOffset = Convert.ToDouble(datavalue);
        }
        else if (checkdata == "SELECTLONG")
        {
            LoadedData.Selectlong = Convert.ToDouble(datavalue);
        }
        else if (NotesChecker == true)
        {
            if (checkdata == "EASY")
            {
                LoadedData.Notes[0] = datavalue;
            }
            else if (checkdata == "NORMAL")
            {
                LoadedData.Notes[1] = datavalue;
            }
            else if (checkdata == "HARD")
            {
                LoadedData.Notes[2] = datavalue;
            }
            else if (checkdata == "EXTRA")
            {
                LoadedData.Notes[3] = datavalue;
            }
        }
    }
   public void SendToGlobalValue(MusicData Sender)
    {


        /* MusicData buff = GameObject.Find("GlobalValueControl").GetComponent<GlobalValue>().MusicParam;
        buff.Title = LoadedData.Title;
        buff.Subtitle = LoadedData.Subtitle;
        buff.Artist = LoadedData.Artist;
        buff.Bpm = LoadedData.Bpm;
        buff.Jackect = LoadedData.Jackect;
        buff.MusicAddress = LoadedData.MusicAddress;
        buff.MovieAddress = LoadedData.MovieAddress;
        buff.Difficulty = LoadedData.Difficulty;
        buff.Offset = LoadedData.Offset;
        buff.SelectOffset = LoadedData.SelectOffset;
        buff.Selectlong = LoadedData.Selectlong;
        buff.Notes = LoadedData.Notes;*/
    }
}
