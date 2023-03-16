using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Screen : MonoBehaviour
{
    ScreenAudio screenAudio;
    ScreenSpriteSort spriteSort;
    public bool startScreen;

    // Start is called before the first frame update
    void Start()
    {
        screenAudio = GetComponent<ScreenAudio>();
        spriteSort = GetComponent<ScreenSpriteSort>();

        // set up references to screen audio + sprite sort in screen boundaries
        ScreenBoundary[] boundaries = GetComponentsInChildren<ScreenBoundary>();
        foreach (ScreenBoundary boundary in boundaries)
        {
            boundary.AssignScreen(this);

            // disable boundary + marker graphics and script
            GameObject teleportLocation = boundary.transform.Find("TeleportMarker").gameObject;
            if (teleportLocation == null)
                throw new Exception("Screen boundary wasn't able to find child TeleportMarker");
            boundary.GetComponent<SpriteRenderer>().enabled = false;
            teleportLocation.GetComponent<SpriteRenderer>().enabled = false;
            teleportLocation.GetComponent<TeleportMarker>().enabled = false;
        }

        if (startScreen)
            SetToActiveScreen();
    }

    public void SetToActiveScreen()
    {
        screenAudio.SetToActiveScreen();
        spriteSort.SetToActiveScreen();
    }

    public void SetInactive()
    {
        spriteSort.SetToInactive();
    }
}
