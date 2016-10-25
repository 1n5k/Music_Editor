using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Widthdisplay : MonoBehaviour {
    private int Width;
    private int change;
    private int bunsu;
    private int [] risum = { 4, 8, 12, 16, 24, 32, 48 };
    private GameObject width;
    private GameObject stoptime;

	void Start () {
        width = GameObject.Find("Widthdisplay");
        stoptime = GameObject.Find("StopSpeed");
    }

    void Update () {
        Width = Notescreate.getmag();
        bunsu = Keyscroll.geti();
        width.GetComponent<Text>().text = "ノーツの幅:Q↓" + Width +"↑W";
        stoptime.GetComponent<Text>().text = " 停止時間:" + risum[bunsu] + "分 ×" + " E↓" + 0 + "拍 ↑R ";
    }
}
