using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameState;

public class GameController : MonoBehaviour
{
    private GameObject[] zones = new GameObject[9];
    public bool gameRunning = true;

    public AIPlayer bot;

    public GameObject spot0;
    public GameObject spot1;
    public GameObject spot2;
    public GameObject spot3;
    public GameObject spot4;
    public GameObject spot5;
    public GameObject spot6;
    public GameObject spot7;
    public GameObject spot8;

    public Button reset;
    public Text player1;
    public Text player2;

    public Text win1;
    public Text win2;

    public GameState curState;

    // Start is called before the first frame update
    void Start()
    {
        zones[0] = spot0;
        zones[1] = spot1;
        zones[2] = spot2;
        zones[3] = spot3;
        zones[4] = spot4;
        zones[5] = spot5;
        zones[6] = spot6;
        zones[7] = spot7;
        zones[8] = spot8;

        Button btn = reset.GetComponent<Button>();
        btn.onClick.AddListener(resetGame);

        reset.gameObject.SetActive(false);

        win1.text = " ";
        win2.text = " ";

        curState = new GameState();
        curState.state = new char[3, 3];
    }

    // Update is called once per frame
    void Update()
    {

    }  

    public void PlaneClick(int index)
    {
        if (zones[index].gameObject.transform.GetChild(0).gameObject.activeInHierarchy != true && zones[index].gameObject.transform.GetChild(1).gameObject.activeInHierarchy != true)
        {
            FillX(index);
            bot.BotTurn();
        }
        else if (gameRunning == true)
        {
            player2.text = ("Illegal Space");
        }
    }

    void FillX(int buttonNumber)
    {
        if (gameRunning == true)
        {
            var row = (buttonNumber) / 3;
            var col = (buttonNumber) % 3;

            curState.state[row, col] = 'x';
            bot.IterateTree(Tuple.Create(row, col));
            bot.curNode.gs.state[row, col] = 'x';           
            zones[buttonNumber].gameObject.transform.GetChild(1).gameObject.SetActive(true);
            zones[buttonNumber].gameObject.GetComponentInChildren<ParticleSystem>().Play();
            player1.text = (row + ", " + col);
            CheckState();
        }
    }

    public void FillO(int buttonNumber)
    {
        if (gameRunning == true)
        {
            var row = (buttonNumber) / 3;
            var col = (buttonNumber) % 3;

            curState.state[row, col] = 'o';
            bot.IterateTree(Tuple.Create(row, col));
            bot.curNode.gs.state[row, col] = 'o';
            zones[buttonNumber].gameObject.transform.GetChild(0).gameObject.SetActive(true);
            zones[buttonNumber].gameObject.GetComponentInChildren<ParticleSystem>().Play();
            player2.text = (row + ", " + col);
            CheckState();
        }
    }

    public static bool IsWin(char player, GameState gs)
    {
        // check horizontally
        for (int c = 0; c < 3; c++)
        {
            int horizontalCount = 0;
            for (int r = 0; r < 3; r++)
            {
                if (gs.state[c, r] == player)
                {
                    horizontalCount++;
                }
            }

            if (horizontalCount == 3)
            {
                return true;
            }
        }

        // check vertically
        for (int c = 0; c < 3; c++)
        {
            int verticalCount = 0;

            for (int r = 0; r < 3; r++)
            {
                if (gs.state[r, c] == player)
                {
                    verticalCount++;
                }
            }

            if (verticalCount == 3)
            {
                return true;
            }
        }

        // check both diagonals

        if (gs.state[0, 0] == player && gs.state[1, 1] == player && gs.state[2, 2] == player)
        {
            return true;
        }
        else if (gs.state[2, 0] == player && gs.state[1, 1] == player && gs.state[0, 2] == player)
        {
            return true;
        }

        return false;
    }

    public static bool IsDraw(GameState gs)
    {
        int occupied = 0;

        for (int c = 0; c < 3; c++)
        {
            for (int r = 0; r < 3; r++)
            {
                if (gs.state[c, r] != 0)
                {
                    occupied++;
                }
            }
        }

        if (occupied == 9 && IsWin('x', gs) == false && IsWin('o', gs) == false)
        {
            return true;
        }

        return false;
    }

    public void CheckState()
    {

        if (IsWin('o', curState) == true)
        {
            Debug.Log("Computer won!");
            gameRunning = false;
            reset.gameObject.SetActive(true);
            win2.text = "Winner!";
        }
        else if (IsWin('x', curState) == true)
        {
            Debug.Log("Player won!");
            gameRunning = false;
            reset.gameObject.SetActive(true);
            win1.text = "Winner!";
        }
        else if (IsDraw(curState) == true)
        {
            Debug.Log("Draw");
            gameRunning = false;
            reset.gameObject.SetActive(true);
            win1.text = "Draw!";
            win2.text = "Draw!";
        }
    }

    void resetGame()
    {
        for (int i = 0; i < 9; i++)
        {
            zones[i].gameObject.transform.GetChild(0).gameObject.SetActive(false);
            zones[i].gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }

        bot.Reboot();
        gameRunning = true;
        reset.gameObject.SetActive(false);

        win1.text = " ";
        win2.text = " ";

        curState = new GameState();
        curState.state = new char[3, 3];
    }

    
}
