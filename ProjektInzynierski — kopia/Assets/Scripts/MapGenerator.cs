using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject singleBlockPref;

    [SerializeField]
    private GameObject squareBlockPref;

    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [SerializeField] List<GameObject> mapBlocks;
    void GenerateBlock(int iter,float x,float z,float rotation)
    {

        GameObject obj;

        switch (iter)
        {
            case 0:
                obj = Instantiate(squareBlockPref);
                obj.transform.position = new Vector3((int)x, 0f, (int)z);
                break;
            case mapSize-1:
                obj = Instantiate(squareBlockPref);
                obj.transform.position = new Vector3(Mathf.Ceil(x), 0f, Mathf.Ceil(z));
                break;
            default:
                obj = Instantiate(singleBlockPref);
                obj.transform.position = new Vector3(x, 0f, z);
                obj.transform.rotation = Quaternion.Euler(new Vector3(0f, rotation, 0f));
                break;
        }
        mapBlocks.Add(obj);
    }

    const int mapSize = 9;
    void GenerateMap()
    {
        for(int i = 0; i < mapSize-1; i++)
            GenerateBlock(i, 0f, i + 0.5f, 90f);//first row

        for(int i =0;i<mapSize-1;i++)
            GenerateBlock(i, i + 0.5f, mapSize, 180f);//second row

        for (int i = mapSize-1; i > 0; i--)
            GenerateBlock(i, mapSize , i + 0.5f, 270f);//third row

        for (int i = mapSize-1; i > 0; i--)
            GenerateBlock(i, i + 0.5f, 0f, 0f);//fourth row

    }
}
