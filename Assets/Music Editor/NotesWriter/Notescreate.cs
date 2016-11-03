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
using System.Xml.Serialization;
using UnityEngine.SceneManagement;






public class Notescreate : MonoBehaviour {

    public static List<Musicnote> nakasio = new List<Musicnote>();

    public RectTransform Notes;                         //単ノーツのコピー元
    public RectTransform LongNotes;                     //ロングノーツのコピー元
    public RectTransform Slide;                         //スライドのコピー元
    public RectTransform Flick;                         //ノーマルフリックのコピー元
    public Sprite Left;                                 //左フリック
    public Sprite Right;                                 //右フリック
    public RectTransform Parent;                        //親指定
    public GameObject Des;                              //デストロイヤー君
    public RectTransform StopTime;                      //右に停止の値
    public RectTransform changeBPM;                        //左に変則の値を生成
    private int bunsu;                                  //左右の押された数(0～7)の格納先
    private int cc;                                     //現在の小節数
    private int maxcc;                                  //今までの最大の小節数
    private int max = 0;                                //最大小節が増えた際のトリガー
    private int[] risum = { 4, 8, 12, 16, 24, 32, 48 }; //分数の値
    private int input;                                  //キー入力の値

    private int  Optiontap = -1;
    private int changetap = -1;
    private int stoptap = -1;

    public static int mag = 1;                          //ノーツの幅
    private int stoptime;
    private int updownstop = 0;
    private double startBPM = 0;                             //xmlデータから読み込み引っ張ってくる
    private double change;
    private int updownBPM = 0;
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
    RectTransform stopcopy;
    RectTransform changecopy;
    GameObject copyDes;
    public Boolean yes = false;
    //List構造体
    public List<Musicnote> list = new List<Musicnote>();
    public Musicnote capsel = new Musicnote();
    public Musicnote None = new Musicnote(); //初期化用
    public Musicnote LongStock = new Musicnote(); //Londnotesのストック
    public Musicnote SlideStock = new Musicnote(); //slidenotes用のストック

    public List<int> delete = new List<int>();
    private int savehaku = 0;

    //Beat
    public List<int> measure = new List<int>();
    private int Beat = 0;
    private int haku = 0;
    private int hakucount = 0;

    string print_array = "";
    XMLWrite writer = new XMLWrite();
    private int ClacBeat = 0;
    private int mea = 0;

    void Start()
    {
        //Notesのサイズデータ
        Size = GameObject.Find("Notes").GetComponent<RectTransform>();
        v = Size.sizeDelta;
        WH = v;
        LongWH = v;

        //stoptime = 0;
        Debug.Log("春風亭翔太");
        //渡し
        try
        {
            List<Musicnote> nakasio = GlobalValue.getMusicParam();
            list = new List<Musicnote>(nakasio);

            startBPM = GlobalValue.getBpm();
            Debug.Log(startBPM);
            change = startBPM;
            //0小節
            measure.Add(0);
            maxcc = Keyscroll.getmaxcc(); //最大小節

            for (int www = 0; www < maxcc; www++)
            {
                measure.Add(48);
            }

            if (list.Count != 0)
            {

                for (int h = 1; h <= maxcc; h++)
                {
                    ClacBeat = 0;
                    for (int b = 0; b < h; b++)
                    {
                        ClacBeat += measure[b];
                    }
                    for (int e = ClacBeat; e < 48 + ClacBeat; e++)
                    {
                        if (list[e].Option[0] != 0)
                        {
                            change = list[e].Option[1];
                            changecopy = (RectTransform)Instantiate(changeBPM, new Vector3(330, -135 - (e * 10), 0), Quaternion.identity);
                            changecopy.transform.Rotate(0, 0, 180);
                            changecopy.GetComponent<Text>().text = "BPM\n" + change + "      _";
                            changecopy.transform.SetParent(Parent, false);
                        }

                        for (int j = 0; j < 12; j++)
                        {
                            if (list[e].NotesSet[j, 0] == 1)
                            {
                                copy = (RectTransform)Instantiate(Notes, new Vector3(298 - (54 * j), -114 - (e * 10), -1.01f), Quaternion.identity);
                                WH.x = v.x * list[e].NotesSet[j, 1];
                                copy.sizeDelta = WH;
                                copy.transform.SetParent(Parent, false);

                            }
                            if (list[e].NotesSet[j, 0] == 2)
                            {
                                slidecopy = (RectTransform)Instantiate(Slide, new Vector3(298 - (54 * j), -114 - (e * 10), -1.01f), Quaternion.identity);
                                WH.x = v.x * list[e].NotesSet[j, 1];
                                slidecopy.sizeDelta = WH;
                                slidecopy.transform.SetParent(Parent, false);

                                slidecopy.GetComponent<SlideCreate>().Slider(54 * (j - list[e].NotesSet[j, 2] - 1), list[e].NotesSet[j, 4] * (490 / list[e].NotesSet[j, 3]), list[e].NotesSet[j, 1]);

                            }
                            if (list[e].NotesSet[j, 0] == 3)
                            {
                                flickcopy = (RectTransform)Instantiate(Flick, new Vector3(298 - (54 * j), -114 - (e * 10), -1), Quaternion.identity);
                                WH.x = v.x * list[e].NotesSet[j, 1];
                                flickcopy.sizeDelta = WH;
                                flickcopy.transform.SetParent(Parent, false);

                                if (list[e].NotesSet[j, 2] == 0)
                                {
                                    flickcopy.GetComponent<Image>().sprite = Left;

                                }
                                if (list[e].NotesSet[j, 2] == 1)
                                {
                                    flickcopy.GetComponent<Image>().sprite = Right;
                                }
                            }
                        }
                    }
                }
                WH = v;
                capsel = list[0];
            }
        }
        catch
        {
            startBPM = 150;
            Debug.Log(startBPM);
            change = startBPM;
            measure.Add(0);
            for (int kal = 0; kal < 48; kal++)
            {
                list.Add(None);
                None = new Musicnote();
            }
            maxcc = Keyscroll.getmaxcc();
        }



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
            for (int a = 1; a <= maxcc; a++)
            {
                Beat += measure[a];
            }
            for (int h = measure[max]; h < Beat; h++)
            {
                list.Add(None);
                None = new Musicnote();
            }
            max = maxcc;
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
            /*if (stoptap == 1 && updownstop == 0)
            {
                copyDes = (GameObject)Instantiate(Des, stopcopy.localPosition, Quaternion.identity);
                copyDes.transform.SetParent(Parent, false);
                for (int d = 0; d < 3; d++)
                {
                    capsel. Option[d] = 0;
                }
            }*/


            if (changetap == 2 && updownBPM == 0)
            {
                copyDes = (GameObject)Instantiate(Des, changecopy.localPosition, Quaternion.identity);
                Debug.Log("copyDes is " + copyDes);
                copyDes.transform.SetParent(Parent, false);
                for (int d = 0; d < 3; d++)
                {
                    capsel. Option[d] = 0;
                }
            }

            //stoptime = 0;
            //updownstop = 0;
            //stoptap = -1;

            change = change + updownBPM;
            updownBPM = 0;
            changetap = -1;

            list[hakucount] = capsel;
            capsel = new Musicnote();
            capsel = list[haku];
            hakucount = haku;

        }

        input = -1;
        magsize = 0;
         Optiontap = -1;


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

        //changeBPM用の入力
        if (Input.GetKeyDown(KeyCode.R))
        {
             Optiontap = 2;
        }

        
        //変則値
        if (capsel. Option[0] == 2 &&  Optiontap == 2)      //すでに存在していて押したら
        {
            copyDes = (GameObject)Instantiate(Des, new Vector3(330, (cc * -480) + 345 + mymove, -1.01f), Quaternion.identity);
            copyDes.transform.SetParent(Parent, false);
            for (int d = 0; d < 3; d++)
            {
                capsel. Option[d] = 0;
            }
             Optiontap = -1;
            changetap = -1;
            updownBPM = 0;
            //destory
        }

        if ( Optiontap == 2 && changetap < 0)              //何もなく押したら
        {
            if (capsel. Option[1] != 0)
            {
                change = capsel. Option[1];
            }
            changetap =  Optiontap;
            changecopy = (RectTransform)Instantiate(changeBPM, new Vector3(330, (cc * -480) + 345 + mymove, -1.01f), Quaternion.identity);
            changecopy.transform.Rotate(0,0,180);
            changecopy.GetComponent<Text>().text = "BPM\n" + change + "      _";
            changecopy.transform.SetParent(Parent, false);
            capsel. Option[0] = 2;
            capsel. Option[1] = (int)change;
             Optiontap = 0;
            updownBPM = 0;
        }

        /*if(changetap == 2 &&  Optiontap == 1)               //押しててStopの方を押したら
        {
            copyDes = (GameObject)Instantiate(Des, changecopy.localPosition, Quaternion.identity);
            copyDes.transform.SetParent(Parent, false);
            for (int d = 0; d < 3; d++)
            {
                capsel. Option[d] = 0;
            }
             Optiontap = -1;
            changetap = -1;
            //Destroy
        }*/

        if (Input.GetKeyDown(KeyCode.E) && changetap == 2)
        {
            updownBPM -= 1;
            if (change + updownBPM <= 0)
            {
                updownBPM = 1 - (int)change;
            }
            capsel. Option[1] = (int)change + updownBPM;
            changecopy.GetComponent<Text>().text = "BPM\n" + (change + updownBPM) + "      _";
        }
        if (Input.GetKeyDown(KeyCode.T) && changetap == 2)
        {
            updownBPM += 1;
            capsel. Option[1] = (int)change + updownBPM;
            changecopy.GetComponent<Text>().text = "BPM\n" + (change + updownBPM) + "      _";
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

        if(risum[bunsu] == 32) //とりあえず32分のキャンセル
        {
            input = -1;
        }

        //デストロイヤー君の移動
        if(input >= 0)
        {
            if (capsel.NotesSet[input, 0] != 0)
            {
                copyDes = (GameObject)Instantiate(Des, new Vector3(270 - (54 * input), (cc * -480) + 363 + mymove, -1.01f), Quaternion.identity);
                copyDes.transform.SetParent(Parent, false);
                for (int d = 0; d < 5; d++)
                {
                    capsel.NotesSet[input, d] = 0;
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
            longcopy = (RectTransform)Instantiate(LongNotes, new Vector3(298 - (54 * input), (cc * -480) + 366 + mymove, -1.01f), Quaternion.identity);
            longcopy.sizeDelta = LongWH;
            longcopy.transform.SetParent(Parent, false);
            LongStock.NotesSet = capsel.NotesSet;

            LongStock.NotesSet[longtap, 0] = 2;
            LongStock.NotesSet[longtap, 1] = mag;
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
                copyDes = (GameObject)Instantiate(Des, longcopy.localPosition + new Vector3(-20 ,-4 , -1.01f), Quaternion.identity);
                copyDes.transform.SetParent(Parent, false);
                for (int d = 0; d < 5; d++)
                {
                    LongStock.NotesSet[longtap, d] = 0;
                    list[starthaku].NotesSet = LongStock.NotesSet;
                }
            }
            else
            {
                LongWH.y = v.y +(updown * 490 / risum[bunsu]);
                longcopy.sizeDelta = LongWH;

                LongStock.NotesSet[longtap, 2] = longtap + 1;
                LongStock.NotesSet[longtap, 3] = risum[bunsu];
                LongStock.NotesSet[longtap, 4] = updown;
                list[starthaku].NotesSet = LongStock.NotesSet;
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
            SlideStock.NotesSet = capsel.NotesSet;

            SlideStock.NotesSet[Slidefirsttap, 0] = 2;
            SlideStock.NotesSet[Slidefirsttap, 1] = mag;
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
                SlideStock.NotesSet[Slidefirsttap, d] = 0;
                list[starthaku].NotesSet = SlideStock.NotesSet;
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
                    SlideStock.NotesSet[Slidefirsttap, d] = 0;
                    list[starthaku].NotesSet = SlideStock.NotesSet;
                }
            }
            else
            {
                slidecopy.GetComponent<SlideCreate>().Slider(54 * (Slidefirsttap - Slidesecondtap), updown * (490 / risum[bunsu]),mag);

                SlideStock.NotesSet[Slidefirsttap, 2] = Slidesecondtap + 1;
                SlideStock.NotesSet[Slidefirsttap, 3] = risum[bunsu];
                SlideStock.NotesSet[Slidefirsttap, 4] = updown;
                list[starthaku].NotesSet = SlideStock.NotesSet;
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
            flickcopy = (RectTransform)Instantiate(Flick, new Vector3(298 - (54 * input), (cc * -480) + 366 + mymove, -1.01f), Quaternion.identity);
            flickcopy.sizeDelta = WH;
            flickcopy.transform.SetParent(Parent, false);
            capsel.NotesSet[flicktap, 0] = 3;
            capsel.NotesSet[flicktap, 1] = mag;

            input = -1;
        }
        if (flicktap >= 0 && Input.GetKeyUp(KeyCode.Space))
        {
            copyDes = (GameObject)Instantiate(Des, flickcopy.localPosition + new Vector3(-20, -4, -1.01f), Quaternion.identity);
            copyDes.transform.SetParent(Parent, false);
            for (int d = 0; d < 5; d++)
            {
                capsel.NotesSet[flicktap, d] = 0;
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
                capsel.NotesSet[flicktap, 2] = 0 ;

                flicktap = -1;
                input = -1;
            }
            if (Input.GetKeyDown(KeyCode.Period))
            {
                flickcopy.GetComponent<Image>().sprite = Right;
                capsel.NotesSet[flicktap, 2] = 1;

                flicktap = -1;
                input = -1;
            }
        }

        //数値化された入力で単ノーツ生成
        if (input >= 0)
        {
            copy = (RectTransform)Instantiate(Notes, new Vector3(298 - (54 * input), (cc * -480) + 366 + mymove, -1.01f), Quaternion.identity);
            copy.sizeDelta = WH;
            copy.transform.SetParent(Parent, false);
            capsel.NotesSet[input, 0] = 1;
            capsel.NotesSet[input, 1] = mag;
        }




        //出力チェック&セーブ機能
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            print_array = "";
            list[haku] = capsel;
            List<Musicnote> listsave = new List<Musicnote>(list);
            for (int h = 1; h <= maxcc; h++)
            {
                int deleteAddcount = 0;
                ClacBeat = 0;
                for (int b = 0; b < h; b++)    //挿入前の拍の合計
                {
                    ClacBeat += measure[b];
                }
                delete.Add(ClacBeat);
                for (int e = ClacBeat; e < 48 + ClacBeat; e++)
                {
                    if (listsave[e]. Option[0] != 0)
                    {
                        delete.Add(e);
                        deleteAddcount++;
                    } else {
                        for (int j = 0; j < 12; j++)
                        {
                            if (listsave[e].NotesSet[j ,0] != 0)
                            {
                                delete.Add(e);
                                deleteAddcount++;
                                break;
                            }
                        }
                    }
                }
                if (deleteAddcount == 0)
                {
                    savehaku = 4;
                    for (int f = 1; f <= savehaku; f++)
                    {
                        listsave.RemoveRange(ClacBeat + f, 11);
                    }
                }
                else
                {
                    int hakusa = 12;
                    int hakusatemp = 0;
                    for (int f = 1; f < delete.Count; f++)
                    {
                        hakusatemp = delete[f] - delete[f - 1];
                        hakusatemp = hakusatemp % 12;
                        if(hakusatemp == 0)
                        {
                            hakusatemp = 12;
                        }
                        if (hakusatemp < hakusa)
                        {
                            hakusa = hakusatemp;
                        }
                    }
                    Debug.Log("hakusa" + hakusa);
                    if (hakusa == 0 || hakusa == 12)
                    {
                        savehaku = 4;
                    }
                    else if (hakusa == 6)
                    {
                        savehaku = 8;
                    }
                    else if (hakusa == 4 || hakusa == 8)
                    {
                        savehaku = 12;
                    }
                    else if (hakusa == 3 || hakusa == 9)
                    {
                        savehaku = 16;
                    }
                    else if (hakusa == 2 || hakusa == 10)
                    {
                        savehaku = 24;
                    }
                    else
                    {
                        savehaku = 48;
                    }

                    if (savehaku == 4)
                    {
                        for (int f = 1; f <= savehaku; f++)
                        {
                            listsave.RemoveRange(ClacBeat + f, 11);
                        }
                    }
                    else if (savehaku == 8)
                    {
                        for (int f = 1; f <= savehaku; f++)
                        {
                            listsave.RemoveRange(ClacBeat + f, 5);
                        }
                    }
                    else if (savehaku == 12)
                    {
                        for (int f = 1; f <= savehaku; f++)
                        {
                            listsave.RemoveRange(ClacBeat + f, 3);
                        }
                    }
                    else if (savehaku == 16)
                    {
                        for (int f = 1; f <= savehaku; f++)
                        {
                            listsave.RemoveRange(ClacBeat + f, 2);
                        }
                    }
                    else if (savehaku == 24)
                    {
                        for (int f = 1; f <= savehaku; f++)
                        {
                            listsave.RemoveRange(ClacBeat + f, 1);
                        }
                    }
                }
                delete = new List<int>();
                measure[h] = savehaku;

            }


            
            for (int me = 0;me < maxcc; me++)
            {
                mea += measure[me];
                for (int h = 0; h < measure[me + 1]; h++)
                {
                    print_array += "|";
                    for (int k = 0; k < 3; k++)
                    {
                        print_array += listsave[h + mea]. Option[k].ToString();
                        if (k < 2) { print_array += ","; }
                    }
                    print_array += "|";
                    for (int i = 0; i < 12; i++)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            print_array += listsave[h + mea].NotesSet[i, j].ToString();
                            if (j < 4) { print_array += ","; }
                        }
                        print_array += "|";
                    }
                    if (h < measure[me + 1] - 1) { print_array += ";\r\n"; } //章の終わり
                    else { print_array += "@;\r\n"; }
                }
                
            }
            mea = 0;


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

            Debug.Log(print_array);
            listsave = new List<Musicnote>();
            for(int meas = 0;meas < maxcc; meas++)
            {
                measure[meas + 1] = 48;
            }

            SceneManager.LoadScene("XMLReading");
        }
    }

    public static int getmag()
    {
        return mag;
    }
}
