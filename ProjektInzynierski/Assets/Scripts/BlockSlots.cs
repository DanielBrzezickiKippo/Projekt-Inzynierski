using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockSlots : MonoBehaviour, IDropHandler
{

    [SerializeField] public List<GameObject> blocks;
    [SerializeField] private float _height;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData != null)
        {
            eventData.pointerDrag.GetComponent<DragDrop>().parentBlock = gameObject;
            eventData.pointerDrag.gameObject.transform.parent = gameObject.transform;
            blocks.Add(eventData.pointerDrag.gameObject);

            //eventData.pointerDrag.GetComponent<RectTransform>().localPosition = NextBlockPos();
            SetBlockPositions();
            //Debug.Log(eventData.pointerDrag.GetComponent<RectTransform>().localPosition);
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
