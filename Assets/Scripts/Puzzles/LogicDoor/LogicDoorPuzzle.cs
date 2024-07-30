using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LogicDoorPuzzle : BasePuzzle
{
    [SerializeField] private Animator _keyAnimator;
    [SerializeField] private Image _imageBackground;
    [SerializeField] private GameObject toggleInformativo;
    [SerializeField] private GameObject winObjects;
    [SerializeField] private GameObject targets;


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
        UpdateSolutionStatus(tagWhoTriggered, tagWhoWasTriggered, true);
    }

    public void OnTriggerExitEvent(string tagWhoTriggered, string tagWhoWasTriggered)
    {
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

            CheckSolution();
        }
    }

    private void CheckSolution()
    {
        if (!solution.Contains(false))
        {
            StartCoroutine(ActiveWinSequence());
        }
    }

    private IEnumerator ActiveWinSequence()
    {
        if (_keyAnimator.gameObject.activeSelf)
        {
            _keyAnimator.SetTrigger("win");
        }

        toggleInformativo.SetActive(false);
        yield return new WaitForSeconds(3f);
        targets.SetActive(false);


        float duration = 2.0f; 
        float elapsed = 0.0f;

        Color color = _imageBackground.color;
        color.a = 0;
        _imageBackground.color = color;
        

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsed / duration);
            _imageBackground.color = color;
            yield return null;
        }

        color.a = 1;
        _imageBackground.color = color;
        winObjects.SetActive(true);
    }

    public void ReloadAll()
    {
        TryLoadSceneAsync(0);
    }
}

