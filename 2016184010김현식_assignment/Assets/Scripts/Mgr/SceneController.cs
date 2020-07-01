using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private UiManager uiManager;

    private void Start()
    {
        uiManager = GameObject.Find("Canvas").GetComponent<UiManager>();
    }


    public void sceneChangeToPlay()
    {
        SceneManager.LoadScene("RunStage");    
    }

    public void sceneChangeToStart()
    {
        uiManager.saveData();
        SceneManager.LoadScene("startScene");

    }
}

