using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine.UI;
public class MusicData_
{

    public string Title;
    public string Subtitle;
    public string Artist;
    public double Bpm;
    public string MusicAddress;
    public string Jackect;
    public int SelectedDifficult;
    public int[] Difficulty = new int[4];
    public string MovieAddress;
    public double Offset;
    public double SelectOffset;
    public double Selectlong;
    public string[] Notes = new string[4];
    public double HiSpeed;
    //public int[,,] NotesSet = new int[300, 12, 5]; //ノーツ保存庫
    public List<Musicnote> NoteList = new List<Musicnote>();
    public List<int> Line = new List<int>(); // [BeatPerMeasure]の保存庫
    public List<double> BMSArr = new List<double>();   //各拍のBMSカウントを算出する
   // public List<LongArr> LongNotes = new List<LongArr>();   //各拍のロングノーツのBMSカウントを取得する
    public List<LongArr> LongNotesList = new List<LongArr>(); //処理したロングノーツを格納する
    public double DeffBPM;
};

public class Musicnote
{
    public int[,] NotesSet = new int[12, 5];   //ノーツ保管庫
    public int[] Option = new int[3];    //変則等保存庫
}

/// <summary>
/// ロングノーツを拍ごとに格納
/// </summary>
public class LongArr {
    /// <summary>
    /// ロングノーツの開始カウントと終了カウントを格納する
    /// </summary>
    public int Keynum;
    public int CutBeat; //何分で刻むかを格納
    public int Count;   //何拍分かを格納
    public int NowCount; //現在何拍目であるかを格納
    public double StartLineCount;   //ロングノーツがどのカウントで開始しているかを格納
    public double EndLineCount;     //ロングノーツがどのカウントで終了するかを格納
}

public class GlobalValue : MonoBehaviour
{
    public static MusicData_ MusicParam;
    private GameObject movie;
    public int MaxNotes = 0;
    public double BMSCount = 0;
    //double FPS = 60;
    public double Multi = 10000;  //1小節を何カウントに分解するかの分解能の設定
    int NowBeat = 0;
    //double DeffBPM;
    bool endflag = false;
    double FPS = 60.0;
    bool OffsetFlag = false;
    public float Offset = 0;
    double temp =0;
    public Text MusicName;
    public GameObject JacketPlane;
    int LongCheckCount = 0;
    
    IEnumerator GetJacketImage(GameObject gameObject, string filePath)
    {
        WWW file = new WWW("file://" + System.IO.Directory.GetCurrentDirectory() + filePath);
        yield return file;
        gameObject.GetComponent<Renderer>().material.mainTexture = file.texture;
    }
    // <summary>
    // ノーツを解析する
    // </summary>
    // <param name="analyze">解析する文字列</param>
    public void AnalyzingNotes(string analyze)
    {
        int k = 0;  //多次元配列格納用
        int linepos = 1;    //何拍目にいるか
        int notepos = 0;    //譜面のどのブロックを見ているか
        //int l = 0;          //Option位置格納変数
        //int m = 0;          //Line位置格納用変数
        int befline = 0;    //全小節の位置を格納
        MusicParam = new MusicData_();
        bool spaceflag = false; //空白(' ')を発見したら、次の'|'まで読み込みを停止するフラグ
        Musicnote buff = new Musicnote();
        StreamWriter log = new StreamWriter(Directory.GetCurrentDirectory() + @"\log.txt", true);
        XMLLoader op = new XMLLoader();
        //Debug.Log("analyze lenghth is "+analyze.Length);
        //Debug.Log("analyze[773] is "+"["+analyze[773]+"]");
        //Debug.Log("buff.Noteset[] is "+buff.NotesSet.Length);
        for (int i = 0; i < analyze.Length; i++)
        {
           
            if (analyze[i]== ' ')
            {
              //  Debug.Log("analyze[" + i + "] is space");
                spaceflag = true;   //このフラグが起動中は譜面配列化を停止する
                continue;
            }
            if (analyze[i] == '|')
            {
              //  Debug.Log("analyze["+i+"] is |");
                spaceflag = false; //読み込みフラグ解除
                notepos++;
                notepos %= 14;
            }
            else if (analyze[i] == '@')           //小節の終わり
            {
                MusicParam.Line.Add(linepos-befline);      //１小節の拍数を格納する
             //   Debug.Log("analyze[" + i + "] is @");
                befline = linepos;
            }
            else if ( analyze[i] == '\n')
            {
              // Debug.Log("analyze[" + i + "] is \\n");

                linepos++;
                
                MusicParam.NoteList.Add(buff);
                
                //Debug.Log("clear!");
               
                buff = new Musicnote();
            }
            else if(analyze[i] == '\r')
            {
               // Debug.Log("analyze[" + i + "] is \\r");
                continue;
            }
            else if (analyze[i] == '\t')
            {
               // Debug.Log("analyze[" + i + "] is \\t");
            }
            
            else {
                if (spaceflag == false)
                {
                    if (analyze[i] == ';' || analyze[i] == ',')      //拍の終わり
                    {
                      // Debug.Log("analyze[" + i + "] is ; or ,");
                    }
                    else if (notepos < 2)       //変則読み込みモード
                    {
                        //Debug.Log("bufflen is "+buff.Option.Length);
                        //Debug.Log("k is "+k.ToString()+"nowbeat is"+linepos.ToString());
                        buff.Option[k] = ConvertStringToInt(ref i,analyze);

                        
                        k++;
                        k %= 3;
                    }
                    else
                    {
                        if (notepos <= 13)
                        {
                            int q = notepos - 2;
                            
                           // Debug.Log("q is " + q);
                           
                            buff.NotesSet[notepos - 2, k] = ConvertStringToInt(ref i, analyze);
                            //log.Write(buff.NotesSet[notepos - 2, k]);
                            if (k == 0)
                            {
                                //Debug.Log("Char.IsNumber()");
                                if (Char.IsNumber(analyze[i])&&analyze[i] !='0')
                                {
                                    
                                    MaxNotes++;
                                }
                            }
                            //MusicParam.NoteList[linepos].NotesSet[notepos - 2, k] = ConvertStringToInt(ref i,analyze);   //格納している拍,レーン位置,各データの位置に値を格納
                            k++;
                            k %= 5;
                        }else
                        {
                        }
                    }
                    
                }
            }
            //Debug.Log(op.errnum);
            //op.errnum++;
            
        }

        Debug.Log("桂歌○");
        Debug.Log(MusicParam.NoteList.Count);

        log.Flush();
        log.Close();

    }

    int  ConvertStringToInt(ref int i,string analyze)
    {
        string setstr = "";
        for(int j = i;j < analyze.Length; j++)
        {
            if(char.IsNumber(analyze, j))
            {
                setstr += analyze[j];
                i = j;
            }else
            {
                break;
            }
        }
        return Convert.ToInt32(setstr.ToString());
    }




    public static List<Musicnote> getMusicParam()
    {
        return MusicParam.NoteList;
    }
    public static double getBpm()
    {
        return MusicParam.Bpm;
    }
}
