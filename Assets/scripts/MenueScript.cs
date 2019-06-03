using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenueScript : MonoBehaviour
{
    public Button btnStart, btnEnd;
    // Start is called before the first frame update
    void Start()
    {
        btnStart.onClick.AddListener(StartGame);
        btnEnd.onClick.AddListener(EndGame);
    }

    private void StartGame()
    { 
        SceneManager.LoadScene("control");
    }

    private void EndGame()
    {
        Debug.Log("Exit");
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("joystick button 0"))
        {
            StartGame();
        }
        else if (Input.GetKeyDown("joystick button 1"))
        {
            EndGame();
        }
    }
}
