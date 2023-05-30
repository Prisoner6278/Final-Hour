using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.AI;

public class NPCDialogueManager : MonoBehaviour
{
    public string characterName;
    public TextAsset dialogueCSV;
    public float talkSpeed = 70f;
    public string voiceSoundName = "DialogueSoundNormal";
    public string portraitAnimatorPath; // this is annoying, thanks Unity
    [SerializeField]
    private DialogueConversation[] conversations;
    DialogueConversation currentConvo;

    // Start is called before the first frame update
    void Start()
    {
        currentConvo = null;
        conversations = CSVReader.ReadNpcCSV(dialogueCSV);

        foreach (DialogueConversation convo in conversations)
        {
            convo.SetCharacterName(characterName);
            convo.SetTalkSpeed(talkSpeed);
            convo.SetVoiceSoundName(voiceSoundName);
        }
    }

    public void StartConversation()
    {
        // defaults to final entry
        currentConvo = conversations[conversations.Length - 1];

        // decide what conversation to start
        for (int i = 0; i < conversations.Length; i++)
        {
            // if the convo requirements are met AND either the convo hasn't been read or the repeat requirements haven't been met
            if (QuestRequirementsMet(conversations[i].GetQuestRequirements()) && ItemRequirementsMet(conversations[i].GetItemRequirements()) && 
                (!conversations[i].GetReadStatus() || !RepeatRequirementsMet(conversations[i].GetRepeatRequirements())))
            {
                currentConvo = conversations[i];
                break;
            }
        }

        // send to dialogue display
        DialogueDisplay.Instance().OnDialogueCompletion.AddListener(ConversationCompleted);
        DialogueDisplay.Instance().ActivateDisplay(currentConvo, portraitAnimatorPath);

        if (GetComponent<NavMeshAgent>() != null)
            GetComponent<NavMeshAgent>().isStopped = true;
    }

    // triggered by dialogue display event
    private void ConversationCompleted()
    {
        // update quest list with triggers
        string[] triggers = currentConvo.GetCompletionTriggers();
        for (int i = 0; i < triggers.Length; i++)
        {
            DialogueConversation triggerConvo = Array.Find(conversations, x => x.GetConversationID() == triggers[i]);
            if (triggerConvo != null)
                triggerConvo.SetReadStatus(true); // trigger is for marking a convo as read
            else
            {
                if (QuestManager.Instance().CheckIfTagIsQuest(triggers[i]))
                    QuestManager.Instance().SetQuestCompletion(triggers[i], true); // trigger is for marking a quest as complete
                else
                    ItemManager.Instance().SetItemHolding(triggers[i]); // trigger is for giving or taking an item
            }
        }

        currentConvo.SetReadStatus(true);
        DialogueDisplay.Instance().OnDialogueCompletion.RemoveListener(ConversationCompleted);

        StartCoroutine(DelayedNavAgent());
    }

    IEnumerator DelayedNavAgent()
    {
        yield return new WaitForSeconds(2.0f);
        if (GetComponent<NavMeshAgent>() != null)
            GetComponent<NavMeshAgent>().isStopped = false;
    }

    private bool QuestRequirementsMet(string[] requirements)
    {
        for (int i = 0; i < requirements.Length; i++)
        {
            if (!QuestManager.Instance().GetQuestCompletion(requirements[i]))
                return false;
        }

        return true;
    }

    private bool RepeatRequirementsMet(string[] requirements)
    {
        for (int i = 0; i < requirements.Length; i++)
        {
            if (QuestManager.Instance().CheckIfTagIsQuest(requirements[i])) // quest requirement
            {
                if (!QuestManager.Instance().GetQuestCompletion(requirements[i])) // not complete
                {
                    return false;
                }
            }
            else // item requirement
            {
                if (!ItemManager.Instance().GetItemHolding(requirements[i])) // don't have item
                {
                    return false;
                }
            }
        }

        return true;
    }

    private bool ItemRequirementsMet(string[] requirements)
    {
        for (int i = 0; i < requirements.Length; i++)
        {
            if (!ItemManager.Instance().GetItemHolding(requirements[i]))
                return false;
        }

        return true;
    }
}

[System.Serializable]
public class DialogueConversation
{
    [SerializeField]
    private string conversationId;
    [SerializeField]
    private bool alreadyRead;
    [SerializeField]
    private string[] questRequirements;
    [SerializeField]
    private string[] itemRequirements;
    [SerializeField]
    private string[] repeatRequirements;
    [SerializeField]
    private string[] completionTriggers;
    [SerializeField]
    private string[] dialogueLines;
    [SerializeField]
    private string characterName;
    private float talkSpeed;
    private string voiceSoundName;

    public void SetConversationID(string newID)
    {
        conversationId = newID;
    }

    public string GetConversationID() => conversationId;

    public void SetReadStatus(bool readStatus)
    {
        alreadyRead = readStatus;
    }

    public bool GetReadStatus() => alreadyRead;

    public void SetQuestRequirements(string[] requirements)
    {
        questRequirements = requirements;
    }

    public string[] GetQuestRequirements() => questRequirements;

    public void SetItemRequirements(string[] requirements)
    {
        itemRequirements = requirements;
    }

    public string[] GetItemRequirements() => itemRequirements;

    public void SetRepeatRequirements(string[] requirements)
    {
        repeatRequirements = requirements;
    }

    public string[] GetRepeatRequirements() => repeatRequirements;

    public void SetCompletionTriggers(string[] requirements)
    {
        completionTriggers = requirements;
    }

    public string[] GetCompletionTriggers() => completionTriggers;

    public void SetDialogueLines(string[] dialogue)
    {
        dialogueLines = dialogue;
    }

    public string[] GetDialogueLines() => dialogueLines;

    public void SetTalkSpeed(float newTalkSpeed)
    {
        talkSpeed = newTalkSpeed;
    }

    public float GetTalkSpeed() => talkSpeed;

    public void SetVoiceSoundName(string newVoiceSoundName)
    {
        voiceSoundName = newVoiceSoundName;
    }

    public string GetVoiceSoundName() => voiceSoundName;

    public void SetCharacterName(string charName)
    {
        characterName = charName;
    }

    public string GetCharacterName() => characterName;
}