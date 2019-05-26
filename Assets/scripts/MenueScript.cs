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
        gameObject.GetComponent<InputManeger>().AddPlayer(0f, 1f, 1f, 1,2);
        gameObject.GetComponent<InputManeger>().AddPlayer(1f, 0f, 1f, 2,1);

        SceneManager.LoadScene("game");
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
