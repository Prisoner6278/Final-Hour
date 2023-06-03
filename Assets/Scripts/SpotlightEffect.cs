using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightEffect : MonoBehaviour
{
    float baseScale = 2.8f;
    float addedScale = 0.0f;
    public bool breatheEffectOn = true;

    // Start is called before the first frame update
    void Start()
    {
        baseScale = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (breatheEffectOn)
        {
            addedScale = Mathf.Sin(Time.time * 2.0f) / (28.0f / baseScale);
            transform.localScale = new Vector3(baseScale + addedScale, baseScale + addedScale, baseScale + addedScale);
        }
    }

    public void ActivateSpriteMask()
    {
        GetComponent<SpriteMask>().enabled = true;
    }
}
