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

public class XMLLoader : MonoBehaviour
{
    
    MusicData loadxml;
    string musicxml = @"D:\Music_Editor\Assets\Xml Editor\NightOfKnights.xml";
    XmlDocument doc;
    void Start()
    {
        
    }
    void Update()
    {
        doc = new XmlDocument();
        loadxml = GetComponent<MusicData>();
        doc.Load(musicxml);
        loadxml.Title = doc.GetElementsByTagName("TITLE")[0].InnerText;
        Debug.Log(loadxml.Title);
        Debug.Log("殺すぞ");
    }
    string GetTitle()
    {
        if(loadxml.Title != null)
        {
            return loadxml.Title;
        }
        else
        {
            return @"Title Not Found";
        }
    }
}