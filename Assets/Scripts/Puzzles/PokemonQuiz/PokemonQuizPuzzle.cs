using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PokemonQuizPuzzle : BasePuzzle
{
    [SerializeField] private Questions questions;
    [SerializeField] private TMP_Text tmpPergunta;
    [SerializeField] private Toggle[] toggleRepostas;
    [SerializeField] private List<RawImage> pokebolasPlayer;
    [SerializeField] private List<RawImage> pokebolasBot;
    [SerializeField] private GameObject detailsAfterLoose;


    private int currentQuestion = 0;
    private int correctAnswer = 0;
    private int countToLose = 3;
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
                    RemoveLifeFromBot();
                }
                else
                {
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
        if (pokebolasBot.Count < countToLose)
        {
            //player win
            SetPlayerPrefValue("PokemonQuizPuzzle", 1);
            
        }
        else if (pokebolasPlayer.Count < countToLose)
        {
            StartCoroutine(ReloadScene());
        }
    }

    private IEnumerator ReloadScene()
    {
        ShowDetailsAfterLose();

        yield return new WaitForSeconds(3f);
        
        LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    private void ShowDetailsAfterLose()
    {

    }

    public void LoadNextScene()
    {
        LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
