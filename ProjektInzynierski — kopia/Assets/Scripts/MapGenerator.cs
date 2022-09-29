using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private List<AreaData> areaData;

    [SerializeField]
    private GameObject singleBlockPref;

    [SerializeField]
    private GameObject squareBlockPref;

    [SerializeField]
    private GameObject squareBoard;


    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [HideInInspector] public List<GameObject> mapBlocks;
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
                break;
        }
        obj.transform.rotation = Quaternion.Euler(new Vector3(0f, rotation, 0f));
        mapBlocks.Add(obj);
    }

    const int mapSize = 9;

    private void CreateBoard()
    {
        GameObject obj = Instantiate(squareBoard);
        obj.transform.position = new Vector3(mapSize / 2f, 0f, mapSize / 2f);
        obj.transform.localScale = new Vector3(mapSize - 2, obj.transform.localScale.y, mapSize - 2);
        obj.transform.localRotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));

        
        if (obj.GetComponent<Area>())
        {
            obj.GetComponent<Area>().SetArea("Codopoly",Color.white);
        }
    }

    private void GenerateMap()
    {
        CreateBoard();
        

        for(int i = 0; i < mapSize-1; i++)
            GenerateBlock(i, 0f, i + 0.5f, 90f);//first row

        for(int i =0;i<mapSize-1;i++)
            GenerateBlock(i, i + 0.5f, mapSize, 0f);//second row

        for (int i = mapSize-1; i > 0; i--)
            GenerateBlock(i, mapSize , i + 0.5f, 90f);//third row

        for (int i = mapSize-1; i > 0; i--)
            GenerateBlock(i, i + 0.5f, 0f, 0f);//fourth row

        SetAreaBlocks();

    }


    private void SetAreaBlocks()
    {
        for(int i =0;i< mapBlocks.Count;i++)
        {
            if(mapBlocks[i].GetComponent<Area>())
                mapBlocks[i].GetComponent<Area>().SetArea(areaData[i],i);
        }
    }

}
