using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSpriteSort : MonoBehaviour
{
    public SpriteRenderer[] nonTreeSprites;
    SpriteRenderer[] treeTrunkSprites;

    GameObject player;
    private bool activeScreen;

    // Start is called before the first frame update
    void Start()
    {
        // get prop sprites
        List<GameObject> spriteSortProps = new List<GameObject>();

        foreach (Transform t in transform)
        {
            if (t.tag == "SpriteSortProp")
                spriteSortProps.Add(t.gameObject);
        }

        nonTreeSprites = new SpriteRenderer[spriteSortProps.Count];
        for (int i = 0; i < spriteSortProps.Count; i++)
            nonTreeSprites[i] = spriteSortProps[i].GetComponent<SpriteRenderer>();
        // get tree trunk sprites
        GameObject[] spriteSortTreeTrunks = GameObject.FindGameObjectsWithTag("SpriteSortTreeTrunk");
        treeTrunkSprites = new SpriteRenderer[spriteSortTreeTrunks.Length];
        for (int i = 0; i < treeTrunkSprites.Length; i++)
            treeTrunkSprites[i] = spriteSortTreeTrunks[i].GetComponent<SpriteRenderer>();

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
            foreach (SpriteRenderer s in nonTreeSprites)
            {
                // behind player
                if (s.transform.parent.position.y > player.transform.position.y && s.sortingLayerName != "EnvironmentBack")
                    s.sortingLayerName = "EnvironmentBack";
                // in front
                else if (s.transform.parent.position.y <= player.transform.position.y && s.sortingLayerName != "EnvironmentFront")
                    s.sortingLayerName = "EnvironmentFront";
            }
            // trees
            foreach (SpriteRenderer s in treeTrunkSprites)
            {
                // behind player
                if (s.gameObject.transform.position.y > player.transform.position.y && s.sortingLayerName != "EnvironmentBack")
                {
                    foreach (SpriteRenderer sprite in s.transform.parent.GetComponentsInChildren<SpriteRenderer>())
                        sprite.sortingLayerName = "EnvironmentBack";
                }
                // in front
                else if (s.gameObject.transform.position.y <= player.transform.position.y && s.sortingLayerName != "EnvironmentFront")
                {
                    foreach (SpriteRenderer sprite in s.transform.parent.GetComponentsInChildren<SpriteRenderer>())
                        sprite.sortingLayerName = "EnvironmentFront";
                }
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
