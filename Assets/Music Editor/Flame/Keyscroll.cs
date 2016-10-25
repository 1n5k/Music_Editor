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

public class Keyscroll : MonoBehaviour {
    public  static int i = 0;      //分数の管理
    public static float count = 0;  //1小節間の進み
    public static int cc = 1; //小節数のカウント
    public static int[] risum = { 4, 8, 12, 16, 24, 32, 48 };//音符の種類
    public static float Ysum = 0;//移動の合計値
    private float c = 0;
    private float fullC = 0;
    private float Ymove;
    private Vector3 v;
    private int one = 0;
    public static int maxcc = 1;

    // Use this for initialization
    void Start () {
        //マウスのロック
       //Cursor.lockState = CursorLockMode.Locked;

        Ymove = -0.1f * 480;//1小節の大きさ
    }

    // Update is called once per frame
    void Update() {
        v = transform.position;

        //risum[i]の逆数が*risum[i]に到達したか判断
        for (int a = 0; a < risum[i]; a++)
        {
            c = risum[i];
            fullC -= 1 / c;
        }

        if (count == fullC)
        {
            count = 0;
            cc--;
            Ysum = 0;
        }

        fullC = 0;



        //刻みを管理
        if (Input.GetKeyDown(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightShift) != true && Input.GetKey(KeyCode.LeftShift) != true)
        {

            //小節線以外で押したとき前の小節線に戻す
            if(count < 0)
            {
                v.y -= Ymove * (1 + count);
                Ysum -= Ymove * (1 + count);
                transform.position = v;

                Ysum = 0;
                count = 0;
                cc--;
                i--;
            }
            else if (count >= 0)
            {
                v.y -= Ymove * count;
                Ysum -= Ymove * count;
                transform.position = v;
                count = 0;
                Ysum = 0;

                i--;
            }

            //値を制限
            if (i < 0)
            {
                i = 0;
            }

        }

        //risum[i]の逆数が*risum[i]に到達したか判断
        for (int a = 0; a < risum[i]; a++)
        {
            c = risum[i];
            fullC += 1 / c;
        }

        if (count == fullC)
        {
            count = 0;
            cc++;
            Ysum = 0;
            if (maxcc < cc) { maxcc++;  }

        }
        fullC = 0;

        //刻みを管理
        if (Input.GetKeyDown(KeyCode.RightArrow) && Input.GetKey(KeyCode.RightShift) != true && Input.GetKey(KeyCode.LeftShift) != true)
        {

            //小節線以外で押したとき前の小節線に戻す
            if (count < 0)
            {
                v.y -= Ymove * (1 + count);
                Ysum -= Ymove * (1 + count);
                transform.position = v;

                Ysum = 0;
                count = 0;
                cc--;
                i++;
            }
            else if(count >= 0)
            {
                v.y -= Ymove * count;
                Ysum -= Ymove * count;
                transform.position = v;
                count = 0;
                Ysum = 0;

                i++;
            }


            //値を制限
            if (i > 6)
            {
                i = 6;
            }

        }

        if (Mathf.Sign(count) == -1 && one == 0)
        {
            cc--;
            one = 1;
        }
        if (Mathf.Sign(count) == 1 && one == 1)
        {
            cc++;
            one = 0;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            c = risum[i];
            count += 1/c;
            v.y += Ymove / risum[i];
            Ysum += Ymove / risum[i];
            transform.position = v;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            c = risum[i];
            count -= 1/c;
            v.y -= Ymove / risum[i];
            if(Ysum == 0)
            {
                Ysum += Ymove - Ymove / risum[i];
            }
            else
            {
                Ysum -= Ymove / risum[i];
            }
            transform.position = v;
        }

        if (count == 0 && cc == 0)
        {
            cc = 1;
        }

        if (count < 0 && cc < 1)
        {
            count = 0;
        }
        Debug.Log("cc"+cc);

        if ( cc == 0 || Ysum <= -48)
        {
            Ysum = 0;
        }

	}
    public static int geti()
    {
        return i;
    }
    public static float getYsum()
    {
        return Ysum;
    }
    public static int getcc()
    {
        return cc;
    }
    public static int getmaxcc()
    {
        return maxcc;
    }
}
