using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSpriteSort : MonoBehaviour
{
    public SpriteRenderer[] sprites;

    GameObject player;
    private bool activeScreen;

    // Start is called before the first frame update
    void Start()
    {
        // get prop sprites
        List<GameObject> spriteSortProps = new List<GameObject>();

        Transform objects = transform.Find("Objects");
        foreach (Transform t in objects)
        {
            foreach (Transform t2 in t)
            {
                if (t2.tag == "SpriteSortProp")
                    spriteSortProps.Add(t2.gameObject);
            }
        }

        sprites = new SpriteRenderer[spriteSortProps.Count];
        for (int i = 0; i < spriteSortProps.Count; i++)
            sprites[i] = spriteSortProps[i].GetComponent<SpriteRenderer>();

        // get player
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            throw new Exception(gameObject.name + " screen sprite sort couldn't find player! " +
                                    "Is the player tagged with the Player tag?");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // each sprite for a screen every frame.. could be more efficient?
        if (activeScreen)
        {
            // props
            foreach (SpriteRenderer s in sprites)
            {
                // behind player
                if (s.transform.parent.position.y > player.transform.position.y && s.sortingLayerName != "EnvironmentBack")
                    s.sortingLayerName = "EnvironmentBack";
                // in front
                else if (s.transform.parent.position.y <= player.transform.position.y && s.sortingLayerName != "EnvironmentFront")
                    s.sortingLayerName = "EnvironmentFront";
            }
        }
    }

    public void SetToActiveScreen()
    {
        activeScreen = true;
    }

    public void SetToInactive()
    {
        activeScreen = false;
    }
}
