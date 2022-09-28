using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private QuestionHandler questionHandler;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TextMeshProUGUI playerInfo;
    [SerializeField] private Camera mainCamera;

    [Header("Roll UI")]
    [SerializeField] private Button rollButton;

    [Header("Property UI")]
    [SerializeField] private GameObject propertyUI;
    [SerializeField] private TextMeshProUGUI categoryText;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private List<Button> answerButtons;
    [SerializeField] private Color defaultColor;


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
        Question question = questionHandler.GetRandomQuestion();

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
                StartCoroutine(EndQuestioning());
            });
        }
    }

    IEnumerator EndQuestioning()
    {
        yield return new WaitForSeconds(1.5f);
        Close(propertyUI);
        gameManager.SetTurn(Turn.nextTurn);
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

}
