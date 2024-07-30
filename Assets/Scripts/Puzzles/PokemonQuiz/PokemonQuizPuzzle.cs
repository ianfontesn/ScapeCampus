using JetBrains.Annotations;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PokemonQuizPuzzle : BasePuzzle
{
    [SerializeField] private Questions questions;
    [SerializeField] private TMP_Text tmpPergunta;
    [SerializeField] private Toggle[] toggleRepostas;
    [SerializeField] private List<RawImage> pokebolasPlayer;
    [SerializeField] private List<RawImage> pokebolasBot;


    private int currentQuestion = 0;
    private int correctAnswer = 0;
    private Dictionary<int, bool> questionsUsed = new();
    private Dictionary<int, bool> answerList = new();

    private void Start()
    {
        FillDictionaryQuestions();
        SetNewQuestion();
    }

    private void FillDictionaryQuestions()
    {
        for (int i = 0; i < questions.GetAllQuestions().Length; i++)
        {
            questionsUsed.Add(i, false);
        }
    }

    private void SetNewQuestion()
    {
        if (!questionsUsed.ContainsValue(false))
        {
            ReloadScene();
        }
        else
        {
            var questionIndex = Random.Range(0, questions.GetAllQuestions().Length);

            if (questionsUsed.ContainsKey(questionIndex) && questionsUsed[questionIndex] == false)
            {
                tmpPergunta.text = questions.GetQuestion(questionIndex);

                var answers = questions.GetAnswer(questionIndex);
                for (int i = 0; i < toggleRepostas.Length; i++)
                {
                    toggleRepostas[i].GetComponentInChildren<Text>().text = answers[i];
                }

                correctAnswer = questions.GetCurrentCorrectAnswer(questionIndex);
                questionsUsed[questionIndex] = true;
            }
            else
            {
                SetNewQuestion();
            }
        }
    }

    public void RespondQuestion()
    {
        for (int i = 0; i < toggleRepostas.Length; i++)
        {
            if (toggleRepostas[i].isOn) 
            {
                if (correctAnswer == i)
                {
                    Debug.Log("Resposta Correta");
                    RemoveLifeFromBot();
                }
                else
                {
                    Debug.Log("Resposta Errada");
                    RemoveLifeFromPlayer();
                }

                break;
            }
        }

        SetNewQuestion();

    }

    private void RemoveLifeFromPlayer()
    {
        if(pokebolasPlayer.Count > 0)
        {
            pokebolasPlayer[pokebolasPlayer.Count - 1].enabled = false;
            pokebolasPlayer.RemoveAt(pokebolasPlayer.Count - 1);
        }

        VerifyWinner();
    }

    private void RemoveLifeFromBot()
    {
        if (pokebolasBot.Count > 0)
        {
            pokebolasBot[pokebolasBot.Count - 1].enabled = false;
            pokebolasBot.RemoveAt(pokebolasBot.Count - 1);

        }

        VerifyWinner() ;
    }

    private void VerifyWinner()
    {
        if (pokebolasBot.Count == 0)
        {
            Debug.Log("BOT WIN");
            ReloadScene();
            
        }
        else if (pokebolasPlayer.Count == 0)
        {
            Debug.Log("PLAYER WIN");
            SetPlayerPrefValue("PokemonQuizPuzzle", 1);
        }
    }

    private void ReloadScene()
    {
        LoadSceneAsync(1);
    }

    public void ChangeScene()
    {
        LoadSceneAsync(2);
    }
}
