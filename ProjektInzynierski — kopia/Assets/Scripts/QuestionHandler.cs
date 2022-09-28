using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


[System.Serializable]
public class Question
{
    public string category;
    public string question;
    public List<string> answers;
    public string correctAnswer;


    public void RandomSortAnswers()
    {
        List<string> newOrder = new List<string>();

        System.Random r = new System.Random();
        var randomized = answers.OrderBy(item => r.Next());

        foreach(string answer in randomized)
        {
            newOrder.Add(answer);
        }

        answers.Clear();
        answers = newOrder;

    }


    public bool isCorrect(string answer)
    {
        if (answer == correctAnswer)
            return true;
        return false;
    }

}


public class QuestionHandler : MonoBehaviour
{

    [SerializeField] private List<Question> questions;

    public Question GetRandomQuestion()
    {
        return questions[UnityEngine.Random.Range(0, questions.Count)];
    }
}
