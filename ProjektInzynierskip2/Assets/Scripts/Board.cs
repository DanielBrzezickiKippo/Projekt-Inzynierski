using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Board : Area
{
    [SerializeField] private TextMeshPro boardArea;

    public override void SetBlock()
    {
        if(boardArea)
            boardArea.text =areaName;
    }

    public override void StepOn()
    {
        UIManager uiManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>();

        //GameManager gameManager = uiManager.gameManager;

        if (type == AreaType.Start)
            uiManager.DoNothing();
        //else if (type == AreaType.Prison)
       //     ques

        //throw new System.NotImplementedException();
    }

}
