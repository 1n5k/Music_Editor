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
using System.Runtime.InteropServices;
using UnityEditor;

/*public class XMLLoader : MonoBehaviour {
    MusicData loadxml;
    void Start(){
        loadxml = GetComponent<MusicData>();
        Debug.Log(loadxml);
    }
}*/
public class XMLWriter{
    public Boolean Write(ref string score)
    {
        String scores = "score.xml";

        try
        {
            
            Debug.Log(Directory.GetCurrentDirectory()); //デバッグ用

            Directory.CreateDirectory("Score");
            String path = Directory.GetCurrentDirectory()+ @"\Score\" + scores; //保存するパスを取得
            Debug.Log(path);
            
            XmlWriter writer = XmlWriter.Create(path);

            writer.WriteRaw("<MusicData>\n");
            writer.WriteRaw(" <NOTES>\n");
            writer.WriteRaw("  " + score);
            writer.WriteRaw(" </NOTES>\n");
            writer.WriteRaw("<MusicData>\n");

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
