using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : Area
{
    [SerializeField] private MeshRenderer blockRenderer;


    public override void StepOn()
    {
        throw new System.NotImplementedException();
    }

    public override void SetBlock()
    {
        Debug.Log(areaName);
        if (blockRenderer)
            blockRenderer.materials[0].SetColor("_Color",color);
    }


}
