using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
