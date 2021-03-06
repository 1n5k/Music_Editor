﻿using UnityEngine;
using System.Collections;
using System.Xml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine.UI;
using System.Runtime.InteropServices;

/*public class XMLLoader : MonoBehaviour {
    MusicData loadxml;
    void Start(){
        loadxml = GetComponent<MusicData>();
        Debug.Log(loadxml);
    }
}*/
public class XMLWrite{
    String scoresname;
    public Boolean Write(ref string score)
    {
        if (XMLLoader.LoadedData.Title != null)
        {

            scoresname = XMLLoader.LoadedData.Title + ".xml";
        }
        else
        {
            scoresname = "score.xml";
        }
        string[] tag =  {
            "<MusicData>",      "\n", 
            "  <TITLE>",          "</TITLE>\n",
            "  <SUBTITLE>",       "</SUBTITLE>\n",
            "  <ARTIST>",         "</ARTIST>\n",
            "  <BPM>",            "</BPM>\n",
            "  <JACKET>",         "</JACKET>\n",
            "  <MUSIC>",          "</MUSIC>\n",
            "  <DIFFCULTY>",      "\n", 
            "    <EASY>",         "</EASY>\n",
            "    <NORMAL>",       "</NORMAL>\n",
            "    <HARD>",         "</HARD>\n",
            "    <EXTRA>",       "</EXTRA>\n",
            "  </DIFFCULTY>",     "\n",
            "  <MOVIE>",          "</MOVIE>\n",
            "  <OFFSET>",         "</OFFSET>\n",
            "  <SELECTOFFSET>",   "</SELECTOFFSET>\n",
            "  <SELECTLONG>",     "</SELECTLONG>\n",
            "  <NOTES>",          "\n",
            "    <EASY>",         "    </EASY>\n",
            "    <NORMAL>",       "    </NORMAL>\n",
            "    <HARD>",         "    </HARD>\n",
            "    <EXTRA>",       "    </EXTRA>\n",
            "  </NOTES>",         "\n",
            "</MusicData>",     "\n",
        };
        MusicData savedata = new MusicData();
        savedata = XMLLoader.LoadedData;
        savedata.Notes[DiffChoice.diff] = score;
        try
        {
            
            Debug.Log(Directory.GetCurrentDirectory()); //デバッグ用

            Directory.CreateDirectory("Score");
            String path = Directory.GetCurrentDirectory()+ @"\Score\" + scoresname; //保存するパスを取得
            Debug.Log(path);
            XmlWriter writer = XmlWriter.Create(path);

            for (int i = 0; i < 48; i++)
            {
                switch(i) {
                    case 0:  
                    case 1:
                    case 3:
                    case 5:
                    case 7:
                    case 9:
                    case 11:
                    case 13:
                    case 14:
                    case 15:
                    case 17:
                    case 19:
                    case 21:
                    case 23:
                    case 24:
                    case 25:
                    case 27:
                    case 29:
                    case 31:
                    case 33:
                    case 34:
                    case 35:
                    case 37:
                    case 39:
                    case 41:
                    case 43:
                    case 44:
                    case 45:
                    case 46:
                    case 47:
                        writer.WriteRaw(tag[i]);
                        break;
                    case 2:
                        writer.WriteRaw(tag[i]);
                        writer.WriteRaw(savedata.Title);
                        break;
                    case 4:
                        writer.WriteRaw(tag[i]);
                        writer.WriteRaw(savedata.Subtitle);
                        break;
                    case 6:
                        writer.WriteRaw(tag[i]);
                        writer.WriteRaw(savedata.Artist);
                        break;
                    case 8:
                        writer.WriteRaw(tag[i]);
                        writer.WriteRaw(savedata.Bpm.ToString());
                        break;
                    case 10:
                        writer.WriteRaw(tag[i]);
                        writer.WriteRaw(savedata.Jackect);
                        break;
                    case 12:
                        writer.WriteRaw(tag[i]);
                        writer.WriteRaw(savedata.MusicAddress);
                        break;
                    case 16:
                        writer.WriteRaw(tag[i]);
                        writer.WriteRaw(savedata.Difficulty[0]);
                        break;
                    case 18:
                        writer.WriteRaw(tag[i]);
                        writer.WriteRaw(savedata.Difficulty[1]);
                        break;
                    case 20:
                        writer.WriteRaw(tag[i]);
                        writer.WriteRaw(savedata.Difficulty[2]);
                        break;
                    case 22:
                        writer.WriteRaw(tag[i]);
                        writer.WriteRaw(savedata.Difficulty[3]);
                        break;
                    case 26:
                        writer.WriteRaw(tag[i]);
                        writer.WriteRaw(savedata.MovieAddress);
                        break;
                    case 28:
                        writer.WriteRaw(tag[i]);
                        writer.WriteRaw(savedata.Offset.ToString());
                        break;
                    case 30:
                        writer.WriteRaw(tag[i]);
                        writer.WriteRaw(savedata.SelectOffset.ToString());
                        break;
                    case 32:
                        writer.WriteRaw(tag[i]);
                        writer.WriteRaw(savedata.Selectlong.ToString());
                        break;
                    case 36:
                        writer.WriteRaw(tag[i]);
                        writer.WriteRaw(savedata.Notes[0]);
                        break;
                    case 38:
                        writer.WriteRaw(tag[i]);
                        writer.WriteRaw(savedata.Notes[1]);
                        break;
                    case 40:
                        writer.WriteRaw(tag[i]);
                        writer.WriteRaw(savedata.Notes[2]);
                        break;
                    case 42:
                        writer.WriteRaw(tag[i]);
                        writer.WriteRaw(savedata.Notes[3]);
                        break;
                    
                }

            }
            writer.Flush();
            writer.Close();
            Debug.Log("Save success!");
            return true;
        }
        catch(Exception exc)
        {
            Debug.Log("Save Error");
            return false;
        }
       // return true;
    }
    
   
}
