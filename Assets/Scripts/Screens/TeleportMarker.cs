using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TeleportMarker : MonoBehaviour
{
    private void Update()
    {
        transform.localScale = new Vector3(1.0f / transform.parent.localScale.x, 1.0f / transform.parent.localScale.y, 1.0f);
    }
}
