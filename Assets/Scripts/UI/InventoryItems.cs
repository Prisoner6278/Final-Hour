using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InventoryItems : MonoBehaviour
{
    public GameObject itemImagePrefab;
    public ItemDisplay[] preloadedItemGraphics;

    // every 120 units on x
    private List<GameObject> itemImages;
    private float firstValueX = -224.1f;
    private float firstValueY = -16.1f;
    private float itemSpacingDistance = 80.5f;

    // Start is called before the first frame update
    void Start()
    {
        itemImages = new List<GameObject>();
    }

    public void AddItem(string itemName)
    {
        GameObject newItem = Instantiate(itemImagePrefab, Vector3.zero, Quaternion.identity, transform);
        newItem.GetComponent<ItemImageLabel>().name = itemName;
        newItem.GetComponent<RectTransform>().anchoredPosition = new Vector2(firstValueX + itemImages.Count * itemSpacingDistance, firstValueY);
        newItem.GetComponentInChildren<Image>().sprite = Array.Find(preloadedItemGraphics, x => string.Compare(x.GetName(), itemName) == 0).GetSprite();
        itemImages.Add(newItem);
    }

    public void RemoveItem(string itemName)
    {
        GameObject itemToRemove = null;
        bool beforeItem = true;
        foreach (GameObject item in itemImages)
        {
            if (beforeItem && item.GetComponent<ItemImageLabel>().name == itemName)
            {
                itemToRemove = item;
                beforeItem = false;
            }
            else if (!beforeItem)
            {
                item.GetComponent<RectTransform>().anchoredPosition = new Vector2(item.GetComponent<RectTransform>().anchoredPosition.x - itemSpacingDistance, item.GetComponent<RectTransform>().anchoredPosition.y);
            }
        }
        itemImages.Remove(itemToRemove);
        itemToRemove.GetComponent<ItemImageLabel>().Destruct();
    }
}

[System.Serializable]
public class ItemDisplay
{
    [SerializeField]
    private string name;
    [SerializeField]
    private Sprite sprite;

    public string GetName()
    {
        return name;
    }

    public Sprite GetSprite()
    {
        return sprite;
    }
}

