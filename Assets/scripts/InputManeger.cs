using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManeger : MonoBehaviour
{
    private static List<Player> players = new List<Player>();
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(players.Count);
    }

    public void AddPlayer(float r, float g, float b, int playerNr, int conrrollerNr)
    {
        Player temp = new Player
        {
            ControllerNr = conrrollerNr,
            PlayerNr = playerNr
        };
        temp.SetColur(r, g, b);

        players.Add(temp);
    }

    public void AddBot(float r, float g, float b, int playerNr)
    {
        Player temp = new Player
        {
            IsBot = true
        };
        temp.PlayerNr = playerNr;

        temp.SetColur(r, g, b);

        players.Add(temp);
    }

    public List<Player> GetPlayer() => players;
}

public class Player
{
    private bool isBot = false;
    private int playerNr, controllerNr;
    private float r, g, b;

    public bool IsBot { get => isBot; set => isBot = value; }
    public int PlayerNr { get => playerNr; set => playerNr = value; }
    public int ControllerNr { get => controllerNr; set => controllerNr = value; }
    public float R { get => r; set => r = value; }
    public float G { get => g; set => g = value; }
    public float B { get => b; set => b = value; }

    public void SetColur(float r, float g, float b)
    {
        this.R = r;
        this.B = b;
        this.G = g;
    }
}
    
