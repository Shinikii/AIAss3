using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameState;

public class AIPlayer : MonoBehaviour
{
    public Text player01;
    public Text player02;
    public GameController main;
    public StateNode curNode;
    private int count;

    public Button reset;

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
                    GameState cloneState = new GameState();
                    cloneState.state = new char[3, 3];

                    for (int a = 0; a < 3; a++)
                    {
                        for (int b = 0; b < 3; b++)
                        {
                            cloneState.state[a, b] = root.gs.state[a, b];
                        }
                    }

                    cloneState.state[c, r] = startingPlayer;

                    StateNode newNode = new StateNode();
                    newNode.gs = cloneState;
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
            if (GameController.IsWin('x', root.gs) == true)
            {
                root.minimaxValue = 1;
            }
            else if (GameController.IsWin('o', root.gs) == true)
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
                int max = -999999;
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
                int min = 999999;

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
        return GameController.IsWin('x', gs) == true || GameController.IsWin('o', gs) == true || GameController.IsDraw(gs) == true;
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
            int min = 999999;

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
