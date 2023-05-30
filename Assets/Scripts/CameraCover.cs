using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCover : MonoBehaviour
{
    public Color dayColor;
    public Color sunsetColor;
    public Color nightColor;

    private Color nextColor;
    private Color currentColor;
    private SpriteRenderer sprite;
    float lerpProgress;
    bool lerping;

    // Start is called before the first frame update
    void Start()
    {
        lerping = false;
        lerpProgress = 0f;
        sprite = GetComponent<SpriteRenderer>();
        currentColor = Color.clear;
    }

    // Update is called once per frame
    void Update()
    {
        if (lerping)
        {
            lerpProgress += Time.deltaTime / 10.0f;
            sprite.color = Color.Lerp(currentColor, nextColor, lerpProgress);
            if (lerpProgress >= 1.0f)
            {
                currentColor = nextColor;
                lerping = false;
            }
        }
    }

    public void UpdateTime(int timeHour)
    {
        if (timeHour > 7)
        {
            nextColor = nightColor;
            lerpProgress = 0f;
            lerping = true;

            List<ActivateSpotlightPlay> spotlights = new List<ActivateSpotlightPlay>();

            foreach (GameObject npc in GameObject.FindGameObjectsWithTag("NPC"))
            {
                if (npc.GetComponent<ActivateSpotlightPlay>() != null)
                    spotlights.Add(npc.GetComponent<ActivateSpotlightPlay>());
            }
            foreach (GameObject lamp in GameObject.FindGameObjectsWithTag("Lamp"))
            {
                if (lamp.GetComponent<ActivateSpotlightPlay>() != null)
                    spotlights.Add(lamp.GetComponent<ActivateSpotlightPlay>());
            }
            foreach (ActivateSpotlightPlay light in spotlights)
                light.ActivateSpotlight();
        }
        else if (timeHour > 5)
        {
            nextColor = sunsetColor;
            lerpProgress = 0f;
            lerping = true;
        }
        
    }
}
