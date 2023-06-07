using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMenuHover : MonoBehaviour, IPointerEnterHandler
{
    public GameObject uiPointer;
    public float pointerPosX;
    public void OnPointerEnter(PointerEventData eventData)
    {
        uiPointer.GetComponent<UIPointer>().SetBasePos(new Vector2(pointerPosX, GetComponent<RectTransform>().anchoredPosition.y));
    }
}
