using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEditor;
using System.Text;
using SimpleJSON;
using System.IO;
using MiniJSON;
[System.Serializable]
public class Lessons
{
    public Lesson[] lessons { get; set; }
}

[System.Serializable]
public class Lesson
{
    public string category { get; set; }
    public string lesson { get; set; }
}

[System.Serializable]
public class Questions
{
    public List<Question> questions { get; set; }
}

[System.Serializable]
public class Question
{
    public string category;
    public string question;
    public List<string> answers;
    public string correctAnswer;

    public Question()
    {

    }

    public Question(string category,string question,List<string> answers,string correctAnswer)
    {
        this.category = category;
        this.question = question;
        this.answers = answers;
        this.correctAnswer = correctAnswer;
    }

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
    [SerializeField] public List<Question> questions;
    [SerializeField] public List<Lesson> lessons;


    public void Awake()
    {
      //  this.questions = new List<Question>();
        TextAsset questionsData= (TextAsset)AssetDatabase.LoadAssetAtPath("Assets/Resources/data.json",typeof(TextAsset));


        IDictionary search = (IDictionary)Json.Deserialize(questionsData.text);

        GetQuestionsFrom("r", search);
        GetQuestionsFrom("python", search);
        GetQuestionsFrom("c", search);
        GetQuestionsFrom("cpp", search);
        GetQuestionsFrom("java", search);
        GetQuestionsFrom("javascript", search);
        GetQuestionsFrom("php", search);
        GetQuestionsFrom("csharp", search);
        GetQuestionsFrom("algorytmy", search);
        GetQuestionsFrom("wiezienie", search);
    }

    public void GetQuestionsFrom(string name, IDictionary search)
    {
        IList quest = (IList)search[name];

        foreach (IDictionary questionData in quest)
        {
            List<string> answers = new List<string>();
            foreach (var answer in (List<object>)questionData["answers"])
            {
                answers.Add(answer.ToString());
            }

            Question question = new Question(
                questionData["category"].ToString(),
                questionData["question"].ToString(),
                answers,
                answers[(int)(long)questionData["correctAnswer"]]
                );

            this.questions.Add(question);
        }
    }


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
