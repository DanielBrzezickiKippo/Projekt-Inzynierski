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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBlock(Area area, int action)
    {
        categoryText.text = area.category;
        areaNameText.text = area.areaName;
        hpText.text = $"+{area.damage * 2}";
        areaImage.color = area.color;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            if (action == 0)
                ChooseDoubleDamage(area);
            else
                ChooseTeleport();

        });
    }

    public void ChooseDoubleDamage(Area area)
    {
        area.damage = area.damage * 2;
    }

    public void ChooseTeleport()
    {

    }

}
