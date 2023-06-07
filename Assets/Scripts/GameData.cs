using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    private GameData instance;

    // GLOBAL GAME DATA HERE
    public float screenSpacingDistance = 3f; // distance player auto moves between screens during screen transition

    public static GameData Instance { get; private set; }

    // check for existing singleton instance
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }
}
