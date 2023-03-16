using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemManager : MonoBehaviour
{
    public TextAsset itemListCSV;
    [SerializeField]
    private Item[] itemList;

    // singleton
    private static ItemManager instance;

    private void Awake()
    {
        // setting up singleton
        if (instance != null && instance != this)
            Destroy(this);
        instance = this;
    }

    public static ItemManager Instance()
    {
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        itemList = CSVReader.ReadItemListCSV(itemListCSV);
    }

    public bool GetItemHolding(string itemName)
    {
        Item i = Array.Find(itemList, x => string.Compare(x.GetName(), itemName) == 0);
        if (i == null)
            throw new Exception("Searched item list for non-existant item! Did you make a typo somewhere?\n");
        return i.GetHolding();
    }

    public void SetItemHolding(string itemName)
    {
        Item i = Array.Find(itemList, x => x.GetName() == itemName);
        if (i == null)
        {
            Debug.Log("searched for " + itemName);
            throw new Exception("Searched item list for non-existant item! Did you make a typo somewhere?\n");
        }

        i.SetHolding(!i.GetHolding()); // if holding, take. if not holding, give
    }
}

[System.Serializable]
public class Item
{
    [SerializeField]
    private string name;
    [SerializeField]
    private bool holding;

    public string GetName() => name;
    public bool GetHolding() => holding;

    public Item(string itemId)
    {
        name = itemId;
        holding = false;
    }

    public void SetHolding(bool holdingStatus)
    {
        holding = holdingStatus;
    }
}
