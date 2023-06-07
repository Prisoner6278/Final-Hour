using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightCutscene : MonoBehaviour
{
    float lerpProgress;
    float lerpSpeed = 1.0f;
    bool lerping;
    SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (lerping)
        {
            lerpProgress += Time.deltaTime / lerpSpeed;
            sprite.color = Color.Lerp(Color.clear, Color.white, lerpProgress);
            if (lerpProgress >= 1.0f)
            {
                lerping = false;
                sprite.color = Color.black;
                AudioManager.Instance().PlaySound("Smack");
            }
        }
    }

    public void BeginLerp()
    {
        lerping = true;
    }
}
