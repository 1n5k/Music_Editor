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
    public RectTransform Flick;                         //ノーマルフリックのコピー元
    public Sprite Left;                                 //左フリック
    public Sprite Right;                                 //右フリック
    public RectTransform Parent;                        //親指定
    public GameObject Des;                              //デストロイヤー君
    public RectTransform changeBPM;                        //左に変則の値を生成
    private int bunsu;                                  //左右の押された数(0～7)の格納先
    private int cc;                                     //現在の小節数
    private int maxcc;                                  //今までの最大の小節数
    private int max = 0;                                //最大小節が増えた際のトリガー
    private int[] risum = { 4, 8, 12, 16, 24, 32, 48 }; //分数の値
    private int input;                                  //キー入力の値

    private int optiontap = 0;
    private int changetap = 0;
    private int stoptap = 0;

    public static int mag = 1;                          //ノーツの幅
    private int stoptime;
    private int startBPM = 0;                             //xmlデータから読み込み引っ張ってくる
    private int magsize;                                //レーン横幅の上限を越えないための天井値

    public float mymove;                                //0～-480(1小節の移動幅)

    //ロングノーツ用の格納
    private int longtap = -1;
    private int starthaku = 0;
    private int updown = 0;

    //スライド用格納
    private int Slidefirsttap = -1;
    private int Slidesecondtap = -1;

    //フリック用格納
    private int flicktap = -1;

    RectTransform Size;                                    //ノーツのサイズデータ格納先
    private Vector2 v;                                  //サイズデータ格納
    private Vector2 WH;                              //変化後の横幅
    private Vector2 LongWH;
    private float fl = 0;
    //コピー
    RectTransform copy;
    RectTransform longcopy;
    RectTransform slidecopy;
    RectTransform flickcopy;
    RectTransform changecopy;
    GameObject copyDes;
    public Boolean yes = false;
    //List構造体
    public List<NotesStore> list = new List<NotesStore>();
    public NotesStore capsel = new NotesStore();
    public NotesStore None = new NotesStore(); //初期化用
    public NotesStore LongStock = new NotesStore(); //Londnotesのストック
    public NotesStore SlideStock = new NotesStore(); //slidenotes用のストック
    //Beat
    public List<int> measure = new List<int>();
    private int Beat = 0;
    private int haku = 0;
    private int hakucount = 0;

    public string print_array = "";
    public XMLWrite writer = new XMLWrite();

    void Start()
    {
        //Notesのサイズデータ
        Size = GameObject.Find("Notes").GetComponent<RectTransform>();
        v = Size.sizeDelta;
        WH = v;
        LongWH = v;

        stoptime = 0;

        startBPM = 220;   //xmlデータから読み込み引っ張ってくる

        //0小節
        measure.Add(0);
    }


    void Update() {
        mymove = 10 * Keyscroll.getYsum();//バーに追従する移動量
        bunsu = Keyscroll.geti(); //○分の配列管理
        cc = Keyscroll.getcc(); //現在いる小節
        maxcc = Keyscroll.getmaxcc(); //最大小節
        Debug.Log(mymove);
        

        //新しい小節の長さ決めと初期化
        if (max < maxcc)
        {
            Beat = 0;
            measure.Add(48);
            for (int a = 0; a <= maxcc; a++)
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
        Debug.Log("haku" + haku);

        //拍が変わるたびに前のlistにcapselの値を入れ、capselに変わった後のlistの値を入れる
        if (haku != hakucount)
        {
            list[hakucount] = capsel;
            capsel = new NotesStore();
            capsel = list[haku];
            hakucount = haku;
        }

        input = -1;
        magsize = 0;


        //ノーツの幅変更
        if (Input.GetKeyDown(KeyCode.Q))
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
            if (mag > 12)
            {
                mag = 12;
            }
            WH.x = v.x * mag;
            LongWH.x = v.x * mag;
        }

        //stoptime用の入力
        if (Input.GetKeyDown(KeyCode.R))
        {
            optiontap = 1;
            changetap = 0;
        }
        //changeBPM用の入力
        if (Input.GetKeyDown(KeyCode.U))
        {
            optiontap = 2;
            stoptap = 0;
        }
        /*
        //停止時間
        if (Input.GetKeyDown(KeyCode.E))
        {
            stoptime -= -1;
            if(stoptime == 0)
            {
                capsel.OPTION[0] = 0;
                capsel.OPTION[1] = 0;
                capsel.OPTION[2] = 0;
            }
            else
            {

            }
            
        }
        if (Input.GetKeyDown(KeyCode.T))
        {

        }*/

        //BPMの幅変更 レーンの横に表示
        if (capsel.OPTION[0] == 2 && optiontap == 2)      //すでに存在していて押したら
        {
            optiontap = 0;
            //destory
        }

        if (optiontap == 2 && changetap < 0)              //何もなく押したら
        {
            changetap = optiontap;
            changecopy = (RectTransform)Instantiate(changeBPM, new Vector3(298, (cc * -480) + 366 + mymove, 0), Quaternion.identity);
            changecopy.GetComponent<Text>().text = " BPM\n" + list[haku].OPTION[1];
            changecopy.transform.SetParent(Parent, false);
            capsel.OPTION[0] = 2;
            capsel.OPTION[1] = startBPM;
            optiontap = 0;
        }

        if(changetap > 0 && optiontap == 1)               //押しててStopの方を押したら
        {
            //Destroy
        }


        if (Input.GetKeyDown(KeyCode.Y) && changetap == 2)
        {

        }
        if (Input.GetKeyDown(KeyCode.I) && changetap == 2)
        {

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
                longtap = -1;
                Slidefirsttap = -1;
                updown = 0;
                flicktap = -1; 

                Debug.Log("1000");
            }
        }


        //数値化された入力でロングノーツ生成
        if (input >= 0 && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.RightShift) != true && Input.GetKey(KeyCode.Space) != true && longtap < 0)
        {
            longtap = input;
            starthaku = haku;
            longcopy = (RectTransform)Instantiate(LongNotes, new Vector3(298 - (54 * input), (cc * -480) + 366 + mymove, 0), Quaternion.identity);
            longcopy.sizeDelta = LongWH;
            longcopy.transform.SetParent(Parent, false);
            LongStock.NOTES = capsel.NOTES;

            LongStock.NOTES[longtap, 0] = 2;
            LongStock.NOTES[longtap, 1] = mag;
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
        }
        if (longtap >= 0 && Input.GetKeyUp(KeyCode.LeftShift))
        {
            if(updown <= 0)
            {
                copyDes = (GameObject)Instantiate(Des, longcopy.localPosition + new Vector3(-20 ,-4 , 0 ), Quaternion.identity);
                copyDes.transform.SetParent(Parent, false);
                for (int d = 0; d < 5; d++)
                {
                    LongStock.NOTES[longtap, d] = 0;
                    list[starthaku].NOTES = LongStock.NOTES;
                }
            }
            else
            {
                LongWH.y = v.y +(updown * 490 / risum[bunsu]);
                longcopy.sizeDelta = LongWH;

                LongStock.NOTES[longtap, 3] = risum[bunsu];
                LongStock.NOTES[longtap, 4] = updown;
                list[starthaku].NOTES = LongStock.NOTES;
            }
            longtap = -1;
            input = -1;
            updown = 0;
            LongWH = v;
        }


        //数値化された入力でスライド生成
        if (input >= 0 && Input.GetKey(KeyCode.RightShift) && Input.GetKey(KeyCode.LeftShift) != true && Input.GetKey(KeyCode.Space) != true && Slidefirsttap < 0)
        {
            Slidefirsttap = input;
            starthaku = haku;
            slidecopy = (RectTransform)Instantiate(Slide, new Vector3(298 - (54 * input), (cc * -480) + 366 + mymove, -1), Quaternion.identity);
            slidecopy.sizeDelta = LongWH;
            slidecopy.transform.SetParent(Parent, false);
            SlideStock.NOTES = capsel.NOTES;

            SlideStock.NOTES[Slidefirsttap, 0] = 2;
            SlideStock.NOTES[Slidefirsttap, 1] = mag;
            input = -1;
        }
        if (Slidefirsttap >= 0 && Input.GetKey(KeyCode.RightShift))
        {

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                updown++;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                updown--;
            }
        }
        if (Slidefirsttap >= 0 && Input.GetKeyUp(KeyCode.RightShift))
        {
            copyDes = (GameObject)Instantiate(Des, slidecopy.localPosition + new Vector3(-20, -4, -1), Quaternion.identity);
            copyDes.transform.SetParent(Parent, false);
            for (int d = 0; d < 5; d++)
            {
                SlideStock.NOTES[Slidefirsttap, d] = 0;
                list[starthaku].NOTES = SlideStock.NOTES;
            }
            Slidefirsttap = -1;
            input = -1;
            updown = 0;
        }

        if (Slidefirsttap >= 0 && Input.GetKey(KeyCode.RightShift) && input >= 0)
        {
            Slidesecondtap = input;
            if (updown <= 0)
            {
                copyDes = (GameObject)Instantiate(Des, slidecopy.localPosition + new Vector3(-20, -4, -1), Quaternion.identity);
                copyDes.transform.SetParent(Parent, false);
                for (int d = 0; d < 5; d++)
                {
                    SlideStock.NOTES[Slidefirsttap, d] = 0;
                    list[starthaku].NOTES = SlideStock.NOTES;
                }
            }
            else
            {
                slidecopy.GetComponent<SlideCreate>().Slider(54 * (Slidefirsttap - Slidesecondtap), updown * (490 / risum[bunsu]));

                SlideStock.NOTES[Slidefirsttap, 2] = Slidesecondtap;
                SlideStock.NOTES[Slidefirsttap, 3] = risum[bunsu];
                SlideStock.NOTES[Slidefirsttap, 4] = updown;
                list[starthaku].NOTES = SlideStock.NOTES;
            }
            Slidefirsttap = -1;
            Slidesecondtap = -1;
            input = -1;
            updown = 0;
        }
         
        //数値化された入力でフリック生成
        if (input >= 0 && Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.RightShift) != true && Input.GetKey(KeyCode.LeftShift) != true && flicktap < 0)
        {
            flicktap = input;
            flickcopy = (RectTransform)Instantiate(Flick, new Vector3(298 - (54 * input), (cc * -480) + 366 + mymove, -1), Quaternion.identity);
            flickcopy.sizeDelta = WH;
            flickcopy.transform.SetParent(Parent, false);
            capsel.NOTES[flicktap, 0] = 3;
            capsel.NOTES[flicktap, 1] = mag;

            input = -1;
        }
        if (flicktap >= 0 && Input.GetKeyUp(KeyCode.Space))
        {
            copyDes = (GameObject)Instantiate(Des, flickcopy.localPosition + new Vector3(-20, -4, -1), Quaternion.identity);
            copyDes.transform.SetParent(Parent, false);
            for (int d = 0; d < 5; d++)
            {
                capsel.NOTES[flicktap, d] = 0;
            }
            flicktap = -1;
            input = -1;
        }

        if (flicktap >= 0 && Input.GetKey(KeyCode.Space))
        {
            if (Input.GetKeyDown(KeyCode.Comma))
            {
                //画像を張り替える
                flickcopy.GetComponent<Image>().sprite = Left;
                capsel.NOTES[flicktap, 2] = 0 ;

                flicktap = -1;
                input = -1;
            }
            if (Input.GetKeyDown(KeyCode.Period))
            {
                flickcopy.GetComponent<Image>().sprite = Right;
                capsel.NOTES[flicktap, 2] = 1;

                flicktap = -1;
                input = -1;
            }
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
            for (int me = 0;me < maxcc; me++)
            {
                for (int h = 0; h < 48; h++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        print_array += list[h + (48 * me)].OPTION[k].ToString();
                        if (k < 2) { print_array += ","; }
                    }
                    print_array += "|";
                    for (int i = 0; i < 12; i++)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            print_array += list[h + (48 * me)].NOTES[i, j].ToString();
                            if (j < 4) { print_array += ","; }
                        }
                        print_array += "|";
                    }
                    if (h < 47) { print_array += ";\n"; } //章の終わり
                    else { print_array += "@;\n"; }
                }
            }
            
           // finalle();
            //XMLWriter writer = GetComponent<XMLWriter>();
            if (writer!=null)
            {
                yes = writer.Write(ref print_array);
                Debug.Log(yes);
                print_array = "";
            }
            else
            {
                Debug.Log(writer);
                Debug.Log("writer is NULL");
            }
        }
    }

    public static int getmag()
    {
        return mag;
    }
}
