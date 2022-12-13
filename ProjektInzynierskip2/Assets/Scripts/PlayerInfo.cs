using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] public TextMeshPro dealHp;
    [SerializeField] public Transform coin;


    public void ShowAnimation(string dealHp,Color color)
    {
        this.dealHp.text = dealHp;
        this.dealHp.color = color;
    }

    public void PlayAnimation()
    {
        this.dealHp.GetComponent<Animator>().Play("dealHpAnim");
    }
}
