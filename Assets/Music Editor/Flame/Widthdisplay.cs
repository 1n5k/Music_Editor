using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Widthdisplay : MonoBehaviour {
    private int Width;
    private GameObject text;
    private string dis = "Q↓ ";
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
