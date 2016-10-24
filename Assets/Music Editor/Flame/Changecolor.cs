using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Changecolor : MonoBehaviour {
    private int bun;
    private int[] risum = { 4, 8, 12, 16, 24, 32, 48 };
    private GameObject Line;
    private GameObject Text;
    private GameObject Background;
    

    void Start () {
        Line = GameObject.Find("MusicLine");
        Text = GameObject.Find("fraction");
        Background = GameObject.Find("Image");
        transform.SetAsLastSibling();
    }

    void Update () {
        bun = Keyscroll.geti();

        if (bun == 0)
        {
            Line.GetComponent<Image>().color = new Color(1, 0, 0, 0.5f);
            Background.GetComponent<Image>().color = new Color(1, 0, 0, 1);
            Text.GetComponent<Text>().text = risum[bun].ToString();
        }
        if (bun == 1)
        {
            Line.GetComponent<Image>().color = new Color(0, 1, 0, 0.5f);
            Background.GetComponent<Image>().color = new Color(0, 1, 0, 1);
            Text.GetComponent<Text>().text = risum[bun].ToString();
        }
        if (bun == 2)
        {
            Line.GetComponent<Image>().color = new Color(1, 1, 0, 0.5f);
            Background.GetComponent<Image>().color = new Color(1, 1, 0, 1);
            Text.GetComponent<Text>().text = risum[bun].ToString();
        }
        if (bun == 3)
        {
            Line.GetComponent<Image>().color = new Color(0, 0, 1, 0.5f);
            Background.GetComponent<Image>().color = new Color(0, 0, 1, 1);
            Text.GetComponent<Text>().text = risum[bun].ToString();
        }
        if (bun == 4)
        {
            Line.GetComponent<Image>().color = new Color(1, 0, 1, 0.5f);
            Background.GetComponent<Image>().color = new Color(1, 0, 1, 1);
            Text.GetComponent<Text>().text = risum[bun].ToString();
        }
        if (bun == 5)
        {
            Line.GetComponent<Image>().color = new Color(0, 1, 1, 0.5f);
            Background.GetComponent<Image>().color = new Color(0, 1, 1, 1);
            Text.GetComponent<Text>().text = risum[bun].ToString();
        }
        if (bun == 6)
        {
            Line.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            Background.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            Text.GetComponent<Text>().text = risum[bun].ToString();
        }
    }
}
