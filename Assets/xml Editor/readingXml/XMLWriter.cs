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

/*public class XMLLoader : MonoBehaviour {
    MusicData loadxml;
    void Start(){
        loadxml = GetComponent<MusicData>();
        Debug.Log(loadxml);
    }
}*/
public class XMLWriter : MonoBehaviour {
/*    string EditFile;
    public Text XMLData;
    public Text OpenFile;
    // Use this for initialization
    public void OpenXml()
    {
        EditFile = OpenFile.text;
        FileStream fs = null;
        XmlReader xmlReader = null;
        XmlReaderSettings settings = null;

        try
        {
            XMLData.text += EditFile + "を読み込みます\r\n";
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
                if (xmlReader.HasValue == true)
                {
                    Type valueType = xmlReader.ValueType;
                    Debug.Log("ValueType: " + valueType.ToString() + "\r\n");
                    Debug.Log("Value: " + xmlReader.Value + "\r\n");
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
            XMLData.text += "Error: " + exc.Message;
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
        }
    }*/
}
