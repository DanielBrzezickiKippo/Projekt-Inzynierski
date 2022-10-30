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

    UIManager uiManager;
    GameManager gameManager;
    public override void StepOn()
    {
        uiManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>();

        gameManager = uiManager.gameManager;


        if (type == AreaType.ToBuy && ownerId != gameManager.GetCurrentPlayerTurnId())
            uiManager.QuestionPlayer(this);
        else if (type == AreaType.ToBuy && ownerId == gameManager.GetCurrentPlayerTurnId())
            uiManager.LearnPlayer(this);
        else if (type == AreaType.Chance)
        {
            ChanceType randomChance = ChanceType.destoyCity;
            //Random r = new Random();
            Chance chance = uiManager.GetComponent<Chance>();
            chance.GiveAChance(chance.RandomEnum<ChanceType>(randomChance));//(ChanceType)Random.Range(0,4));
        }
        else if (type == AreaType.Start)
            uiManager.DoNothing();
        else if (type == AreaType.Prison)
        {
            if (gameManager.IsPlayerInPrison())
                uiManager.QuestionPlayer(this);
            else
            {
                StartCoroutine(uiManager.SendMessage($"Gracz {gameManager.GetCurrentPlayer().name} trafia do wiezienia", 0f));
                gameManager.PlayerGoToPrison();
            }
        }
        else if (type == AreaType.KnowledgeCompetition)
            uiManager.CreateSelectables(gameManager.GetCurrentPlayer().properties, 0);
        else if (type == AreaType.Teleport)
            uiManager.CreateSelectables(gameManager.GetPlotsWherePlayerCanTeleport(), 1);

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

        if (type == AreaType.Chance)
        {
            blockRenderer.gameObject.SetActive(false);
            plotDamage.gameObject.SetActive(false);
        }

    }


    public bool hasOwner()
    {
        if (ownerId == -1)
            return false;
        return true;
    }

    public void SetOwner(Player player ,int playerId)
    {
        ownerId = playerId;
        color = player.playerColor;
        player.properties.Add(this);
        SetBlock();
    }

    public void ClearOwner()
    {
        gameManager.RemovePlotFromPlayer(ownerId, this);
        ownerId = -1;
        color = Color.black;
        SetBlock();
    }

}
