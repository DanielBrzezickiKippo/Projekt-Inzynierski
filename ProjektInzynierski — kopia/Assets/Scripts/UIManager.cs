using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerInfo;
    [Header("Roll UI")]
    [SerializeField] private Button rollButton;

    [Header("Property UI")]
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

    }

    public void SetCurrentPlayer(string player)
    {
        playerInfo.text = player;
    }


}
