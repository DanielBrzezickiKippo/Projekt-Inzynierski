using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public enum ChanceType
{
    giveHp,
    stealHp,
    destoyCity,
    goToPrison
}



public class Chance : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private UIManager uiManager;


    public T RandomEnum<T>(T obj)
    {
        List<T> enums = Enum.GetValues(typeof(T)).Cast<T>().ToList();
        System.Random random = new System.Random();

        return enums[random.Next(0, enums.Count)];
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GiveAChance(ChanceType type)
    {
        uiManager.DeleteSelectables();
        switch (type)
        {
            case ChanceType.giveHp:
                uiManager.SetPopUI("Szansa", "Dostajesz dodatkowe ¿ycie.", GiveHp);
                //GiveHp();
                break;
            case ChanceType.stealHp:
                uiManager.CreateSelectablesPlayerToDealHP(gameManager.GetPlayersWithoutCurrentId(), 1);
                break;
            case ChanceType.destoyCity:
                uiManager.CreateSelectablesAreaToDestroy(gameManager.GetPlayersWithoutCurrentId());
                break;
            case ChanceType.goToPrison:
                uiManager.SetPopUI("Szansa", "Trafiasz do wiêzienia.", GoToPrison);
                //GoToPrison();
                //StartCoroutine(uiManager.End(uiManager.selectUI, 1f, Turn.nextTurn));
                break;
        }
    }

    public void GiveHp()
    {
        Player currentPlayer = gameManager.GetCurrentPlayer();
        currentPlayer.hp++;
        gameManager.SetPlayersUI();
        StartCoroutine(uiManager.SendMessage($"Gracz {gameManager.GetCurrentPlayer().name} zdobywa zycie", 0f));
        gameManager.SetTurn(Turn.nextTurn);
    }

    public void GoToPrison()
    {
        gameManager.ChooseTeleport(gameManager.mapGenerator.mapBlocks[8].GetComponent<Area>());
        StartCoroutine(uiManager.SendMessage($"Gracz {gameManager.GetCurrentPlayer().name} trafia do wiezienia", 0f));

    }
}
