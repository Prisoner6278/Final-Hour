using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScreenBoundary : MonoBehaviour
{
    public enum BoundaryDirection
    {
        Left,
        Right,
        Up,
        Down
    }

    public BoundaryDirection boundaryDirection;
    public GameObject screenToTransitionTo;
    public string soundToPlay;

    Transform teleportLocation;
    Screen thisScreen;

    private void Start()
    {
        teleportLocation = transform.Find("TeleportMarker");
        if (teleportLocation == null)
            throw new Exception("Screen boundary wasn't able to find child TeleportMarker");
    }

    public Vector4 GetTransitionDirection()
    {
        Debug.Log("screen set to inactive");
        thisScreen.SetInactive();
        screenToTransitionTo.GetComponent<Screen>().SetToActiveScreen();

        Vector4 output = Vector2.zero;
        float transitionAngle = 0.0f;

        switch (boundaryDirection)
        {
            case BoundaryDirection.Left:
                {
                    transitionAngle = 90.0f;
                    output = Vector2.left;
                    break;
                }
            case BoundaryDirection.Right:
                {
                    transitionAngle = 270.0f;
                    output = Vector2.right;
                    break;
                }
            case BoundaryDirection.Up:
                {
                    transitionAngle = 0.0f;
                    output = Vector2.up;
                    break;
                }
            case BoundaryDirection.Down:
                {
                    transitionAngle = 180.0f;
                    output = Vector2.down;
                    break;
                }
        }

        output.z = teleportLocation.position.x;
        output.w = teleportLocation.position.y;
        ScreenTransitionEffect.Instance().Transition(transitionAngle, screenToTransitionTo.transform.position);
        if (soundToPlay != "")
            AudioManager.Instance().PlaySound(soundToPlay);
        return output;
    }

    public void AssignScreen(Screen screen)
    {
        thisScreen = screen;
    }
}