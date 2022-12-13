using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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
    [SerializeField] private TextMeshProUGUI lExplainQuestionText;
    [SerializeField] private TextMeshProUGUI lExplainLearnText;
    [SerializeField] private TextMeshProUGUI lText;
    [SerializeField] private Button lessonButton;

    [Header("Select UI")]
    [SerializeField] public GameObject selectUI;
    [SerializeField] private GameObject selectableContent;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private GameObject blockUIprefab;


    [Header("PopUp")]
    [SerializeField] public GameObject popUI;
    [SerializeField] private TextMeshProUGUI titlePopText;
    [SerializeField] private TextMeshProUGUI descriptionPopText;
    [SerializeField] private Button buttonPop;

    [Header("Animation text")]
    [SerializeField] private TextMeshProUGUI endTurnText;
    [SerializeField] private List<string> rewardedTexts;
    [SerializeField] private List<string> motivationalTexts;
    [SerializeField] private Color correctColor;
    [SerializeField] private Color incorrectColor;


    
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
        Question question = questionHandler.GetRandomQuestionByCategory(plot.category.ToLower());

        Open(propertyUI);
        question.RandomSortAnswers();

        categoryText.text = plot.areaName;
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
        Lesson lesson = questionHandler.GetRandomLessonByCategory(plot.category.ToLower());

        lCategoryText.text = plot.areaName;
        lLearnText.gameObject.SetActive(true);
        lExplainLearnText.gameObject.SetActive(false);
        lExplainQuestionText.gameObject.SetActive(false);
        lLearnText.text = lesson.lesson;

        lLearnText.ForceMeshUpdate();
        Bounds bounds = lLearnText.bounds;

        Open(learnUI);

        lessonButton.onClick.RemoveAllListeners();
        lessonButton.onClick.AddListener(() =>{
            StartCoroutine(End(learnUI,0f, Turn.nextTurn));
        });
    }

    public IEnumerator ExplainToPlayer(Plot plot, string explanation, string question, float time)
    {

        ShowAnimation(true);
        yield return new WaitForSeconds(time);
        Close(propertyUI);

        lCategoryText.text = plot.areaName;
        lLearnText.gameObject.SetActive(false);
        lExplainLearnText.gameObject.SetActive(true);
        lExplainQuestionText.gameObject.SetActive(true);
        lExplainLearnText.text = explanation;
        lExplainQuestionText.text = question;

        Open(learnUI);

        lessonButton.onClick.RemoveAllListeners();
        lessonButton.onClick.AddListener(() => {
            StartCoroutine(End(learnUI, 0f, Turn.nextTurn));
            gameManager.ShowPlayerInfoAnimations();
        });
    }

    public void CreateSelectables(List<Area> areaList, int selectableAction)
    {
        DeleteSelectables();
        Open(selectUI);


        titleText.text = "Szansa";
        if (selectableAction == 0)
            descriptionText.text = "Mianuj swój dzia³ królow¹ nauk.";
        else
        {
            titleText.text = "Teleportacja";
            descriptionText.text = "Wybierz dzia³ do którego chcesz sie przenieœæ.";
        }

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

    public void CreateSelectablesPlayerToDealHP(List<Player> players, int amount)
    {
        DeleteSelectables();
        titleText.text = "Szansa";
        descriptionText.text = "Wybierz przeciwnika, któremu odbierzesz 1 ¿ycie.";
        if (players.Count == 0)
        {
            StartCoroutine(End(selectUI, 0f, Turn.nextTurn));
            return;
        }
        foreach (Player player in players)
        {
            BlockUI blockUI = Instantiate(blockUIprefab, selectableContent.transform).GetComponent<BlockUI>();
            blockUI.SetBlockDealHp(player,amount);

        }
        Open(selectUI);
    }

    public void CreateSelectablesAreaToDestroy(List<Player> players)
    {
        DeleteSelectables();

        titleText.text = "Szansa";
        descriptionText.text = "Wyczyœæ dzia³ przeciwnika.";

        int allProperties = 0;
        foreach (Player p in players) {
            allProperties += p.properties.Count;
            foreach (Area area in p.properties)
            {
                BlockUI blockUI = Instantiate(blockUIprefab, selectableContent.transform).GetComponent<BlockUI>();
                blockUI.SetBlock(area, 2);
            }
        }
        if (allProperties == 0)
        {
            ChanceType randomChance = ChanceType.destoyCity;
            //Random r = new Random();
            Chance chance = GetComponent<Chance>();
            chance.GiveAChance(chance.RandomEnum<ChanceType>(randomChance));//(ChanceType)Random.Range(0,4));}
        }
        else
            Open(selectUI);
        //StartCoroutine(End(selectUI, 0f, Turn.nextTurn));
    }


    public void DeleteSelectables()
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

    public enum TextOnEndTurn
    {
        motivational,
        rewarded,
        none
    }

    public void ShowAnimation(bool isMotivational)
    {
        float pos = 250f;
        float rot = 35f;

        endTurnText.transform.localPosition = new Vector2(UnityEngine.Random.Range(-pos, pos), UnityEngine.Random.Range(-pos, pos));

        endTurnText.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, UnityEngine.Random.Range(-rot, rot)));
        string text;
        if (isMotivational)
        {
            text = motivationalTexts[UnityEngine.Random.Range(0, motivationalTexts.Count)];
            endTurnText.color = incorrectColor;
        }
        else
        {
            text = rewardedTexts[UnityEngine.Random.Range(0, rewardedTexts.Count)];
            endTurnText.color = correctColor;
        }

        endTurnText.text = text;
        endTurnText.GetComponent<Animator>().Play("EndTurnTextAnimation");
    }

    public IEnumerator End(GameObject objectToClose,float timeToWait,Turn turn,TextOnEndTurn textOnEndTurn = TextOnEndTurn.none)
    {
        switch (textOnEndTurn)
        {
            case TextOnEndTurn.motivational:
                ShowAnimation(true);
                break;
            case TextOnEndTurn.rewarded:
                ShowAnimation(false);
                break;
            case TextOnEndTurn.none:
                break;
        }


        yield return new WaitForSeconds(timeToWait);
        Close(objectToClose);
        gameManager.SetTurn(turn);
    }
    public delegate void toDoAfterClickPopUI();
    public void SetPopUI(string title, string description,toDoAfterClickPopUI functon )
    {
        titlePopText.text = title;
        descriptionPopText.text = description;
        Open(popUI);
        buttonPop.onClick.RemoveAllListeners();
        buttonPop.onClick.AddListener(() =>
        {
            functon();
            Close(popUI);
        });
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
        playerInfo.text = player.name + " <color=green>" + player.rightAnswers + "</color>" + " <color=red>" + player.wrongAnswers + "</color> ";//; +"\nzycia: "+player.hp;
        
        if(player.wrongAnswers+player.rightAnswers>0)
            playerInfo.text += Math.Round((((float)player.rightAnswers / ((float)player.rightAnswers + (float)player.wrongAnswers)) * 100f),2) + "%";

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
