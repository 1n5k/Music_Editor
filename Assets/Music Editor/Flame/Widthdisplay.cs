using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.IO;

public class Widthdisplay : MonoBehaviour {
    private int Width;
    private GameObject text;
    private string dis = "Q↓ ";
    MusicData title;
    // Use this for initialization
    void Start () {
        text = GameObject.Find("Widthdisplay");

    }

    void Update () {
        Width = Notescreate.getmag();
        dis = Width.ToString();
        text.GetComponent<Text>().text = "ノーツの幅:Q↓ " + dis +" ↑W";

    }
}
