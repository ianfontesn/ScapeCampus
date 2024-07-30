using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Questions : MonoBehaviour
{

    [SerializeField] private Question[] questions = new Question[5];


   [Serializable]
   public class Question
    {
        [TextArea]
        public string question;
        public string[] answers = new string[4];
        public int arrayIndexCorrectAnswer;
    }


    public Question[] GetAllQuestions()
    {
        return questions;
    }

    public string GetQuestion(int index)
    {
        return questions[index].question;
    }

    public string[] GetAnswer(int index)
    {
        return questions[index].answers;
    }

    public int GetCurrentCorrectAnswer(int index)
    {
        return questions[index].arrayIndexCorrectAnswer;
    }


}
