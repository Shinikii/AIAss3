using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateStates(StateNode root, char startingPlayer)
    {
        // count the number of game states generated
        count++;

        // start when X moves first
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                if (root.gs.state[i, j] == 0 && IsTerminalState(root.gs) == false)
                {
                    var copiedGameState = root.gs.Copy();
                    copiedGameState.state[i, j] = startingPlayer;

                    var newNode = new StateNode();
                    newNode.gs = copiedGameState;
                    newNode.parent = root;
                    root.children.Add(newNode);

                    newNode.action = Tuple.Create(i, j);

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
            root.minimaxValue = Utility(root);
        }
        else
        {
            if (root.player == 'x')
            {
                // the max player
                var max = int.MinValue;

                foreach (var child in root.children)
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
                var min = int.MaxValue;

                foreach (var child in root.children)
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

    private bool IsTerminalState(GameState gs)
    {
        return IsWinningState('x', gs) == true || IsWinningState('o', gs) == true || IsDraw(gs) == true;
    }
}
