using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Area: MonoBehaviour
{
    public string areaName;
    public Color color;
    public int damage;

    public abstract void SetBlock();
    public abstract void StepOn();

    public void SetArea(string areaName, Color color, int damage = 0)
    {
        this.areaName = areaName;
        this.damage = damage;
        this.color = color;
        SetBlock();
    }

}
