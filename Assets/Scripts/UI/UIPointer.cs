using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPointer : MonoBehaviour
{
    private Vector2 basePos;

    void Awake()
    {
        basePos = GetComponent<RectTransform>().anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector2(basePos.x + Mathf.Cos(Time.time * 6.0f) * 3f, basePos.y);
    }

    public void SetBasePos(Vector2 newPos)
    {
        basePos = newPos;
    }
}
