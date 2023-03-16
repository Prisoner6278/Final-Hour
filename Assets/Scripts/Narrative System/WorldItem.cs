using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public string itemName;
    public string pickupText;
    public string portraitAnimatorPath;
    private DialogueConversation pickupConvo;
    private float worldItemTalkSpeed = 70f;
    private string voiceSoundName = "DialogueSoundNormal";

    private void Start()
    {
        pickupConvo = new DialogueConversation();
        pickupConvo.SetTalkSpeed(worldItemTalkSpeed);
        pickupConvo.SetVoiceSoundName(voiceSoundName);
        pickupConvo.SetDialogueLines( new string[] { pickupText });
    }
    public void PickupItem()
    {
        DialogueDisplay.Instance().OnDialogueCompletion.AddListener(PickupCompleted);
        DialogueDisplay.Instance().ActivateDisplay(pickupConvo, portraitAnimatorPath);
    }

    private void PickupCompleted()
    {
        ItemManager.Instance().SetItemHolding(itemName);
        Destroy(this.gameObject);
    }
}
