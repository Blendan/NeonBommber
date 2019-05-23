using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
    private GameObject[] pauseObjects;
    public Button btnUnfreze, btnEnd;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;

        pauseObjects = GameObject.FindGameObjectsWithTag("PauseMenue");

        btnEnd.onClick.AddListener(EndGame);
        btnUnfreze.onClick.AddListener(Unfreze);

        hidePaused();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)||Input.GetKeyDown("joystick button 7"))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                Debug.Log("paused");

                showPaused();
            }
            else if (Time.timeScale == 0)
            {
                Debug.Log("unpaused");
                Time.timeScale = 1;
                hidePaused();
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

    private void EndGame()
    {
        Debug.Log("Exit");
        Application.Quit();
    }

    private void Unfreze()
    {
        Debug.Log("unpaused");
        Time.timeScale = 1;
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
