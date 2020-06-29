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

    // Start is called before the first frame update
    void Start()
    {
        player_Controller = GameObject.Find("Player").GetComponent<Player_Controller>();

        dash_1 = GameObject.Find("dash_1");
        dash_2 = GameObject.Find("dash_2");

        hp_1 = GameObject.Find("hp_1");
        hp_2 = GameObject.Find("hp_2");
        hp_3 = GameObject.Find("hp_3");


        nowStat = player_Controller.getDashAndHp();
    }

    // Update is called once per frame
    void Update()
    {
        if (nowStat != player_Controller.getDashAndHp())
        {
            nowStat = player_Controller.getDashAndHp();
            setUI();
        }

    }

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

}
