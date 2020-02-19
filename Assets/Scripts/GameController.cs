using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

            bot.IterateTree(Tuple.Create(row, col));
            bot.curNode.gs.state[row, col] = 'x';           
            zones[buttonNumber].gameObject.transform.GetChild(1).gameObject.SetActive(true);
            zones[buttonNumber].gameObject.GetComponentInChildren<ParticleSystem>().Play();
            player1.text = (row + ", " + col);
            bot.CheckState();
        }
    }

    public void FillO(int buttonNumber)
    {
        if (gameRunning == true)
        {
            var row = (buttonNumber) / 3;
            var col = (buttonNumber) % 3;

            bot.IterateTree(Tuple.Create(row, col));
            bot.curNode.gs.state[row, col] = 'o';
            zones[buttonNumber].gameObject.transform.GetChild(0).gameObject.SetActive(true);
            zones[buttonNumber].gameObject.GetComponentInChildren<ParticleSystem>().Play();
            player2.text = (row + ", " + col);
            bot.CheckState();
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
    }

    
}
