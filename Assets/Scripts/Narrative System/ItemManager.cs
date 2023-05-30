using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ItemManager : MonoBehaviour
{
    public TextAsset itemListCSV;
    [SerializeField]
    private Item[] itemList;
    private InventoryItems inventoryUI;
    private GameObject clockCanvas;

    public UnityEvent ItemActionStart;
    public UnityEvent ItemActionEnd;

    // singleton
    private static ItemManager instance;

    public static ItemManager Instance()
    {
        return instance;
    }

    private void Awake()
    {
        // setting up singleton
        if (instance != null && instance != this)
            Destroy(this);
        instance = this;

        inventoryUI = GameObject.Find("InventoryCanvas").transform.Find("Items").GetComponent<InventoryItems>();
        if (inventoryUI == null)
            throw new Exception("Searched for Inventory Canvas Items child and couldn't find it in the scene.\n");
    }

    private void Start()
    {
        // has to happen after time text setup in timemanager
        clockCanvas = GameObject.Find("ClockCanvas");
        if (clockCanvas == null)
            throw new Exception("couldn't find ClockCanvas, is it added to the scene?");

        if (SceneManager.GetActiveScene().name != "Day2Scene")
            clockCanvas.SetActive(false); // turn off clock UI until player picks up watch

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
        bool taking = !i.GetHolding();
        i.SetHolding(taking); // if holding, take. if not holding, give

        StartCoroutine(InventoryUpdate(taking, itemName));
    }

    // update inventory UI
    private IEnumerator InventoryUpdate(bool taking, string itemName)
    {
        if (itemName == "#watch")
        {
            clockCanvas.SetActive(true);
            yield break;
        }

        // open inventory UI
        ItemActionEnd.RemoveAllListeners(); // reset
        ItemActionStart.Invoke();

        yield return new WaitForSeconds(0.4f);

        // add or remove item
        if (taking)
            inventoryUI.AddItem(itemName);
        else
            inventoryUI.RemoveItem(itemName);

        yield return new WaitForSeconds(0.7f);



        ItemActionEnd.Invoke();
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
