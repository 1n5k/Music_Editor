using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;

public class DiffChoice : MonoBehaviour {

    string s = "";
    public InputField input;
    public static int diff;

    
	// Use this for initialization
	public void Save () {
        s = input.text;
        switch (s)
        {
            case "EASY":
            case "easy":
                diff = 0;
                break;

            case "NORMAL":
            case "normal":
                diff = 1;
                break;

            case "HARD":
            case "hard":
                diff = 2;
                break;

            case "EXTRA":
            case "extra":
                diff = 3;
                break;
            default:
                Debug.Log("muri");
                break;
        }
        Debug.Log(diff);
    }

    public int getDiffrence()
    {
        return diff;
    }

    // Update is called once per frame
    void Update () {
	
	}
}
