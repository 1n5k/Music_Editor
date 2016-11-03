using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScrollController : MonoBehaviour
{
    //private int a;
    //private int b = 5;
    private int f;
    [SerializeField]
    RectTransform prefab = null;

    void Start()
    {
        for (int i = 1; i < 200; i++)
        {
            var item = Instantiate(prefab) as RectTransform;
            item.SetParent(transform, false);

            var text = item.GetComponentInChildren<Text>();
            text.text = i.ToString();
        }
    }

 /*   void Update()
    {
        a = Keyscroll.getmaxcc() + 5;

        if (b < a)
        {
            b++;
            var item = Instantiate(prefab) as RectTransform;
            item.SetParent(transform, false);

            var text = item.GetComponentInChildren<Text>();
            text.text = a.ToString();

        }
    }*/
}