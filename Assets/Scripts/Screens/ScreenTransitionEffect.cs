using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTransitionEffect : MonoBehaviour
{
    public Image transitionImage;

    // lerp stuff
    bool wipeTransitioning;
    bool fadingIn;
    float lerpProgress;
    float lerpSpeed = 1.0f;
    Vector3 nextCamPos;

    private static ScreenTransitionEffect instance;

    private void Awake()
    {
        // setting up singleton
        if (instance != null && instance != this)
            Destroy(this);
        instance = this;
    }

    public static ScreenTransitionEffect Instance()
    {
        return instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        wipeTransitioning = false;
        fadingIn = false;
        lerpProgress = 0.0f;
        transitionImage.material.SetFloat("_TransitionProgress", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (wipeTransitioning)
        {
            lerpProgress += Time.deltaTime * lerpSpeed;
            transitionImage.material.SetFloat("_TransitionProgress", lerpProgress);
            if (lerpProgress >= 1.0f)
            {
                StartCoroutine(FadeIn());
                wipeTransitioning = false;
            }
        }
        else if (fadingIn)
        {
            lerpProgress += Time.deltaTime;
            transitionImage.color = Color.Lerp(Color.white, Color.clear, lerpProgress);
            if (lerpProgress >= 1.0f)
                fadingIn = false;
        }
    }

    public void Transition(float degreeDirection, Vector3 newCamPosition)
    {
        transitionImage.color = Color.white;
        transitionImage.material.SetFloat("_ImageRotation", degreeDirection);
        wipeTransitioning = true;
        fadingIn = false;
        nextCamPos = newCamPosition;
        lerpProgress = 0.0f;
    }

    private IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(0.2f);
        Camera.main.transform.position = new Vector3(nextCamPos.x, nextCamPos.y, -10f);
        lerpProgress = 0.0f;
        fadingIn = true;
    }

    private void OnApplicationQuit()
    {
        transitionImage.material.SetFloat("_TransitionProgress", 0);
    }
}
