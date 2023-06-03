using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAIGraphics : MonoBehaviour
{
    Transform graphics;
    Animator animator;
    int order;
    Vector2 previousPos;

    public int defaultXScale = 1;

    // for footsteps audio
    private Vector2 lastFootStepPos;
    public bool multipleFootSteps = false; // setting only for Barry + Jerry
    [HideInInspector]
    public Screen myScreen;

    private enum AnimationState
    {
        WalkFront,
        WalkBack,
        WalkSide,
        Idle
    }

    AnimationState currentState;

    // Start is called before the first frame update
    void Start()
    {
        graphics = transform.Find("Graphics");
        animator = GetComponent<Animator>();
        previousPos = transform.position;
        currentState = AnimationState.Idle;

        lastFootStepPos = transform.localPosition;

        // find closest screen (the one the npc is on) and assign it
        myScreen = GameObject.Find("Dock").GetComponent<Screen>();
        foreach (Screen screen in FindObjectsOfType<Screen>())
        {
            if (Vector2.Distance(myScreen.gameObject.transform.position, transform.position) > Vector2.Distance(screen.gameObject.transform.position, transform.position))
                myScreen = screen;
        }
    }

    // Update is called once per frame
    void Update()
    {
        order = 1000 - Mathf.RoundToInt(graphics.gameObject.transform.position.y * 10f);
        graphics.GetComponent<SpriteRenderer>().sortingOrder = order;

        // set animation state
        Vector2 posDifference = (Vector2)transform.position - previousPos;
        if (posDifference == Vector2.zero && currentState != AnimationState.Idle) // idle
        {
            UpdateAnimationState("Idle");
            currentState = AnimationState.Idle;
            lastFootStepPos = transform.localPosition; // audio
        }
        else if (Mathf.Abs(posDifference.y) > Mathf.Abs(posDifference.x))
        {
            if (posDifference.y > 0.0f && currentState != AnimationState.WalkBack)
            {
                UpdateAnimationState("WalkBack");
                currentState = AnimationState.WalkBack;
            }
            else if (posDifference.y < 0.0f && currentState != AnimationState.WalkFront) // front
            {
                UpdateAnimationState("WalkFront");
                currentState = AnimationState.WalkFront;
            }
        }
        else if (posDifference.x > 0.0f && currentState != AnimationState.WalkSide) // side
        {
            UpdateAnimationState("WalkSide");
            currentState = AnimationState.WalkSide;
        }
        else if (posDifference.x < 0.0f && currentState != AnimationState.WalkSide)
        {
            UpdateAnimationState("WalkSide");
            currentState = AnimationState.WalkSide;
        }

        // flip sprite based on direction facing
        if (posDifference.x > 0.0f)
            transform.localScale = new Vector3(-defaultXScale, 1.0f, 1.0f);
        else
            transform.localScale = new Vector3(defaultXScale, 1.0f, 1.0f);

        previousPos = transform.position;

    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(transform.localPosition, lastFootStepPos) > 1.5f && myScreen.active)
        {
            Debug.Log("currentPos: " + transform.localPosition + "last foot step pos: " + lastFootStepPos + " play footstep");
            float rand = Random.Range(0f, 1f);
            if (rand > 0.5f)
                AudioManager.Instance().PlaySound("FootStep3");
            else
                AudioManager.Instance().PlaySound("FootStep4");

            if (multipleFootSteps)
                StartCoroutine(DelayedStep());

            lastFootStepPos = transform.localPosition;
        }
    }

    IEnumerator DelayedStep()
    {
        yield return new WaitForSeconds(0.08f);
        float rand = Random.Range(0f, 1f);
        if (rand > 0.5f)
            AudioManager.Instance().PlaySound("FootStep3");
        else
            AudioManager.Instance().PlaySound("FootStep4");
    }

    private void UpdateAnimationState(string currentAnimation)
    {
        animator.SetBool("WalkBack", false);
        animator.SetBool("WalkFront", false);
        animator.SetBool("WalkSide", false);
        animator.SetBool("Idle", false);

        animator.SetBool(currentAnimation, true);
    }
}
