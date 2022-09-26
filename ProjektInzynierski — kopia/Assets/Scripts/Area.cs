using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AreaData
{
    public string name;
    public string category;
    public int damage;
    public Color color = Color.white;
    public AreaType type;
}

public enum AreaType
{
    ToBuy,
    KnowledgeCompetition,
    Chance,
    Teleport,
    Prison,
    Start,
    None
}

public abstract class Area: MonoBehaviour
{
    [HideInInspector]public string areaName;
    [HideInInspector] public Color color;
    [HideInInspector] public int damage;
    [HideInInspector] public string category;
    public AreaType type;

    public abstract void SetBlock();
    public abstract void StepOn();

    public void SetArea(string areaName, Color color, string category="-", int damage = 0, AreaType type = AreaType.None)
    {
        this.areaName = areaName;
        this.damage = damage;
        this.color = color;
        this.category = category;
        this.type = type;
        SetBlock();
    }

    public void SetArea(AreaData area)
    {
        this.areaName =area.name;
        this.damage =area.damage;
        this.color = area.color;
        this.category = area.category;
        this.type = area.type;
        SetBlock();
    }

}
