using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIPlayer : MonoBehaviour
{
    public Text player01;
    public Text player02;
    public GameController main;
    public StateNode curNode;
    private int count;

    public Button reset;

    public Text win1;
    public Text win2;

    // Start is called before the first frame update
    void Start()
    {
        curNode = new StateNode();
        curNode.gs = new GameState();
        curNode.gs.state = new char[3, 3];
        curNode.player = 'x';
        GenerateStates(curNode, 'x');
        var terminalNodes = new List<StateNode>();
        GetTerminalNodes(curNode, terminalNodes);
        Debug.Log("Generated " + terminalNodes.Count + " terminal nodes");
        Debug.Log("Generated " + count + " nodes");
        win1.text = " ";
        win2.text = " ";
    }

    public void Reboot()
    {
        curNode = new StateNode();
        curNode.gs = new GameState();
        curNode.gs.state = new char[3, 3];
        curNode.player = 'x';
        GenerateStates(curNode, 'x');
        var terminalNodes = new List<StateNode>();
        GetTerminalNodes(curNode, terminalNodes);
        Debug.Log("Generated " + terminalNodes.Count + " terminal nodes");
        Debug.Log("Generated " + count + " nodes");
        win1.text = " ";
        win2.text = " ";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public class GameState
    {
        public char[,] state;

        public GameState()
        {
            state = new char[3, 3];
        }

        public GameState Copy()
        {
            GameState copy = new GameState();
            copy.state = new char[3, 3];

            for (int c = 0; c < 3; c++)
            {
                for (int r = 0; r < 3; r++)
                {
                    copy.state[c, r] = state[c, r];
                }
            }
            return copy;
        }
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

    private void GenerateStates(StateNode root, char startingPlayer)
    {
        // count the number of game states generated
        count++;

        // start when X moves first
        for (int c = 0; c < 3; c++)
        {
            for (int r = 0; r < 3; r++)
            {
                if (root.gs.state[c, r] == 0 && IsTerminal(root.gs) == false)
                {
                    GameState copiedGameState = root.gs.Copy();
                    copiedGameState.state[c, r] = startingPlayer;

                    StateNode newNode = new StateNode();
                    newNode.gs = copiedGameState;
                    newNode.parent = root;
                    root.children.Add(newNode);

                    newNode.action = Tuple.Create(c, r);

                    if (startingPlayer == 'x')
                    {
                        newNode.player = 'o';
                        GenerateStates(newNode, 'o');
                    }
                    else if (startingPlayer == 'o')
                    {
                        newNode.player = 'x';
                        GenerateStates(newNode, 'x');
                    }
                    else
                    {
                        Console.WriteLine("Error: Unknown button value");
                        return;
                    }
                }
            }
        }

        // If we reach a terminal node then it's minimax value is simply the outcome of the game since there are no more moves to play
        if (root.children.Count == 0)
        {
            if (IsWin('x', root.gs) == true)
            {
                root.minimaxValue = 1;
            }
            else if (IsWin('o', root.gs) == true)
            {
                root.minimaxValue = -1;
            }
            else
            {
                root.minimaxValue = 0;
            }
        }
        else
        {
            if (root.player == 'x')
            {
                // the max player
                int max = int.MinValue;
                foreach (StateNode child in root.children)
                {
                    if (child.minimaxValue > max)
                    {
                        max = child.minimaxValue;
                    }
                }

                root.minimaxValue = max;
            }
            else
            {
                // min player
                int min = int.MaxValue;

                foreach (StateNode child in root.children)
                {
                    if (child.minimaxValue < min)
                    {
                        min = child.minimaxValue;
                    }
                }

                root.minimaxValue = min;
            }
        }

    }

    private bool IsTerminal(GameState gs)
    {
        return IsWin('x', gs) == true || IsWin('o', gs) == true || IsDraw(gs) == true;
    }

    private bool IsWin(char player, GameState gs)
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

    private bool IsDraw(GameState gs)
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

    private void GetTerminalNodes(StateNode root, List<StateNode> terminalNodes)
    {
        if (root.children.Count == 0)
        {
            terminalNodes.Add(root);
        }
        else
        {
            foreach (StateNode node in root.children)
            {
                GetTerminalNodes(node, terminalNodes);
            }
        }
    }

    public void CheckState()
    {

        if (IsWin('o', curNode.gs) == true)
        {
            Debug.Log("Computer won!");
            main.gameRunning = false;
            reset.gameObject.SetActive(true);
            win2.text = "Winner!";
        }
        else if (IsWin('x', curNode.gs) == true)
        {
           Debug.Log("Player won!");
            main.gameRunning = false;
            reset.gameObject.SetActive(true);
            win1.text = "Winner!";
        }
        else if (IsDraw(curNode.gs) == true)
        {
            Debug.Log("Draw");
            main.gameRunning = false;
            reset.gameObject.SetActive(true);
            win1.text = "Draw!";
            win2.text = "Draw!";
        }
    }

    public void IterateTree(Tuple<int, int> action)
    {
        foreach (StateNode n in curNode.children)
        {
            if (n.action.Item1 == action.Item1 && n.action.Item2 == action.Item2)
            {
                curNode = n;
                Debug.Log("Moving down tree");
                return;
            }
        }
    }

    public void BotTurn()
    {

        if (main.gameRunning == true)
        {

            // choose the min minimax value
            StateNode minNode = null;
            int min = int.MaxValue;

            foreach (StateNode n in curNode.children)
            {
                if (n.minimaxValue < min)
                {
                    min = n.minimaxValue;
                    minNode = n;
                }
            }

            if (minNode != null)
            {
                // perform the ai move now
                main.FillO(minNode.action.Item1 * 3 + minNode.action.Item2);
                Debug.Log("AI moving to " + minNode.action.Item1 + ", " + minNode.action.Item2);
            }
            else
            {
                Debug.Log("Error node is null");
                return;
            }
        }
    }
}
