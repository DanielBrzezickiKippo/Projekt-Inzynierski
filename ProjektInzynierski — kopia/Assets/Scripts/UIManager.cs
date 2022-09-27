using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TextMeshProUGUI playerInfo;

    [Header("Roll UI")]
    [SerializeField] private Button rollButton;

    [Header("Property UI")]
    [SerializeField] private GameObject propertyUI;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private List<Button> answerButtons;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void QuestionPlayer()
    {
        Open(propertyUI);
        //random question
        questionText.text = "question";

        //answer
        for (int i =0;i<answerButtons.Count;i++) {
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "answer";
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => {

                Close(propertyUI);
                gameManager.SetTurn(Turn.nextTurn);
            });
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


    public void SetCurrentPlayer(string player)
    {
        playerInfo.text = player;
    }


}
