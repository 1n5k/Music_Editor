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


public class NotesStore{

    public int[,] NOTES = new int[12, 5];
    public int[] OPTION = new int[3];
}

public class Notescreate : MonoBehaviour {

    public RectTransform Notes;                         //単ノーツのコピー元
    public RectTransform LongNotes;                     //ロングノーツのコピー元
    public RectTransform Slide;                         //スライドのコピー元
    public RectTransform Flick;                         //フリックのコピー元
    public RectTransform Parent;                        //親指定
    public GameObject Des;                              //デストロイヤー君
    private int bunsu;                                  //左右の押された数(0～7)の格納先
    private int bunsucount = 0;
    private int cc;                                     //現在の小節数
    private int maxcc;                                  //今までの最大の小節数
    private int max = 0;                                //最大小節が増えた際のトリガー
    private int[] risum = { 4, 8, 12, 16, 24, 32, 48 }; //分数の値
    private int input;                                  //キー入力の値
    private int magsize;                                //レーン横幅の上限を越えないための天井値
    public float mymove;                                //0～-480(1小節の移動幅)
    public static int mag = 1;                          //ノーツの幅
    RectTransform Size;                                    //ノーツのサイズデータ格納先
    private Vector2 v;                                  //サイズデータ格納
    private Vector2 WH;                              //変化後の横幅
    private Vector2 LongWH;
    private float fl = 0;
    //コピー
    RectTransform copy;
    RectTransform longcopy;
    GameObject copyDes;
    //List構造体
    public List<NotesStore> list = new List<NotesStore>();
    public NotesStore capsel = new NotesStore();
    public NotesStore None = new NotesStore(); //初期化用
    public NotesStore LongStock = new NotesStore(); //Londnotesのストック
    //Beat
    public List<int> measure = new List<int>();
    private int Beat = 0;
    private int haku = 0;
    private int hakucount = 0;
    private int BPM = 0;                  //xmlデータから読み込み引っ張ってくる

    string print_array = "";
    private int longtap = -1;
    private int starthaku = 0;
    private int updown = 0;

    void Start()
    {
        //Notesのサイズデータ
        Size = GameObject.Find("Notes").GetComponent<RectTransform>();
        v = Size.sizeDelta;
        WH = v;
        LongWH = v;

        //0小節
        measure.Add(0);
    }


    void Update () {
        mymove = 10 * Keyscroll.getYsum();//バーに追従する移動量
        bunsu = Keyscroll.geti(); //○分の配列管理
        cc = Keyscroll.getcc(); //現在いる小節
        maxcc = Keyscroll.getmaxcc(); //最大小節
        Debug.Log(mymove);

        //新しい小節の長さ決めと初期化
        if(max < maxcc)
        {
            Beat = 0;
            measure.Add(48/*risum[bunsu]*/);
            for(int a = 0;a <= maxcc; a++)
            {
                Beat += measure[a];
            }
            for (int h = measure[max]; h < Beat; h++)
            {
                list.Add(None);
                None = new NotesStore();
            }
            max++;
        }

        int l = 0;
        int c = risum[bunsu];
        for (int a = 0; a < cc; a++)
        {
            l += measure[a];
        }
        fl = l - mymove / 10;   //* c / 480;
        haku = (int)fl; 
        Debug.Log("haku" +haku);

        //拍が変わるたびに前のlistにcapselの値を入れ、capselに変わった後のlistの値を入れる
        if (haku != hakucount)
        {
            list[hakucount] = capsel;
            capsel = new NotesStore();
            capsel = list[haku];
            hakucount = haku;
        }


        //左右キー押されたときのlistに挿入,削除
        /*if (bunsucount < bunsu)
        {
            int move = 0;
            int calcBeat = 0;
            int k = measure[cc];
            int blBeat = k; //前の小節の分数
            measure[cc] = risum[bunsu];//今の小節の分数に変える
            blBeat = measure[cc] - blBeat;//前と今の拍の違い,挿入の長さ
            Debug.Log(blBeat);
            k = k / blBeat;

            for (int a = 0; a < cc; a++)
            {
                calcBeat += measure[a];//挿入前の拍数
            }
            for (int b = 0; b < blBeat; b++)
            {
                move += k;
                int sara = b + calcBeat + move;
                None.NOTES[1, 0] = bunsu;
                list.Insert(sara, None);
                None = new NotesStore();
            }
            Beat += risum[bunsu] - risum[bunsucount];
            bunsucount = bunsu;
        }

        if (bunsucount > bunsu)
        {
            bunsucount = bunsu;
        }
        */

        input = -1;
        magsize = 0;

        //ノーツの幅変更
        if(Input.GetKeyDown(KeyCode.Q))
        {
            mag--;
            if (mag < 1)
            {
                mag = 1;
            }
            WH.x = v.x * mag;
            LongWH.x = v.x * mag;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            mag++;
            if(mag > 12)
            {
                mag = 12;
            }
            WH.x = v.x * mag;
            LongWH.x = v.x * mag;
        }
        //入力を数値化
        if (Input.GetKeyDown(KeyCode.A))//左から1番目
        {
            input = 0;
            magsize = input + mag;
            if (magsize > 12)
            {
                input = -1;
            }

        }
        if (Input.GetKeyDown(KeyCode.Z))//左から2番目
        {
            input = 1;
            magsize = input + mag;
            if(magsize > 12)
            {
                input = -1;
            }
        }
        if (Input.GetKeyDown(KeyCode.S))//左から3番目
        {
            input = 2;
            magsize = input + mag;
            if (magsize > 12)
            {
                input = -1;
            }
        }
        if (Input.GetKeyDown(KeyCode.X))//左から4番目
        {
            input = 3;
            magsize = input + mag;
            if (magsize > 12)
            {
                input = -1;
            }
        }
        if (Input.GetKeyDown(KeyCode.D))//左から5番目
        {
            input = 4;
            magsize = input + mag;
            if (magsize > 12)
            {
                input = -1;
            }
        }
        if (Input.GetKeyDown(KeyCode.C))//左から6番目
        {
            input = 5;
            magsize = input + mag;
            if (magsize > 12)
            {
                input = -1;
            }
        }
        if (Input.GetKeyDown(KeyCode.F))//左から7番目
        {
            input = 6;
            magsize = input + mag;
            if (magsize > 12)
            {
                input = -1;
            }
        }
        if (Input.GetKeyDown(KeyCode.V))//左から8番目
        {
            input = 7;
            magsize = input + mag;
            if (magsize > 12)
            {
                input = -1;
            }
        }
        if (Input.GetKeyDown(KeyCode.G))//左から9番目
        {
            input = 8;
            magsize = input + mag;
            if (magsize > 12)
            {
                input = -1;
            }
        }
        if (Input.GetKeyDown(KeyCode.B))//左から10番目
        {
            input = 9;
            magsize = input + mag;
            if (magsize > 12)
            {
                input = -1;
            }
        }
        if (Input.GetKeyDown(KeyCode.H))//左から11番目
        {
            input = 10;
            magsize = input + mag;
            if (magsize > 12)
            {
                input = -1;
            }
        }
        if (Input.GetKeyDown(KeyCode.N))//左から12番目
        {
            input = 11;
            magsize = input + mag;
            if (magsize > 12)
            {
                input = -1;
            }
        }//入力の数値化終了


        //デストロイヤー君の移動
        if(input >= 0)
        {
            if (capsel.NOTES[input, 0] != 0)
            {
                copyDes = (GameObject)Instantiate(Des, new Vector3(270 - (54 * input), (cc * -480) + 363 + mymove, 0), Quaternion.identity);
                copyDes.transform.SetParent(Parent, false);
                for (int d = 0; d < 5; d++)
                {
                    capsel.NOTES[input, d] = 0;
                }
                input = -1;

                Debug.Log("1000");
            }
        }


        //数値化された入力でロングノーツ生成
        if(input >= 0 && Input.GetKey(KeyCode.LeftShift) && longtap <= 0)
        {
            longtap = input;
            starthaku = haku;
            longcopy = (RectTransform)Instantiate(LongNotes, new Vector3(298 - (54 * input), (cc * -480) + 366 + mymove, 0), Quaternion.identity);
            longcopy.sizeDelta = LongWH;
            longcopy.transform.SetParent(Parent, false);

            LongStock.NOTES[longtap, 0] = 2;
            capsel.NOTES[longtap, 0] = LongStock.NOTES[longtap, 0];
            LongStock = new NotesStore();
            LongStock.NOTES[longtap, 1] = mag;
            capsel.NOTES[longtap, 1] = LongStock.NOTES[longtap, 1];
            LongStock = new NotesStore();

            input = -1;
        }
        if(longtap >= 0 && Input.GetKey(KeyCode.LeftShift))
        {
            
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                updown++;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                updown--;
            }
            if(updown < 0)
            {
                longtap = -1;
            }
        }
        if (longtap >= 0 && Input.GetKeyUp(KeyCode.LeftShift))
        {
            if(updown <= 0)
            {
                copyDes = (GameObject)Instantiate(Des, longcopy.localPosition + new Vector3(-20 ,-4 , 0 ), Quaternion.identity);
                copyDes.transform.SetParent(Parent, false);
                for (int d = 0; d < 5; d++)
                {
                    list[starthaku].NOTES[longtap, d] = 0;
                }
            }
            else
            {
                LongWH.y = updown * 491 / risum[bunsu];
                longcopy.sizeDelta = LongWH;

                LongStock.NOTES[longtap, 3] = risum[bunsu];
                list[starthaku].NOTES[longtap, 3] = LongStock.NOTES[longtap, 3];
                LongStock = new NotesStore();
                LongStock.NOTES[longtap, 4] = updown;
                list[starthaku].NOTES[longtap, 4] = LongStock.NOTES[longtap, 4];
                LongStock = new NotesStore();
            }
            longtap = -1;
            input = -1;
            updown = 0;
            LongWH = v;
        }

        //数値化された入力で単ノーツ生成
        if (input >= 0)
        {
            copy = (RectTransform)Instantiate(Notes, new Vector3(298 - (54 * input), (cc * -480) + 366 + mymove, 0), Quaternion.identity);
            copy.sizeDelta = WH;
            copy.transform.SetParent(Parent, false);
            capsel.NOTES[input, 0] = 1;
            capsel.NOTES[input, 1] = mag;
        }




        //出力チェック&セーブ機能
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            list[haku] = capsel;
            for(int me = 0;me < maxcc; me++)
            {
                for (int h = 0; h < 48; h++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        print_array += list[h * maxcc].OPTION[k].ToString();
                        if (k < 2) { print_array += ","; }
                    }
                    print_array += "|";
                    for (int i = 0; i < 12; i++)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            print_array += list[h * maxcc].NOTES[i, j].ToString();
                            if (j < 4) { print_array += ","; }
                        }
                        print_array += "|";
                    }
                    if (h < 47) { print_array += ";\n"; } //章の終わり
                    else { print_array += "@;\n"; }
                }
            }

            Debug.Log(print_array);
            print_array = "";
        }
    }
    
    public static int getmag()
    {
        return mag;
    }
}
