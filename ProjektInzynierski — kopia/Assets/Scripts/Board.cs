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
        throw new System.NotImplementedException();
    }

}
