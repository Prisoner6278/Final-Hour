using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public bool movementLocked;
    Vector2 moveDir;

    Animator animator;
    AnimationState animationState;

    // npc interaction
    bool inNPCArea;

    // used for screen transitions / potentially cutscenes?
    Queue<(Vector2, float)> moveQueue = new Queue<(Vector2, float)>();
    Vector3 moveQueueGoalPos;
    bool playingQueue;

    PlayerStateController stateController;

    // IS THIS STUFF ACTUALLY BEING USED??
    private enum AnimationState
    {
        WalkFront,
        WalkBack,
        WalkSide,
        Idle
    }


    // Start is called before the first frame update
    void Start()
    {
        inNPCArea = false;
        movementLocked = false;
        playingQueue = false;

        stateController = GetComponent<PlayerStateController>();
        animator = GetComponent<Animator>();
        animationState = AnimationState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        if (!movementLocked) // regular movement
        {
            moveDir.x = Input.GetAxisRaw("Horizontal");
            moveDir.y = Input.GetAxisRaw("Vertical");
        }
        else if (!playingQueue && moveQueue.Count > 0) // begin next auto move in queue
        {
            PlayMoveQueue();
        }
        else if (playingQueue && Vector2.Distance(transform.position, moveQueueGoalPos) < 0.05f) // finished current auto move
        {
            ResetMoveDir();
            stateController.ChangeState(PlayerState.StateChangeInstruction.ChangeToRoam);
            moveSpeed *= 2.0f;
            playingQueue = false;
        }
    }

    // actual movement performed here to avoid jumps from framerate
    private void FixedUpdate()
    {
        if (!movementLocked) // regular movement
        {
            // animation stuff
            if (moveDir.x == 0 && moveDir.y != 0)
            {
                if (moveDir.y > 0)
                {
                    UpdateAnimationState("WalkBack");
                    animationState = AnimationState.WalkBack;
                }
                else if (moveDir.y < 0)
                {
                    UpdateAnimationState("WalkFront");
                    animationState = AnimationState.WalkFront;
                }
            }
            else if (moveDir.x > 0)
            {
                UpdateAnimationState("WalkSide");
                animationState = AnimationState.WalkSide;
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (moveDir.x < 0)
            {
                UpdateAnimationState("WalkSide");
                animationState = AnimationState.WalkSide;
                transform.localScale = new Vector3(1, 1, 1);
            }
            else // idle
            {
                if (animationState == AnimationState.WalkSide)
                {
                    UpdateAnimationState("IdleSide");
                }
                else if (animationState == AnimationState.WalkFront)
                {
                    UpdateAnimationState("IdleBack");
                }
                else
                    UpdateAnimationState("IdleFront");
                animationState = AnimationState.Idle;
            }
        }

        // actual movement
        transform.Translate(moveDir * Time.deltaTime * moveSpeed);
    }

    private void UpdateAnimationState(string currentAnimation)
    {
        animator.SetBool("WalkBack", false);
        animator.SetBool("WalkFront", false);
        animator.SetBool("WalkSide", false);
        animator.SetBool("IdleBack", false);
        animator.SetBool("IdleFront", false);
        animator.SetBool("IdleSide", false);

        animator.SetBool(currentAnimation, true);
    }

    // set by states
    public void LockMovement()
    {
        // if glitch with player animation playing during dialogue,
        // check here with these debug logs
        //Debug.Log("movedir: " + moveDir);
        if (moveDir.x > 0.0f || moveDir.x < 0.0f)
        {
            UpdateAnimationState("IdleSide");
            //Debug.Log("idle side");
        }
        else if (moveDir.y > 0.0f)
        {
            UpdateAnimationState("IdleBack");
            //Debug.Log("idle back");
        }
        else
        {
            UpdateAnimationState("IdleFront");
            //Debug.Log("idle front");
        }

        ResetMoveDir();

        GetComponent<BoxCollider2D>().enabled = false;
        movementLocked = true;
    }

    // set by states
    public void UnlockMovement()
    {
        GetComponent<BoxCollider2D>().enabled = true;
        movementLocked = false;
    }

    // for clarity of code
    private void ResetMoveDir()
    {
        moveDir = Vector2.zero;
    }

    // takes in movement direction and distance, called by a state
    public void QueueMovement(Vector2 newMovementDir, float newMoveDistance)
    {
        moveQueue.Enqueue((newMovementDir, newMoveDistance));
    }

    // perform each movement from queue until its empty
    void PlayMoveQueue()
    {
        playingQueue = true;
        // updates move info for the queued move
        (Vector2, float) moveInfo = moveQueue.Dequeue();
        moveDir = moveInfo.Item1;
        moveSpeed /= 2.0f;
        moveQueueGoalPos = transform.position + new Vector3(moveDir.x * moveInfo.Item2, moveDir.y * moveInfo.Item2, 0);
        // playingQueue will be set to false in update once player is at goal pos
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "NPC" && !inNPCArea)
        {
            collision.gameObject.GetComponent<NPCDialogueManager>().StartConversation();
            stateController.ChangeState(PlayerState.StateChangeInstruction.ChangeToDialogue);
            inNPCArea = true;
        }
        else if (collision.gameObject.tag == "Item")
        {
            collision.gameObject.GetComponent<WorldItem>().PickupItem();
            stateController.ChangeState(PlayerState.StateChangeInstruction.ChangeToDialogue);
        }
        else if (collision.gameObject.tag == "ScreenBoundary" && collision.gameObject.GetComponent<ScreenBoundary>().enabled == true)
        {
            // going from roaming state ----> transition state
            stateController.ChangeState(PlayerState.StateChangeInstruction.ChangeToTransition);
            Vector4 boundaryData = collision.gameObject.GetComponent<ScreenBoundary>().GetTransitionDirection();
            Vector2 teleportlocation = new Vector2(boundaryData.z, boundaryData.w);
            Vector2 movementDir = new Vector2(boundaryData.x, boundaryData.y);
            transform.position = teleportlocation;
            QueueMovement(movementDir, GameData.Instance.screenSpacingDistance);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "NPC")
            inNPCArea = false;
    }
}