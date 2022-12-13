using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
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
    public string category;
    public string lesson;

    public Lesson(string category, string lesson)
    {
        this.category = category;
        this.lesson = lesson;
    }
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
    public int difficulty;
    public string explanation;


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

    public Question(string category, string question, List<string> answers, string correctAnswer, int difficulty, string explanation)
    {
        this.category = category;
        this.question = question;
        this.answers = answers;
        this.correctAnswer = correctAnswer;
        this.difficulty = difficulty;
        this.explanation=explanation;
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

    public int difficulty = 0;

    public void Awake()
    {
        difficulty = PlayerPrefs.GetInt("difficulty", 0);

        TextAsset questionsData = Resources.Load<TextAsset>("questionsData");
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


        TextAsset lessonsData = Resources.Load<TextAsset>("lessonsData"); //(TextAsset)AssetDatabase.LoadAssetAtPath("Assets/Resources/lessonsdata.json", typeof(TextAsset));
        IDictionary lessonsSearch = (IDictionary)Json.Deserialize(lessonsData.text);


        GetLessonsFrom("r", lessonsSearch);
        GetLessonsFrom("python", lessonsSearch);
        GetLessonsFrom("c", lessonsSearch);
        GetLessonsFrom("cpp", lessonsSearch);
        GetLessonsFrom("java", lessonsSearch);
        GetLessonsFrom("javascript", lessonsSearch);
        GetLessonsFrom("php", lessonsSearch);
        GetLessonsFrom("csharp", lessonsSearch);
        GetLessonsFrom("algorytmy", lessonsSearch);
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

            Question question;
            if (name == "wiezienie")
            {
                question = new Question(
                    questionData["category"].ToString(),
                    questionData["question"].ToString(),
                    answers,
                    answers[(int)(long)questionData["correctAnswer"]]
                    );
            }
            else
            {
                question = new Question(
                    questionData["category"].ToString(),
                    questionData["question"].ToString(),
                    answers,
                    answers[(int)(long)questionData["correctAnswer"]],
                    (int)(long)questionData["difficulty"],
                    questionData["explanation"].ToString()
                    );
            }

            this.questions.Add(question);
        }
    }


    public void GetLessonsFrom(string name, IDictionary search)
    {
        IList less = (IList)search[name];

        foreach (IDictionary lessonData in less)
        {
            Lesson lesson = new Lesson(
                lessonData["category"].ToString(),
                lessonData["lesson"].ToString()
                );

            this.lessons.Add(lesson);
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
            if (question.category == category&&question.difficulty==difficulty)
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
