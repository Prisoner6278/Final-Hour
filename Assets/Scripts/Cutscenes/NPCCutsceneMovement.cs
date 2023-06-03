using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPCCutsceneMovement : MonoBehaviour
{
    Vector2 moveDir;
    public float moveSpeed = 1.5f;

    Queue<(Vector2, float)> moveQueue = new Queue<(Vector2, float)>();
    Vector3 moveQueueGoalPos;
    bool playingQueue;
    public UnityEvent OnMoveQueueCompletion;

    // Update is called once per frame
    void Update()
    {
        if (!playingQueue && moveQueue.Count > 0) // begin next auto move in queue
        {
            PlayMoveQueue();
        }
        else if (playingQueue && Vector2.Distance(transform.position, moveQueueGoalPos) < 0.1f) // finished current auto move
        {
            ResetMoveDir();
            playingQueue = false;

            if (moveQueue.Count < 1)
                OnMoveQueueCompletion.Invoke();
        }
        else // no movement
        {
            //lastFootStepPos = transform.position; // audio
        }
    }

    private void ResetMoveDir()
    {
        moveDir = Vector2.zero;
    }

    private void FixedUpdate()
    {
        transform.Translate(moveDir.normalized * Time.deltaTime * moveSpeed);
    }

    public void QueueMovement(Vector2 newMovementDir, float newMoveDistance)
    {
        moveQueue.Enqueue((newMovementDir, newMoveDistance));
    }

    void PlayMoveQueue()
    {
        playingQueue = true;
        // updates move info for the queued move
        (Vector2, float) moveInfo = moveQueue.Dequeue();
        moveDir = moveInfo.Item1;
        moveQueueGoalPos = transform.position + new Vector3(moveDir.x * moveInfo.Item2, moveDir.y * moveInfo.Item2, 0);
        // playingQueue will be set to false in update once player is at goal pos
    }
}
