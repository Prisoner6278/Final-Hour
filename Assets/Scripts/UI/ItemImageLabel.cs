using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemImageLabel : MonoBehaviour
{
    [HideInInspector]
    public string itemName;

    public void Destruct()
    {
        StartCoroutine(DelayedDestroy());
    }

    IEnumerator DelayedDestroy()
    {
        GetComponent<Animator>().SetBool("remove", true);
        yield return new WaitForSeconds(0.5f);
        Destroy(this);
    }
}
