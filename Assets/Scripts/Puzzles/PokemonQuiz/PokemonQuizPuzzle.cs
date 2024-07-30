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
    [SerializeField] private GameObject answerGroup;
    [SerializeField] private Toggle[] toggleRepostas;
    [SerializeField] private List<RawImage> pokebolasPlayer;
    [SerializeField] private List<RawImage> pokebolasBot;
    [SerializeField] private GameObject detailsAfterLose;
    [SerializeField] private GameObject canvaDetails;
    [SerializeField] private GameObject canvaPrincipal;
    [SerializeField] private GameObject canvaBloqueio;
    [SerializeField] private GameObject tmpCenaIncompleta;


    private int currentQuestion = 0;
    private int correctAnswer = 0;
    private int countToLose = 3;
    private bool waitCoroutine = false;
    private bool isFirstFound = false;
    private bool isSecondFound = false;
    private Dictionary<int, bool> questionsUsed = new();
    private Dictionary<int, bool> answerList = new();

    private void Start()
    {
        FillDictionaryQuestions();
        SetNewQuestion();
    }

    private void Update()
    {
        if (!isFirstFound || !isSecondFound)
        {
            canvaBloqueio.SetActive(true);
        }
        else
        {
            canvaBloqueio.SetActive(false);
        }
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
        if (GetPlayerPrefValue("PokemonQuizPuzzle") != 1)
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
                        toggleRepostas[i].isOn = false;
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
        if (pokebolasPlayer.Count > 0)
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

        VerifyWinner();
    }

    private void VerifyWinner()
    {
        if (pokebolasBot.Count < countToLose)
        {
            //player win
            SetPlayerPrefValue("PokemonQuizPuzzle", 1);
            ShowDetailsAfterWin();

        }
        else if (pokebolasPlayer.Count < countToLose)
        {
            ShowDetailsAfterLose();
        }
    }

    private void ShowDetailsAfterLose()
    {
        canvaPrincipal.SetActive(false);
        canvaDetails.SetActive(true);
        detailsAfterLose.SetActive(true);
    }

    public void ShowDetailsAfterWin()
    {
        answerGroup.SetActive(false);
        tmpPergunta.text = "Você é um grande guerreiro! Avance para a próxima cena!";
    }

    public void ReloadScene()
    {
        TryLoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextScene()
    {
        if (!TryLoadSceneAsync(2) && !waitCoroutine)
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

    public void OnTargetFound(string target)
    {
        if (target == "poke1")
        {
            isFirstFound = true;
        }
        else if (target == "poke2")
        {
            isSecondFound = true;
        }
    }

    public void OnTargetLost(string target)
    {
        if (target == "poke1")
        {
            isFirstFound = false;
        }
        else if (target == "poke2")
        {
            isSecondFound = false;
        }

    }
}
