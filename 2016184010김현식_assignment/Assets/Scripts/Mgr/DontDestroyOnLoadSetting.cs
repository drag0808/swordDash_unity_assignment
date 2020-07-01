using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyOnLoadSetting : MonoBehaviour
{

    [SerializeField] private GameObject scoreManager;

    // Start is called before the first frame update
    void Start()
    {
        scoreManager = GameObject.Find("scoreManager");

        DontDestroyOnLoad(scoreManager);
    }

    private void Update()
    {
        SceneManager.LoadScene("startScene");
    }
}
