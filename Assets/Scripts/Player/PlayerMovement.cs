using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public bool movementLocked;
    Vector2 moveDir;

    Animator animator;
    AnimationState animationState;

    // npc / item interaction
    GameObject closestNPC;
    GameObject closestItem;
    GameObject closestBuilding;

    // used for screen transitions / potentially cutscenes?
    Queue<(Vector2, float)> moveQueue = new Queue<(Vector2, float)>();
    Vector3 moveQueueGoalPos;
    bool playingQueue;
 
    PlayerStateController stateController;


    // for footsteps audio
    private Vector2 lastFootStepPos;

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
        closestNPC = null;
        movementLocked = false;
        playingQueue = false;

        stateController = GetComponent<PlayerStateController>();
        animator = GetComponent<Animator>();
        animationState = AnimationState.Idle;

        lastFootStepPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // DEV HAX
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadScene("Day1Scene");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneManager.LoadScene("Day2Scene");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SceneManager.LoadScene("MainMenuScene");
        }

        if (!movementLocked)
        {
            // regular movement
            moveDir.x = Input.GetAxisRaw("Horizontal");
            moveDir.y = Input.GetAxisRaw("Vertical");

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
            {
                // talk with NPC
                if (closestNPC != null)
                {
                    AudioManager.Instance().PlaySound("DialogueBoxOpen");
                    closestNPC.GetComponent<NPCDialogueManager>().StartConversation();
                    stateController.ChangeState(PlayerState.StateChangeInstruction.ChangeToDialogue);
                }
                // pick up item
                else if (closestItem != null)
                {
                    AudioManager.Instance().PlaySound("DialogueBoxOpen");
                    closestItem.gameObject.GetComponent<WorldItem>().PickupItem();
                    stateController.ChangeState(PlayerState.StateChangeInstruction.ChangeToDialogue);
                }
                // enter / exit building
                else if (closestBuilding!= null)
                {
                    ScreenTransition(closestBuilding);
                    stateController.ChangeState(PlayerState.StateChangeInstruction.ChangeToDialogue);
                }
            }
        }
        else if (!playingQueue && moveQueue.Count > 0) // begin next auto move in queue
        {
            PlayMoveQueue();
        }
        else if (playingQueue && Vector2.Distance(transform.position, moveQueueGoalPos) < 0.1f) // finished current auto move
        {
            ResetMoveDir();
            stateController.ChangeState(PlayerState.StateChangeInstruction.ChangeToRoam);
            moveSpeed *= 2.0f;
            playingQueue = false;
        }
        else // no movement
        {
            lastFootStepPos = transform.position; // audio
        }
    }

    // actual movement performed here to avoid jumps from framerate
    private void FixedUpdate()
    {
        // actual movement
        transform.Translate(moveDir.normalized * Time.deltaTime * moveSpeed);
        if (Vector2.Distance(transform.position, lastFootStepPos) > 1.2f)
        {
            float rand = Random.Range(0f, 1f);
            if (rand > 0.5f)
                AudioManager.Instance().PlaySound("FootStep1");
            else
                AudioManager.Instance().PlaySound("FootStep2");

            lastFootStepPos = transform.position;
        }

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
                else
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
                    UpdateAnimationState("IdleFront");
                }
                else
                {
                    UpdateAnimationState("IdleBack");
                }
                animationState = AnimationState.Idle;
            }
        }
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

    public Vector2 GetMoveDir()
    {
        return moveDir;
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
        if (collision.gameObject.tag == "ScreenBoundary" && collision.gameObject.GetComponent<ScreenBoundary>().enabled == true)
        {
            ScreenTransition(collision.gameObject);
        }
    }

    private void ScreenTransition(GameObject obj)
    {
        // going from roaming state ----> transition state
        stateController.ChangeState(PlayerState.StateChangeInstruction.ChangeToTransition);
        Vector4 boundaryData = obj.gameObject.GetComponent<ScreenBoundary>().GetTransitionDirection();
        Vector2 teleportlocation = new Vector2(boundaryData.z, boundaryData.w);
        Vector2 movementDir = new Vector2(boundaryData.x, boundaryData.y);
        transform.position = teleportlocation;
        QueueMovement(movementDir, GameData.Instance.screenSpacingDistance);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "NPC")
        {
            closestNPC = collision.gameObject;
            closestNPC.GetComponent<ObjectOutlineControl>().SetToOutline();
        }
        if (collision.gameObject.tag == "Item")
        {
            closestItem = collision.gameObject;
            closestItem.GetComponent<ObjectOutlineControl>().SetToOutline();
        }
        if (collision.gameObject.tag == "Building")
        {
            closestBuilding = collision.gameObject;
            closestBuilding.GetComponent<ObjectOutlineControl>().SetToOutline();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "NPC")
        {
            closestNPC.GetComponent<ObjectOutlineControl>().SetToDefault();
            closestNPC = null;
        }
        if (collision.gameObject.tag == "Item")
        {
            closestItem.GetComponent<ObjectOutlineControl>().SetToDefault();
            closestItem = null;
        }
        if (collision.gameObject.tag == "Building")
        {
            closestBuilding.GetComponent<ObjectOutlineControl>().SetToDefault();
            closestBuilding = null;
        }
    }
}