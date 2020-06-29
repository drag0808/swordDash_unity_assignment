using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    [Header("타이머 표시 text UI")]
    [SerializeField] private Text timerText;

    public bool timerOn = true;
    public float totalTime = 0f;

    private int minute = 0;
    private int second = 0;
    private int tic = 0;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (timerOn)
        {
            totalTime += Time.deltaTime;
        }
        timerText.text = TimerCalc();
    }


    private string TimerCalc()
    {
        tic = (int)((totalTime % 1) * 100);
        second = (int)totalTime % 60;
        minute = (int)totalTime / 60;

        if(second < 10)
            return minute + " : 0" + second + " : " + tic;
        return minute + " : " + second + " : " + tic;
    }

}
