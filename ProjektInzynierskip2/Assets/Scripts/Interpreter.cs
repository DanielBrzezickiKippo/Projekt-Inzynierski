using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Interpreter : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    bool possibleToCompile(List<GameObject> blocks)
    {
        int num = 1;   
        for(int i =0;i<blocks.Count;i++)
        {
            Debug.Log(num + ". " + blocks[i].GetComponentInChildren<TextMeshProUGUI>().text);
            num++;
            if (blocks[i].GetComponent<BlockSlots>() != null)
                possibleToCompile(blocks[i].GetComponent<BlockSlots>().blocks);
            
        }
        return false;
    }

    public void interprete()
    {
        if (possibleToCompile(GetComponent<BlockSlots>().blocks))
        {

        }
        /*if (possibleToCompile(code))
        {

        }*/
    }


    void makeAction(string command)
    {
        switch (command)
        {
            case "move":
                break;

        }
    }

}
