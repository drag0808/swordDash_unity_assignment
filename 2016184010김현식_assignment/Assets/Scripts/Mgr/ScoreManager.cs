using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreManager : MonoBehaviour
{
    private UiManager uiManager;
    //[Header("이름 리스트")]
    [SerializeField] private List<string> names;
    //[Header("스코어 리스트")]
    [SerializeField] private List<float> scores;


    public void scoreSave()
    {
        uiManager = GameObject.Find("Canvas").GetComponent<UiManager>();

        float scoreTemp = uiManager.getScore();
        string nameTemp = uiManager.getName();

        scores.Add(scoreTemp);
        names.Add(nameTemp);
        
        // 등수 받기
        if (scores.Count > 5)
        {
            if (scores[4] < scores[5])
            {
                scores.RemoveAt(5);
                names.RemoveAt(5);
            }

            else
            {
                scores[4] = scores[5];
                scores.RemoveAt(5);
                names[4] = names[5];
                names.RemoveAt(5);
            }
        }
        
        // 등수 정렬
        for(int i = 0; i < scores.Count; i++)
        {
            for(int j = 0; j < scores.Count; j++)
            {
                if(scores[i] < scores[j])
                {
                    float temp = scores[j];
                    scores[j] = scores[i];
                    scores[i] = temp;

                    string temp2 = names[j];
                    names[j] = names[i];
                    names[i] = temp2;
                }
            }
        }
        
        

    }

    public List<float> getScores()
    {
        return scores;
    }
    public List<string> getNames()
    {
        return names;
    }




}
