using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    Player_Controller player_Controller;

    private GameObject dash_1;
    private GameObject dash_2;
    private GameObject hp_1;
    private GameObject hp_2;
    private GameObject hp_3;

    private int[] nowStat = {0,0};


    [SerializeField] private GameObject Img_rePlay;

    ///////////////////////////////////////
    ///////////////////////////////////////
    //////  타이머

    [Header("타이머 표시 text UI")]
    [SerializeField] private Text timerText;

    public bool timerOn = true;
    public float totalTime = 0f;

    private int minute = 0;
    private int second = 0;
    private int tic = 0;

    ///////////////////////////////////////
    ///////////////////////////////////////

    [Header("스코어메니저(안지울 오브젝트)")]
    [SerializeField] GameObject ScoreManager;
    ScoreManager scoreManager;

    
    [SerializeField] private InputField InputName;

    [SerializeField] bool isPlay;


    void Start()
    {
        player_Controller = GameObject.Find("Player").GetComponent<Player_Controller>();
        dash_1 = GameObject.Find("dash_1");
        dash_2 = GameObject.Find("dash_2");
        hp_1 = GameObject.Find("hp_1");
        hp_2 = GameObject.Find("hp_2");
        hp_3 = GameObject.Find("hp_3");     

        nowStat = player_Controller.getDashAndHp();


        ScoreManager = GameObject.Find("scoreManager");
        scoreManager = ScoreManager.GetComponent<ScoreManager>();

        isPlay = true;

    }

    // Update is called once per frame
    void Update()
    {
        //print(isPlay);
        if(isPlay)
        {
            //  대시, 체력 UI 표시
            if (nowStat != player_Controller.getDashAndHp())
            {
                nowStat = player_Controller.getDashAndHp();
                setUI();
            }

            //  타이머 
            if (timerOn)
            {
                totalTime += Time.deltaTime;
            }
            timerText.text = TimerCalc();
        }

    }



    ////    체력, 대시 UI 업데이트
    void setUI()
    {
        // 대시
        if(nowStat[0] == 2) 
        {
            dash_1.SetActive(true);
            dash_2.SetActive(true);
        }
        else if (nowStat[0] == 1)
        {
            dash_1.SetActive(false);
            dash_2.SetActive(true);
        }
        else if (nowStat[0] == 0)
        {
            dash_1.SetActive(false);
            dash_2.SetActive(false);
        }

        // 체력
        if (nowStat[1] == 3)
        {
            hp_1.SetActive(true);
            hp_2.SetActive(true);
            hp_3.SetActive(true);
        }
        else if (nowStat[1] == 2)
        {
            hp_1.SetActive(false);
            hp_2.SetActive(true);
            hp_3.SetActive(true);
        }
        else if (nowStat[1] == 1)
        {
            hp_1.SetActive(false);
            hp_2.SetActive(false);
            hp_3.SetActive(true);
        }
    }



    ////    타이머 표시
    private string TimerCalc()
    {
        tic = (int)((totalTime % 1) * 100);
        second = (int)totalTime % 60;
        minute = (int)totalTime / 60;

        if (second < 10)
            return minute + " : 0" + second + " : " + tic;
        return minute + " : " + second + " : " + tic;
    }
    
    public void raceEnd()
    {
        timerOn = false;
        print("레이스 끝");
        Img_rePlay.SetActive(true);
    }

    public void saveData()
    {
        scoreManager.scoreSave();
    }


    public float getScore()
    {
        return totalTime;
    }
    public string getName()
    {
        return InputName.text;
    }

    public void setIsPlay(bool stat)
    {
        isPlay = stat;
    }

}
