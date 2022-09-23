using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockSlots : MonoBehaviour, IDropHandler
{

    [SerializeField] public List<GameObject> blocks;
    [SerializeField] private float _height;



    bool CanBeDropped(DragDrop dropped)
    {
        DragDrop previousDrop=null;
        if (blocks.Count > 0)
            previousDrop = blocks[blocks.Count - 1].GetComponent<DragDrop>();
        switch (dropped.blockType)
        {
            case BlockType.elseIfStatement:
                if (previousDrop == null || previousDrop.blockType != BlockType.ifStatement || previousDrop.blockType != BlockType.elseIfStatement)
                    return false;
                break;
            case BlockType.elseStatement:
                if (previousDrop == null||previousDrop.blockType != BlockType.ifStatement)
                    return false;

                break;
        }
        return true;
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData != null)
        {
            DragDrop dropped = eventData.pointerDrag.GetComponent<DragDrop>();


            if (CanBeDropped(dropped))
            {
                dropped.parentBlock = gameObject;
                dropped.gameObject.transform.parent = gameObject.transform;
                blocks.Add(dropped.gameObject);

                //eventData.pointerDrag.GetComponent<RectTransform>().localPosition = NextBlockPos();
                SetBlockPositions();
                //Debug.Log(eventData.pointerDrag.GetComponent<RectTransform>().localPosition);
            }
        }
    }


    public void DeleteBlock(GameObject block)
    {
        blocks.Remove(block);
        SetBlockPositions();
    }

    public void SetBlockPositions()
    {
        float height = 0;//gameObject.GetComponent<RectTransform>().sizeDelta.y;
        for (int i =0;i<blocks.Count;i++)
        {
            GameObject block = blocks[i];


            RectTransform _blockRt = block.GetComponent<RectTransform>();
            height += _blockRt.sizeDelta.y;

            _blockRt.localPosition = new Vector2(50f, -height);


            BlockSlots blockSlots = block.GetComponent<BlockSlots>();
            if (blockSlots)
            {
                height += blockSlots._height;
            }
        }
        _height = height;

        BlockSlots parent = gameObject.transform.parent.GetComponent<BlockSlots>();
        if (parent)
        {
            parent.SetBlockPositions();
        }
    }

}
