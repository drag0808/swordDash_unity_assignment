using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.TextCore.LowLevel;
using UnityEngine.UI;

public class ScorePrintManager : MonoBehaviour
{
    private GameObject go_scoreManager;
    private ScoreManager scoreManager;

    [SerializeField] private Text name_1;
    [SerializeField] private Text name_2;
    [SerializeField] private Text name_3;
    [SerializeField] private Text name_4;
    [SerializeField] private Text name_5;
    [SerializeField] private Text score_1;
    [SerializeField] private Text score_2;
    [SerializeField] private Text score_3;
    [SerializeField] private Text score_4;
    [SerializeField] private Text score_5;

    [SerializeField] private List<float> scores;
    [SerializeField] private List<string> scoresSt;
    [SerializeField] private List<string> names;
    private int tic;
    private int second;
    private int minute;

    // Start is called before the first frame update
    void Start()
    {
        go_scoreManager = GameObject.Find("scoreManager");
        scoreManager = go_scoreManager.GetComponent<ScoreManager>();


        scores = scoreManager.getScores();
        names = scoreManager.getNames();


        //scoresSt.Clear();

        for(int i = 0; i < scores.Count; ++i)
        {
            scoresSt.Add(TimerCalc(scores[i]));
        }

        if (scores.Count < 1)
            return;
        else
        {
            for (int i = 0; i < scores.Count; ++i)
            {
                print(scores[i]);
                print(name[i]);
               
               
                if (i == 0)
                {
                    name_1.text = names[i];
                    score_1.text = scoresSt[i];
                }
                else if (i == 1)
                {
                    name_2.text = names[i];
                    score_2.text = scoresSt[i];
                }
                else if (i == 2)
                {
                    name_3.text = names[i];
                    score_3.text = scoresSt[i];
                }
                else if (i == 3)
                {
                    name_4.text = names[i];
                    score_4.text = scoresSt[i];
                }
                else if (i == 4)
                {
                    name_5.text = names[i];
                    score_5.text = scoresSt[i];
                }
            }
        }

    }



    private string TimerCalc(float totalTime)
    {
        tic = (int)((totalTime % 1) * 100);
        second = (int)totalTime % 60;
        minute = (int)totalTime / 60;

        string ticSt = tic.ToString();
        string secondSt = second.ToString();
        string minuteSt = minute.ToString();

        if (second < 10)
            return minuteSt + " : 0" + secondSt + " : " + ticSt;
        return minuteSt + " : " + secondSt + " : " + ticSt;
    }



}
