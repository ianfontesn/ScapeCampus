using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ProgrammingLogicPuzzle : BasePuzzle
{
    /*
        Vetor de soluções:
        Cada posição do vetor é preenchida com 0 e 1
        Cada posição, representa um target (pedaço do código)
        Se a posição for correta entre dois targets, suas posições no vetor são salvas como 1,
        Se a posição for incorreta, ou os targets sairem de posição, suas posições serão zeradas no vetor
        Ao ordenar corretamente cada um dos targets, o vetor estará 
        totalmente preenchido com 1 e o próximo desafio será liberado adicionando um PlayerPref da cena.
     */

    private bool[] solution = new bool[6];
    private int indexAnswer = 3;
    private readonly Dictionary<string, string> validTransitions = new()
    {
        { "if(", "n" },
        { "n", "%" },
        { "%", "2" },
        { "2", "==" },
        { "==", "0" }
    };
    private bool waitCoroutine = false;

    [SerializeField] private TMP_Text tmpBalao;
    [SerializeField] private Toggle toggleInformativo;
    [SerializeField] private Transform toggleGroupAnswers;
    [SerializeField] private GameObject tmpCenaIncompleta;




    private void Start()
    {
        ResetPlayerPrefs();
    }

    public void OnTriggerEnterEvent(string tagWhoTriggered, string tagWhoWasTriggered)
    {
        UpdateSolutionStatus(tagWhoTriggered, tagWhoWasTriggered, true);
    }

    public void OnTriggerExitEvent(string tagWhoTriggered, string tagWhoWasTriggered)
    {
        UpdateSolutionStatus(tagWhoTriggered, tagWhoWasTriggered, false);
    }

    /// <summary>
    /// Altera o estado da solução a cada evento disparado pelo trigger dos targets
    /// </summary>
    /// <param name="tagWhoWasTriggered">Quem foi acionado</param>
    /// <param name="tagWhoTriggered">Quem acionou</param>
    /// <param name="triggerType">true: triggerEnter, false: triggerExit</param>
    private void UpdateSolutionStatus(string tagWhoWasTriggered, string tagWhoTriggered, bool triggerType)
    {
        if (validTransitions.ContainsKey(tagWhoWasTriggered) && validTransitions[tagWhoWasTriggered].Equals(tagWhoTriggered))
        {
            int index = Array.IndexOf(validTransitions.Keys.ToArray(), tagWhoWasTriggered);
            if (index >= 0 && index < solution.Length - 1)
            {
                solution[index] = triggerType;
                solution[index + 1] = triggerType;
            }
        }

        CheckSolution();

    }

    private void CheckSolution()
    {
        if (!solution.Contains(false))
        {
            ChangeText(1);
            ShowAnswers();
            toggleInformativo.isOn = true;
        }
    }

    private void ChangeText(int text)
    {
        switch (text)
        {
            case 1:
                tmpBalao.text = "Muito bom. Você acertou a sequência! Mas... Foi chute? Responda abaixo o que essa lógica realiza.";
                break;

            case 2:
                tmpBalao.text = "Incrível! Você sabe mesmo em? Agora está liberado a seguir para a segunda tarefa.";
                break;

            case 3:
                tmpBalao.text = "Resposta incorreta.";
                break;
        }

    }

    private void ShowAnswers()
    {
        toggleGroupAnswers.gameObject.SetActive(true);
    }

    public void CheckCorrectAnswers()
    {
        for (int i = 0; i < toggleGroupAnswers.childCount; i++)
        {
            var toggle = toggleGroupAnswers.GetChild(i).GetComponent<Toggle>();

            if (toggle != null && toggle.isOn)
            {
                if (i == indexAnswer)
                {
                    SetPlayerPrefValue("ProgrammingLogicPuzzle", 1);
                    ChangeText(2);
                }
                else
                {
                    ChangeText(3);
                }
            }
        }

    }

    public void ChangeScene()
    {
        if (!TryLoadSceneAsync(1) && !waitCoroutine)
        {
            waitCoroutine = true;
            StartCoroutine(ShowIncompleteScene());
        }
    }

    private IEnumerator ShowIncompleteScene()
    {
        tmpCenaIncompleta.SetActive(true);
        yield return new WaitForSeconds(2);
        tmpCenaIncompleta.SetActive(false);
        waitCoroutine = false;
    }
}
