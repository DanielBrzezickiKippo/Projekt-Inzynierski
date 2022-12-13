using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BlockUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI categoryText;
    [SerializeField] private TextMeshProUGUI areaNameText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private Image areaImage;
    [SerializeField] private Button button;
    [SerializeField] private Image hpImage;

    UIManager uiManager;
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        uiManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>();
        gameManager = uiManager.gameManager;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBlock(Area area, int action)
    {

        if(action==0)
            SetUI(area.category, area.areaName, $"+{area.damage * 2}", area.color, true);
        else
            SetUI(area.category, area.areaName, $"+{area.damage * 2}", area.color, false);

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            if (action == 0)
                gameManager.ChooseDoubleDamage(area);
            else if (action == 1)
                gameManager.ChooseTeleport(area);
            else if (action == 2)
            {
                area.GetComponent<Plot>().ClearOwner();
                //gameManager.RemovePlotFromPlayer(area.GetComponent<Plot>().ownerId, area);
                StartCoroutine(uiManager.End(uiManager.selectUI, 0f, Turn.nextTurn));
            }
        });
    }

    public void SetBlockDealHp(Player player, int amount)
    {
        SetUI("Gracz",player.name,"-1",player.playerColor,true);

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            gameManager.DealDamage(player, amount);
            StartCoroutine(uiManager.End(uiManager.selectUI, 0f, Turn.nextTurn));
        });
    }

    void SetUI(string cat, string name, string amount,  Color color, bool isActive)
    {
        if (cat.Equals("algorytmy"))
            categoryText.text = "nauka";
        else
            categoryText.text = "jêzyk";
        areaNameText.text = name;
        hpText.text = amount;
        areaImage.color = color==new Color(0f,0f,0f,0f)?new Color(0f,0f,0f,1f):color;

        //categoryText.gameObject.SetActive(isActive);
        //areaNameText.gameObject.SetActive(isActive);
        hpText.gameObject.SetActive(isActive);
    }

}
