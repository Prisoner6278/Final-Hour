using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TreeSorter : MonoBehaviour
{
    void Start()
    {
        Transform foliage = transform.Find("FoliageGraphics");
        Transform trunk = transform.Find("TrunkGraphics");

        int order = 0;
        if (foliage != null)
        {
            order = 1000 - Mathf.RoundToInt(foliage.gameObject.transform.position.y * 10f);
            foliage.GetComponent<SpriteRenderer>().sortingOrder = order;
        }
        if (trunk != null)
        {
            if (order == 0)
                order = 1000 - Mathf.RoundToInt(trunk.gameObject.transform.position.y * 10f);
            trunk.GetComponent<SpriteRenderer>().sortingOrder = order;
        }
    }
}
