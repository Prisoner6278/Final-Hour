using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItemCheck : MonoBehaviour
{
    public string itemName;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (ItemManager.Instance().GetItemHolding(itemName))
            {
                ItemManager.Instance().SetItemHolding(itemName);
                Destroy(gameObject);
            }
        }
    }
}
