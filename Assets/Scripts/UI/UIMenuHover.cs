using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMenuHover : MonoBehaviour, IPointerEnterHandler
{
    public GameObject uiPointer;
    public float pointerOffsetX;
    public void OnPointerEnter(PointerEventData eventData)
    {
        uiPointer.GetComponent<UIPointer>().SetBasePos(new Vector2(GetComponent<RectTransform>().anchoredPosition.x + pointerOffsetX, GetComponent<RectTransform>().anchoredPosition.y));
    }
}
