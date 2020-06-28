using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startSceneController : MonoBehaviour
{
    public void sceneChangeToPlay()
    {
        SceneManager.LoadScene("RunStage");
    }
}
