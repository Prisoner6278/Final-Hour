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

    private void UpdateAnimationState(string currentAnimation)
    {
        animator.SetBool("WalkBack", false);
        animator.SetBool("WalkFront", false);
        animator.SetBool("WalkSide", false);
        animator.SetBool("Idle", false);

        animator.SetBool(currentAnimation, true);
    }
}
