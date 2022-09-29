using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private QuestionHandler questionHandler;
    [SerializeField] public GameManager gameManager;
    [SerializeField] private TextMeshProUGUI playerInfo;
    [SerializeField] private Camera mainCamera;

    [Header("Notify")]
    [SerializeField] private TextMeshProUGUI notificationText;

    [Header("Roll UI")]
    [SerializeField] public Button rollButton;

    [Header("Property UI")]
    [SerializeField] public GameObject propertyUI;
    [SerializeField] private TextMeshProUGUI categoryText;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private List<Button> answerButtons;
    [SerializeField] private Color defaultColor;

    [Header("Learn UI")]
    [SerializeField] private GameObject learnUI;
    [SerializeField] private TextMeshProUGUI lCategoryText;
    [SerializeField] private TextMeshProUGUI lLearnText;
    [SerializeField] private Button lessonButton;

    [Header("Select UI")]
    [SerializeField] public GameObject selectUI;
    [SerializeField] private GameObject selectableContent;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private GameObject blockUIprefab;    





    // Start is called before the first frame update
    void Start()
    {
        defaultColor = answerButtons[0].image.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void QuestionPlayer(Plot plot)
    {
        Question question = questionHandler.GetRandomQuestionByCategory(plot.category);

        Open(propertyUI);
        question.RandomSortAnswers();

        categoryText.text = question.category;
        questionText.text = question.question;

        AnswerButtons(true);
        //answer
        for (int i =0;i<answerButtons.Count;i++) {
            string answer = question.answers[i];
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = answer;
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => {
                AnswerButtons(false);
                SetCorrectButtons(question.correctAnswer);
                gameManager.CheckPlayerAnswer(question,answer,plot);

                //StartCoroutine(End(propertyUI, 1.5f, Turn.nextTurn));
            });
        }
    }


    public void LearnPlayer(Plot plot)
    {
        Lesson lesson = questionHandler.GetRandomLessonByCategory(plot.category);

        lCategoryText.text = lesson.category;
        lLearnText.text = lesson.lesson;

        Open(learnUI);

        lessonButton.onClick.RemoveAllListeners();
        lessonButton.onClick.AddListener(() =>{
            StartCoroutine(End(learnUI,0f, Turn.nextTurn));
        });
    }


    public void CreateSelectables(List<Area> areaList, int selectableAction)
    {
        DeleteSelectables();
        Open(selectUI);
        if (areaList.Count == 0)
        {
            StartCoroutine(End(selectUI, 0f, Turn.nextTurn));
            return;
        }
        foreach (Area area in areaList)
        {
            BlockUI blockUI = Instantiate(blockUIprefab, selectableContent.transform).GetComponent<BlockUI>();
            blockUI.SetBlock(area, selectableAction);
        }
    }

    void DeleteSelectables()
    {
        foreach(Transform obj in selectableContent.transform)
        {
            Destroy(obj.gameObject);
        }
    }

    public void DoNothing()
    {
        gameManager.SetTurn(Turn.nextTurn);
    }

    public IEnumerator End(GameObject objectToClose,float timeToWait,Turn turn)
    {
        yield return new WaitForSeconds(timeToWait);
        Close(objectToClose);
        gameManager.SetTurn(turn);
    }

    void SetCorrectButtons(string answer)
    {
        foreach(Button answerButton in answerButtons)
        {
            if (answerButton.GetComponentInChildren<TextMeshProUGUI>().text == answer)
                answerButton.image.color = Color.green;
            else
                answerButton.image.color = Color.red;
        }
    }

    void AnswerButtons(bool isInteractable)
    {
        foreach(Button b in answerButtons)
        {
            b.image.color = defaultColor;
            b.interactable = isInteractable;
        }
    }

    void Open(GameObject obj)
    {
        obj.SetActive(true);
    }
    void Close(GameObject obj)
    {
        obj.SetActive(false);
    }


    public void SetCurrentPlayer(Player player)
    {
        playerInfo.text = player.name;// +"\nzycia: "+player.hp;
        mainCamera.backgroundColor = player.playerColor;
    }


    public void RollButton(bool isInteractable, int rolled=0)
    {
        if (isInteractable)
        {
            rollButton.GetComponentInChildren<TextMeshProUGUI>().text = "Losuj";
            rollButton.interactable = true;
        }
        else
        {
            rollButton.GetComponentInChildren<TextMeshProUGUI>().text = "Wylosowano: " + rolled;
            rollButton.interactable = false;
        }
    }


    public IEnumerator SendMessage(string message,float time)
    {
        notificationText.text = message;
        notificationText.gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        notificationText.gameObject.SetActive(false);
    }

}
