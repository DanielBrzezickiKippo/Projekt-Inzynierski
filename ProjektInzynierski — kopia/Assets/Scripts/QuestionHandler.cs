using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[System.Serializable]
public class Lesson
{
    public string category;
    public string lesson;
}

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
    [SerializeField] private List<Lesson> lessons;

    public Question GetRandomQuestion()
    {
        return questions[UnityEngine.Random.Range(0, questions.Count)];
    }

    public Question GetRandomQuestionByCategory(string category)
    {
        List<Question> questionsByCategory = new List<Question>();
        foreach (Question question in questions)
        {
            if (question.category == category)
                questionsByCategory.Add(question);
        }

        return questionsByCategory[UnityEngine.Random.Range(0, questionsByCategory.Count)];
    }

    public Lesson GetRandomLessonByCategory(string category)
    {
        List<Lesson> lessonsByCategory = new List<Lesson>();
        foreach(Lesson lesson in lessons)
        {
            if (lesson.category == category)
                lessonsByCategory.Add(lesson);
        }

        return lessonsByCategory[UnityEngine.Random.Range(0, lessonsByCategory.Count)];
    }
}
