using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControllerSelector : MonoBehaviour
{
    private bool gotPlayer = false;
    private int controlerForP1 = -1, controlerForP2 = -1;

    public Text p1Text, p2Text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void StartGame()
    {
        SceneManager.LoadScene("game");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            SetPlayer1(0);

            gotPlayer = true;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            SetPlayer2(0);

            gotPlayer = true;
        }

        if (Input.GetKey("joystick 1 button 0"))
        {
            if(controlerForP1==-1 && controlerForP2 != 1)
            {
                SetPlayer1(1);
            }
            else if (controlerForP2 == -1 && controlerForP1 != 1)
            {
                SetPlayer2(1);
                Debug.Log("thisding");
            }

            gotPlayer = true;
        }

        if (Input.GetKey("joystick 2 button 0"))
        {
            if (controlerForP1 == -1 && controlerForP1 == 2)
            {
                SetPlayer1(2);
            }
            else if(controlerForP2 == -1 && controlerForP1 != 2)
            {
                SetPlayer2(2);
            }

            gotPlayer = true;
        }

        if(gotPlayer)
        {
            if(Input.GetKeyDown("joystick button 7")||Input.GetKeyDown(KeyCode.Space))
            {
                if (controlerForP1 == -1)
                {
                    gameObject.GetComponent<InputManeger>().AddBot(0f, 1f, 1f, 1);
                }
                else
                {
                    gameObject.GetComponent<InputManeger>().AddPlayer(0f, 1f, 1f, 1, controlerForP1);
                }

                if (controlerForP2 == -1)
                {
                    gameObject.GetComponent<InputManeger>().AddBot(1f, 0f, 1f, 2);
                }
                else
                {
                    gameObject.GetComponent<InputManeger>().AddPlayer(1f, 0f, 1f, 2, controlerForP2);
                }

                StartGame();
            }
        }
    }

    private void SetPlayer1(int c)
    {
        controlerForP1 = c;

        p1Text.text = "P1 active";
    }

    private void SetPlayer2(int c)
    {
        controlerForP2 = c;

        p2Text.text = "P2 active";
    }
}
