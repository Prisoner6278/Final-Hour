using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveScreenFromCam : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Screen[] screensInScene = Object.FindObjectsOfType<Screen>();

        Screen closestScreen = screensInScene[0];
        float closestScreenDistance = Vector2.Distance(transform.position, closestScreen.transform.position);

        for (int i = 1; i < screensInScene.Length; i++)
        {
            float thisDistance = Vector2.Distance(transform.position, screensInScene[i].transform.position);
            if (thisDistance < closestScreenDistance)
            {
                closestScreen = screensInScene[i];
                closestScreenDistance = thisDistance;
            }
        }

        closestScreen.SetToActiveScreen();
    }
}
