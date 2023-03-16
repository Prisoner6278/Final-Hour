using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TreeSorter : MonoBehaviour
{
    void Start()
    {
        SpriteRenderer foliage = transform.Find("FoliageGraphics").GetComponent<SpriteRenderer>();
        SpriteRenderer trunk = transform.Find("TrunkGraphics").GetComponent<SpriteRenderer>();

        foliage.sortingOrder = 1000 - Mathf.RoundToInt(foliage.gameObject.transform.position.y * 10f);
        trunk.sortingOrder = 1;
    }
}
