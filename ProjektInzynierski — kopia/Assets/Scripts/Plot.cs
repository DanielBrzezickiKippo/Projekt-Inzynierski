using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Plot : Area
{
    [SerializeField] private MeshRenderer blockRenderer;
    [SerializeField] private TextMeshPro plotArea;
    [SerializeField] private TextMeshPro plotDamage;


    public override void StepOn()
    {
        throw new System.NotImplementedException();
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


}
