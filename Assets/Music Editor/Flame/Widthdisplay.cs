﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.IO;

public class Widthdisplay : MonoBehaviour {
    private int Width;
    private int change;
    private GameObject width;

	void Start () {
        width = GameObject.Find("Widthdisplay");

    }

    void Update () {
        Width = Notescreate.getmag();
        width.GetComponent<Text>().text = "ノーツの幅:Q↓" + Width +"↑W";
    }
}
