using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Plot : Area
{
    [SerializeField] private MeshRenderer blockRenderer;
    [SerializeField] private TextMeshPro plotArea;
    [SerializeField] private TextMeshPro plotDamage;

    public int ownerId=-1;


    public override void StepOn()
    {
        UIManager uiManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>();

        GameManager gameManager = uiManager.gameManager;


        if (type == AreaType.ToBuy && ownerId != gameManager.GetCurrentPlayerTurnId())
            uiManager.QuestionPlayer(this);
        else if (type == AreaType.ToBuy && ownerId == gameManager.GetCurrentPlayerTurnId())
            uiManager.LearnPlayer(this);
        else if (type == AreaType.Chance)
            uiManager.DoNothing();
        else if (type == AreaType.Start)
            uiManager.DoNothing();
        else if (type == AreaType.Prison)
        {
            if(gameManager.IsPlayerInPrison())
                uiManager.QuestionPlayer(this);
            else
                gameManager.PlayerGoToPrison();
        }

        //else if (type == AreaType.Start)
        //     gameManager.PlayerGoThroughStart();





    }

    public override void SetBlock()
    {
        if (blockRenderer)
            blockRenderer.materials[0].SetColor("_Color",color);

        if(plotArea)
            plotArea.text = areaName;
        
        if(plotDamage)
            plotDamage.text = damage.ToString();
    }


    public bool hasOwner()
    {
        if (ownerId == -1)
            return false;
        return true;
    }

    public void SetOwner(int playerId, Color playerColor)
    {
        ownerId = playerId;
        color = playerColor;
        SetBlock();
    }

}
