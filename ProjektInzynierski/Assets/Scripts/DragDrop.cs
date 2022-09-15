using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IDragHandler,IBeginDragHandler,IEndDragHandler
{
    public GameObject parentBlock;
    [SerializeField] private Canvas _canvas;

    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;

    public void HandleBlock()
    {
        if (parentBlock != null)
        {
            parentBlock.GetComponent<BlockSlots>().DeleteBlock(gameObject);
            gameObject.transform.parent = _canvas.transform;
            parentBlock = null;        
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _canvasGroup.alpha = 0.6f;
        _canvasGroup.blocksRaycasts=false;

        HandleBlock();
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta/_canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
