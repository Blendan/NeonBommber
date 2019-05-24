using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScript : MonoBehaviour
{
    public GameObject[] players;

    private GameObject[] menue;

    public Text timerText, wonText;
    private bool isDone = false;

    public float timer = 200f;
    // Start is called before the first frame update
    void Start()
    {
        menue = GameObject.FindGameObjectsWithTag("WinnMenue");

        Hide();
    }

    private void Hide()
    {
        foreach(GameObject value in menue)
        {
            value.SetActive(false);
        }
    }

    private void Show()
    {
        foreach (GameObject value in menue)
        {
            value.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        string timenow = ((int)timer / 60) + ":";

        int seconds = (int)timer % 60;

        if(seconds<10)
        {
            timenow += "0" + seconds;
        }
        else
        {
            timenow += seconds;
        }

        timerText.text = timenow;

        if(timer <= 0 && !isDone)
        {
            ShowWonnScreen();
        }

        if(isDone)
        {
            if (Input.GetKeyDown("joystick button 0"))
            {
                SceneManager.LoadScene("game");
            }
            else if (Input.GetKeyDown("joystick button 1"))
            {
                Application.Quit();
            }
        }
    }

    private void ShowWonnScreen()
    {
        isDone = true;
        Time.timeScale = 0;
        Show();
        int maxPoints = -1;
        string player = "";
        bool draw = false; 

        foreach(GameObject value in players)
        {
            int points = value.GetComponent<PointCounter>().GetPoints();

            Debug.Log(points);

            if(points > maxPoints)
            {
                player = value.GetComponent<Movment>().player;
                maxPoints = points;
                draw = false;
            }
            else if(points == maxPoints)
            {
                draw = true;
            }
        }

        if(draw)
        {
            wonText.text = "Draw";
        }
        else
        {
            wonText.text = "Player "+player+"\nWon";
        }
    }
}
