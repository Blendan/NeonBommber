using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
    private GameObject[] pauseObjects;
    public Button btnUnfreze, btnEnd, btnEnd2 ,btnRestart;
    private bool isPaused = false;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;

        pauseObjects = GameObject.FindGameObjectsWithTag("PauseMenue");

        btnEnd.onClick.AddListener(EndGame);
        btnEnd2.onClick.AddListener(EndGame);
        btnUnfreze.onClick.AddListener(Unfreze);
        btnRestart.onClick.AddListener(RestartGame);

        Debug.Log(pauseObjects.Length);

        hidePaused();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)||Input.GetKeyDown("joystick button 7"))
        {
            if (!isPaused)
            {
                Time.timeScale = 0;
                Debug.Log("paused");

                isPaused = true;

                showPaused();
            }
            else
            {
                Unfreze();
            }
        }

        if (Time.timeScale == 0)
        {
            if(Input.GetKeyDown("joystick button 0"))
            {
                Unfreze();
            }
            else if(Input.GetKeyDown("joystick button 1"))
            {
                EndGame();
            }
        }
    }

    private void RestartGame()
    {
        SceneManager.LoadScene("game");
    }

    private void EndGame()
    {
        Debug.Log("Exit");
        Application.Quit();
    }

    private void Unfreze()
    {
        Debug.Log("unpaused");
        Time.timeScale = 1;
        isPaused = false;
        hidePaused();
    }

    private void hidePaused()
    {
        foreach (GameObject g in pauseObjects)
        {
            g.SetActive(false);
        }
    }

    private void showPaused()
    {
        foreach (GameObject g in pauseObjects)
        {
            g.SetActive(true);
        }
    }
}
