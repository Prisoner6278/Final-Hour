using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp_Gate : MonoBehaviour
{
    public string itemRequirement;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && ItemManager.Instance().GetItemHolding(itemRequirement))
        {
            ItemManager.Instance().SetItemHolding(itemRequirement);
            Destroy(gameObject);
        }
    }
}
