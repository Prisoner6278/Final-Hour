using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryWindow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public AnimationCurve curve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);

    bool pointerHovering;
    RectTransform rect;
    Vector3 currentPos;
    Vector3 showingPos = new Vector3(0, -46.8f, 0); // y axis position of UI
    Vector3 hidingPos = new Vector3(0, 80.7f, 0);

    // lerp stuff
    bool open;
    bool showing;
    bool hiding;
    float lerpProgress;
    float lerpSpeed = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        open = false;
        showing = false;
        hiding = false;
        lerpProgress = 0.0f;
        pointerHovering = false;
        rect = GetComponent<RectTransform>();

        ItemManager.Instance().ItemActionStart.AddListener(ShowInventory);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) || (pointerHovering && Input.GetKeyDown(KeyCode.Mouse0)))
        {
            if (open)
                HideInventory();
            else
                ShowInventory();
        }

        if (showing)
        {
            lerpProgress += Time.deltaTime * lerpSpeed;
            rect.anchoredPosition = Vector3.Slerp(currentPos, showingPos, curve.Evaluate(lerpProgress));
            if (lerpProgress >= 1.0f)
                showing = false;
        }
        else if (hiding)
        {
            lerpProgress += Time.deltaTime * lerpSpeed;
            rect.anchoredPosition = Vector3.Slerp(currentPos, hidingPos, curve.Evaluate(lerpProgress));
            if (lerpProgress >= 1.0f)
                hiding = false;
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        pointerHovering = true;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        pointerHovering = false;
    }

    void ShowInventory()
    {
        ItemManager.Instance().ItemActionEnd.AddListener(HideInventory);
        hiding = false;
        lerpProgress = 0.0f;
        currentPos = rect.anchoredPosition;
        showing = true;
        open = true;
    }

    void HideInventory()
    {
        showing = false;
        lerpProgress = 0.0f;
        currentPos = rect.anchoredPosition;
        hiding = true;
        open = false;
    }
}
