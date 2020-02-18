using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private StateNode curNode;
    private GameObject[] zones = new GameObject[9];
    private int count;
    public bool gameRunning = true;

    public GameObject spot0;
    public GameObject spot1;
    public GameObject spot2;
    public GameObject spot3;
    public GameObject spot4;
    public GameObject spot5;
    public GameObject spot6;
    public GameObject spot7;
    public GameObject spot8;

    public Text textBox1;


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

        curNode = new StateNode();
        curNode.gs = new GameState();
        curNode.gs.state = new char[3,3];
        curNode.player = 'x';

    }

    // Update is called once per frame
    void Update()
    {

    }

    

    public class StateNode
    {
        public GameState gs;
        public StateNode parent;
        public List<StateNode> children;
        public int minimaxValue;
        public char player;
        public Tuple<int, int> action = new Tuple<int, int>(-1, -1);
        public StateNode()
        {
            gs = new GameState();
            children = new List<StateNode>();
        }
    }

    public class GameState
    {
        public char[,] state;

        public GameState()
        {
            state = new char[3, 3];
        }

        /// <summary>
        /// Checks if this game state is equal to another game state
        /// </summary>
        /// <param name="gs"></param>
        /// <returns></returns>
        public bool Equals(GameState gs)
        {

            for (var i = 0; i < state.GetLength(0); i++)
            {
                for (var j = 0; j < state.GetLength(1); j++)
                {
                    if (state[i, j] != gs.state[i, j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Returns a fresh copy of this game state
        /// </summary>
        /// <returns></returns>
        public GameState Copy()
        {
            var copy = new GameState();
            copy.state = new char[3, 3];

            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    copy.state[i, j] = state[i, j];
                }
            }
            return copy;
        }
    }

    public void PlaneClick(int index)
    {
        FillX(index);
        //PassTurn();
    }

    void FillX(int buttonNumber)
    {
        if (gameRunning == true)
        {
            var row = (buttonNumber - 1) / 3;
            var col = (buttonNumber - 1) % 3;

            MoveThroughTree(Tuple.Create(row, col));
            curNode.gs.state[row, col] = 'x';           
            zones[buttonNumber].gameObject.transform.GetChild(0).gameObject.SetActive(true);
            CheckState();
        }
    }

    void FillO(int buttonNumber)
    {
        if (gameRunning == true)
        {
            var row = (buttonNumber - 1) / 3;
            var col = (buttonNumber - 1) % 3;

            MoveThroughTree(Tuple.Create(row, col));
            curNode.gs.state[row, col] = 'o';
            zones[buttonNumber].gameObject.transform.GetChild(1).gameObject.SetActive(true);
            CheckState();
        }
    }

    private void CheckState()
    {

        /*if (IsWinningState('o', curNode.gs) == true)
        {
            textBox1.text = "Computer won!";
            gameRunning = false;
        }
        else if (IsWinningState('x', curNode.gs) == true)
        {
            textBox1.text = "Player won!";
            gameRunning = false;
        }
        else if (IsDraw(curNode.gs) == true)
        {
            textBox1.text = "Draw";
            gameRunning = false;
        }*/
    }

    private void MoveThroughTree(Tuple<int, int> action)
    {
        foreach (var n in curNode.children)
        {
            if (n.action.Item1 == action.Item1 && n.action.Item2 == action.Item2)
            {
                curNode = n;
                Debug.Log("Moving down tree");
                return;
            }
        }
    }
}
