using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarryJerryCutscene : MonoBehaviour
{
    private GameObject player;
    private GameObject cutSceneCanvas;
    public GameObject barryNJerryPrefab;
    private GameObject barrynJerryInstance;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cutSceneCanvas = GameObject.FindGameObjectWithTag("CutSceneCanvas");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // player walks to middle of screen
    public void Part1()
    {
        // player walk
        player.GetComponent<PlayerStateController>().ChangeState(PlayerState.StateChangeInstruction.ChangeToDialogue);
        cutSceneCanvas.GetComponent<Animator>().SetBool("PlayCutScene", true);
        player.GetComponent<PlayerMovement>().QueueMovement(Vector2.left, 5f);
        player.GetComponent<PlayerMovement>().OnMoveQueueCompletion.AddListener(Part2);
    }

    // bj appear and walk to player, start convo
    private void Part2()
    {
        player.GetComponent<PlayerStateController>().ChangeState(PlayerState.StateChangeInstruction.ChangeToDialogue);
        barrynJerryInstance = Instantiate(barryNJerryPrefab, new Vector3(2.09f, -116.13f, 0.0f), Quaternion.identity);
        barrynJerryInstance.GetComponent<NPCCutsceneMovement>().QueueMovement(new Vector2(0.0f, -1.0f), 6f);
        barrynJerryInstance.GetComponent<NPCCutsceneMovement>().QueueMovement(new Vector2(1.0f, -1.0f), 1.5f);
        barrynJerryInstance.GetComponent<NPCCutsceneMovement>().OnMoveQueueCompletion.AddListener(Part2Point5);
    }

    private void Part2Point5()
    {
        barrynJerryInstance.GetComponent<NPCCutsceneMovement>().OnMoveQueueCompletion.RemoveListener(Part2Point5);
        barrynJerryInstance.GetComponent<NPCDialogueManager>().StartConversation();
        DialogueDisplay.Instance().OnDialogueCompletion.AddListener(Part3);
    }

    // after convo, BJ walk away
    private void Part3()
    {
        player.GetComponent<PlayerStateController>().ChangeState(PlayerState.StateChangeInstruction.ChangeToDialogue);
        DialogueDisplay.Instance().OnDialogueCompletion.RemoveListener(Part3);
        barrynJerryInstance.GetComponent<NPCCutsceneMovement>().QueueMovement(new Vector2(-1.0f, 0f), 10f);
        barrynJerryInstance.GetComponent<NPCCutsceneMovement>().OnMoveQueueCompletion.AddListener(Part4);
    }

    // game resumes
    private void Part4()
    {
        Destroy(barrynJerryInstance);
        cutSceneCanvas.GetComponent<Animator>().SetBool("PlayCutScene", false);
        DialogueDisplay.Instance().OnDialogueCompletion.RemoveListener(Part3);
        player.GetComponent<PlayerStateController>().ChangeState(PlayerState.StateChangeInstruction.ChangeToRoam);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Part1();
        }
    }
}
