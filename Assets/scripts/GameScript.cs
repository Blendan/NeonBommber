using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScript : MonoBehaviour
{
    public GameObject player, bot;
    public InputManeger inputManeger;

    public Text[] pointDisplays;

    private List<GameObject> players = new List<GameObject>();

    private GameObject[] menue;

    public Text timerText, wonText;
    private bool isDone = false;

    public float timer = 200f;
    // Start is called before the first frame update
    void Start()
    {
        menue = GameObject.FindGameObjectsWithTag("WinnMenue");

        Hide();

        foreach(Player value in inputManeger.GetPlayer())
        {
            SpawnPlayer(value,0f,0f);
        }
    }

    private void SpawnPlayer(Player playerData, float x ,float y)
    {
        GameObject temp = Instantiate(player, new Vector3(x, y), new Quaternion(0, 0, 0, 0)) as GameObject;

        temp.gameObject.GetComponent<ColurControle>().SetColur(playerData.R, playerData.G, playerData.B);
        temp.gameObject.GetComponent<Movment>().player = playerData.PlayerNr + "";
        temp.gameObject.GetComponent<Movment>().controlerNr = playerData.ControllerNr;

        temp.gameObject.GetComponent<PointCounter>().text = pointDisplays[playerData.PlayerNr-1];

        players.Add(temp);
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
