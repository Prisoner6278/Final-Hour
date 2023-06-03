using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateSpotlightPlay : MonoBehaviour
{
    private bool activated = false;
    private float lerpCounter;
    private float alphaCutoff;
    GameObject spotlight;

    private void Start()
    {
        spotlight = transform.Find("Spotlight").gameObject;
        spotlight.SetActive(true);
        lerpCounter = 0.0f;
    }

    public void ActivateSpotlight()
    {
        spotlight.GetComponent<SpriteMask>().enabled = true;
        alphaCutoff = spotlight.GetComponent<SpriteMask>().alphaCutoff;
        spotlight.GetComponent<SpriteMask>().alphaCutoff = 1.1f;
        activated = true;
    }

    private void Update()
    {
        if (activated)
        {
            lerpCounter += Time.deltaTime / 10f;
            spotlight.GetComponent<SpriteMask>().alphaCutoff = Mathf.Lerp(1.1f, alphaCutoff, lerpCounter);
            if (lerpCounter >= 1.0f)
                activated = false;
        }
    }
}
