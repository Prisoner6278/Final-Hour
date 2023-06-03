using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectOutlineControl : MonoBehaviour
{
    public Material outlineMat;
    public Material defaultMat;

    public SpriteRenderer sprite;

    public void SetToOutline()
    {
        sprite.material = outlineMat;
    }

    public void SetToDefault()
    {
        sprite.material = defaultMat;
    }
}
