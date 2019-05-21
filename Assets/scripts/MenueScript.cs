using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenueScript : MonoBehaviour
{
    public Button btnStart;
    // Start is called before the first frame update
    void Start()
    {
        btnStart.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        SceneManager.LoadScene("game");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
