using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarryJerryCutscene : MonoBehaviour
{
    private GameObject player;
    private GameObject cutSceneCanvas;

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

    public void TriggerCutScene()
    {
        player.GetComponent<PlayerStateController>().ChangeState(PlayerState.StateChangeInstruction.ChangeToDialogue);
        cutSceneCanvas.GetComponent<Animator>().SetBool("PlayCutScene", true);
        player.GetComponent<PlayerMovement>().QueueMovement(Vector2.left, 8f);
        DialogueDisplay.Instance().OnDialogueCompletion.AddListener(FinishCutScene);
    }

    private void FinishCutScene()
    {
        cutSceneCanvas.GetComponent<Animator>().SetBool("PlayCutScene", false);
        DialogueDisplay.Instance().OnDialogueCompletion.RemoveListener(FinishCutScene);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            TriggerCutScene();
        }
    }
}
