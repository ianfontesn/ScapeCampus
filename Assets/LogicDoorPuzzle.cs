using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LogicDoorPuzzle : BasePuzzle
{
    private readonly Dictionary<string, string> validPositions = new()
    {
        { "1", "and" },
        { "2", "and" },
        { "3", "or" },
        { "4", "nor" },
        { "5", "nand" },
    };

    private bool[] solution = new bool [5];

    public void OnTriggerEnterEvent(string tagWhoTriggered, string tagWhoWasTriggered)
    {
        Debug.Log("trigger enter " + "tagWhoTriggered: " + tagWhoTriggered + " tagWhoWasTriggered: " + tagWhoWasTriggered);
        UpdateSolutionStatus(tagWhoTriggered, tagWhoWasTriggered, true);
    }

    public void OnTriggerExitEvent(string tagWhoTriggered, string tagWhoWasTriggered)
    {
        Debug.Log("trigger exit" + "tagWhoTriggered: " + tagWhoTriggered + " tagWhoWasTriggered: " + tagWhoWasTriggered);

        UpdateSolutionStatus(tagWhoTriggered, tagWhoWasTriggered, false);
    }

    private void UpdateSolutionStatus(string tagWhoTriggered, string tagWhoWasTriggered, bool isOnPosition)
    {
        if (validPositions.ContainsKey(tagWhoWasTriggered))
        {
            if (validPositions[tagWhoWasTriggered].Equals(tagWhoTriggered))
            {
                int.TryParse(tagWhoWasTriggered, out int pos);
                Debug.Log(pos);

                solution[pos -1] = isOnPosition;
            }

            string debugstring = "";
            foreach(var s in solution)
            {
               debugstring += (s + " | ");
            }
            Debug.Log(debugstring);

            CheckSolution();
        }
    }

    private void CheckSolution()
    {
        if (!solution.Contains(false))
        {
            Debug.Log("SOLUÇÃO COMPLETA.");
        }
    }
}
