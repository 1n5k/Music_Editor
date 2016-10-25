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

class MusicData
{
    public string Title;
    public string Subtitle;
    public string Artist;
    public double Bpm;
    public string MusicAddress;
    public string Jackect;
    public int[] Difficulty = new int[4];
    public string MovieAddress;
    public double Offset;
    public double SelectOffset;
    public double Selectlong;
    public string[] Notes = new string[4];
};
public class XMLLoader : MonoBehaviour
{

    string EditFile;

    public Text XMLData;
    //public Text OpenFile;

    static Boolean DifficultyChecker = false;
    static Boolean NotesChecker = false;
    static MusicData LoadedData;
    // Use this for initialization

    
    public void OpenXml()
    {
        EditFile = @"D:\Music_Editor\NightOfKnights.xml";//OpenFile.text;
        FileStream fs = null;
        XmlReader xmlReader = null;
        XmlReaderSettings settings = null;
        string Listener = "";
        LoadedData = new MusicData();
        try
        {
            //XMLData.text += EditFile + "を読み込みます\r\n";
            fs = new FileStream(EditFile, FileMode.Open);

            settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            settings.IgnoreWhitespace = true;

            xmlReader = XmlReader.Create(fs, settings);

            while (xmlReader.Read() == true)
            {
                XmlNodeType nType = xmlReader.NodeType;
                Debug.Log("NodeType: " + nType.ToString() + "\r\n");
                Debug.Log("LocalName: " + xmlReader.LocalName + "\r\n");
                Debug.Log("Depth: " + Convert.ToString(xmlReader.Depth) + "\r\n");
                Debug.Log("Name: " + xmlReader.Name + "\r\n");
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
                    Debug.Log("ValueType: " + valueType.ToString() + "\r\n");
                    Debug.Log("Value: " + xmlReader.Value + "\r\n");
                    CheckNodeData(Listener, xmlReader.Value);
                }

                //属性がある場合
                if (xmlReader.HasAttributes == true)
                {
                    for (int i = 0; i < xmlReader.AttributeCount; i++)
                    {
                        xmlReader.MoveToAttribute(i);
                        Debug.Log("Attribute Name: " + xmlReader.Name + "\r\n");
                        if (xmlReader.HasValue == true)
                        {
                            Type valueType = xmlReader.ValueType;
                            Debug.Log("ValueType: " + valueType.ToString() + "\r\n");
                            Debug.Log("Attribute Value: " + xmlReader.Value + "\r\n");
                        }
                    }
                    xmlReader.MoveToElement();
                }

                Debug.Log("\r\n");
            }
        }
        catch (Exception exc)
        {
            Debug.Log(@"Error: " + exc.Message);
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
            SendToGlobalValue(LoadedData);
        }
    }

    void Start()
    {
        OpenXml();
    }
    //タグを発見したときに判別を行う
    void CheckNodeData(string checkdata, string datavalue)
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
                LoadedData.Difficulty[0] = Convert.ToInt32(datavalue);
            }
            else if (checkdata == "NORMAL")
            {
                LoadedData.Difficulty[1] = Convert.ToInt32(datavalue);
            }
            else if (checkdata == "HARD")
            {
                LoadedData.Difficulty[2] = Convert.ToInt32(datavalue);
            }
            else if (checkdata == "EXTRA")
            {
                LoadedData.Difficulty[3] = Convert.ToInt32(datavalue);
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
    void SendToGlobalValue(MusicData Sender)
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
