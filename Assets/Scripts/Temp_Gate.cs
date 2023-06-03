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

            // destroy other beach gates if this is one
            if (itemRequirement == "#beach_key")
            {
                GameObject[] beachGates = GameObject.FindGameObjectsWithTag("BeachGate");
                foreach (GameObject gate in beachGates)
                {
                    if (gate != gameObject)
                        Destroy(gate);
                }
            }
            Destroy(gameObject);
        }
    }
}
