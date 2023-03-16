using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightEffect : MonoBehaviour
{
    float baseScale = 2.8f;
    float addedScale = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        addedScale = Mathf.Sin(Time.time * 2.0f) / 10.0f;
        transform.localScale = new Vector3(baseScale + addedScale, baseScale + addedScale, baseScale + addedScale);
    }
}
