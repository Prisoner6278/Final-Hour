using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JamesNPCTest : MonoBehaviour
{
    public float moveSpeed = 1f;
    Vector2 moveDir;

    // used for screen transitions / potentially cutscenes?
    Queue<(Vector2, float)> moveQueue = new Queue<(Vector2, float)>();
    Vector3 moveQueueGoalPos;
    bool playingQueue;
    bool reachedGoalPos;

    Vector2 originalPos;
    public float walkRadius = 4.0f;

    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;
        ResetMoveDir();
        StartCoroutine(PlayMoveQueue());
        reachedGoalPos = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playingQueue && Vector2.Distance(transform.position, moveQueueGoalPos) < 0.05f) // finished current auto move
        {
            ResetMoveDir();
            //playingQueue = false;
        }
    }

    private void FixedUpdate()
    {
        // actual movement
        transform.Translate(moveDir * Time.deltaTime * moveSpeed);
    }

    // for clarity of code
    private void ResetMoveDir()
    {
        moveDir = Vector2.zero;

        int direction = Random.Range(0, 15);
        Vector2 newDir;

        switch (direction)
        {
            case 0:
                newDir = Vector2.right;
                break;
            case 1:
                newDir = Vector2.up;
                break;
            case 2:
                newDir = Vector2.left;
                break;
            case 3:
                newDir = Vector2.down;
                break;
            case 4:
                newDir = Vector2.right;
                break;
            case 5:
                newDir = new Vector2(1.0f, 1.0f);
                break;
            case 6:
                newDir = new Vector2(-1.0f, -1.0f);
                break;
            case 7:
                newDir = new Vector2(-1.0f, 1.0f);
                break;
            case 8:
                newDir = new Vector2(1.0f, -1.0f);
                break;
            default:
                newDir = Vector2.zero;
                break;
        }
        QueueMovement(newDir, Random.Range(0, 4));
        StartCoroutine(PlayMoveQueue());
    }

    // takes in movement direction and distance, called by a state
    public void QueueMovement(Vector2 newMovementDir, float newMoveDistance)
    {
        moveQueue.Enqueue((newMovementDir, newMoveDistance));
    }

    // perform each movement from queue until its empty
    private IEnumerator PlayMoveQueue()
    {
        playingQueue = true;
        // updates move info for the queued move
        (Vector2, float) moveInfo = moveQueue.Dequeue();
        moveDir = moveInfo.Item1;
        if (moveDir == Vector2.zero)
        {
            Debug.Log("playing zero from queueu");
            moveSpeed = 0.0f;
            yield return new WaitForSeconds(moveInfo.Item2);
            moveSpeed = 1.0f;
        }
        moveQueueGoalPos = transform.position + new Vector3(moveDir.x * moveInfo.Item2, moveDir.y * moveInfo.Item2, 0);
        moveQueueGoalPos = new Vector3(Mathf.Clamp(moveQueueGoalPos.x, originalPos.x - walkRadius, originalPos.x + walkRadius),
           Mathf.Clamp(moveQueueGoalPos.y, originalPos.y - walkRadius, originalPos.y + walkRadius), 0);
    }
}
