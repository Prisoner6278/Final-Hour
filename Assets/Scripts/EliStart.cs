using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Desperation());
    }

    IEnumerator Desperation()
    {
        yield return new WaitForSeconds(0.1f);
        GameObject.FindWithTag("Player").GetComponent<PlayerStateController>().ChangeState(PlayerState.StateChangeInstruction.ChangeToDialogue);
        AudioManager.Instance().PlaySound("DialogueBoxOpen");
        GetComponent<NPCDialogueManager>().StartConversation();
    }
}
