using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CSVReader : MonoBehaviour
{
    // will have to change if csv format changes
    private static int npcCSVColumnNum = 6;

    // CSV layout:
    // Quest Requirements | Item Requirements | Converstation ID | Dialogue | Repeat Requirements | Triggers

public static DialogueConversation[] ReadNpcCSV(TextAsset input)
    {
        string[] data = input.text.Split(new string[] { ";", "\n" }, StringSplitOptions.RemoveEmptyEntries);

        int rowNum = (data.Length / npcCSVColumnNum) - 1;
        
        List<DialogueConversation> convoList = new List<DialogueConversation>();
        
        string currentConvoID = "";
        DialogueConversation currentConvo = null;
        List<string> currentDialogueLines = null;
        
        // scanning each row
        for (int i = 0; i < rowNum; i++)
        {
            string convoID = data[npcCSVColumnNum * (i + 1) + 2].Trim();
            if (convoID != currentConvoID)
            {
                currentConvoID = convoID;

                // add previous convo to complete list
                if (currentConvo != null)
                {
                    currentConvo.SetDialogueLines(currentDialogueLines.ToArray());
                    convoList.Add(currentConvo);
                }

                // set up new convo
                currentConvo = new DialogueConversation();
                currentConvo.SetConversationID(currentConvoID);
                currentDialogueLines = new List<string>();

                // set up quest requirements
                string questId = data[npcCSVColumnNum * (i + 1)].Trim();
                if (questId != "~")
                {
                    // this convo has quest requirements
                    string[] requirements = { questId }; // NEED TO COME BACK AND ALLOW FOR MULTIPLE REQUIREMENTS
                    currentConvo.SetQuestRequirements(requirements);
                }
                else
                {
                    // no requirements
                    currentConvo.SetQuestRequirements(new string[] { });
                }

                // set up item requirements
                string itemId = data[npcCSVColumnNum * (i + 1) + 1].Trim();
                if (itemId != "~")
                {
                    // this convo has item requirements
                    string[] requirements = { itemId }; // NEED TO COME BACK AND ALLOW FOR MULTIPLE REQUIREMENTS
                    currentConvo.SetItemRequirements(requirements);
                }
                else
                {
                    // no requirements
                    currentConvo.SetItemRequirements(new string[] { });
                }

                // add dialogue
                currentDialogueLines.Add(data[npcCSVColumnNum * (i + 1) + 3]);

                // set up repeat requirements
                string repeatReqId = data[npcCSVColumnNum * (i + 1) + 4].Trim(); // NEED TO COME BACK AND ALLOW FOR MULTIPLE REPEAT REQS
                if (repeatReqId != "~")
                {
                    // this convo has repeat requirements
                    string[] repeatReqs = { repeatReqId };
                    currentConvo.SetRepeatRequirements(repeatReqs);
                }
                else
                {
                    // no repeat requirements
                    currentConvo.SetRepeatRequirements(new string[] { });
                }

                // set up triggers
                string[] triggerIds = data[npcCSVColumnNum * (i + 1) + 5].Trim().Split(' '); // NEED TO COME BACK AND ALLOW FOR MULTIPLE TRIGGERS

                if (triggerIds[0] != "~")
                    currentConvo.SetCompletionTriggers(triggerIds);
                else
                {
                    // no triggers
                    currentConvo.SetCompletionTriggers(new string[] { });
                }
            }
            else
            {
                // add dialogue to current convo
                currentDialogueLines.Add(data[npcCSVColumnNum * (i + 1) + 3]);
            }
        }

        // add last conversation
        currentConvo.SetDialogueLines(currentDialogueLines.ToArray());
        convoList.Add(currentConvo);

        return convoList.ToArray();
    }

    public static Quest[] ReadQuestListCSV(TextAsset input)
    {
        string[] data = input.text.Split(new string[] { ";", "\n" }, StringSplitOptions.RemoveEmptyEntries);

        int tableSize = data.Length - 1;
        Quest[] questList = new Quest[tableSize];

        for (int i = 0; i < tableSize; i++)
            questList[i] = new Quest(data[(i + 1)].Trim());

        return questList;
    }

    public static Item[] ReadItemListCSV(TextAsset input)
    {
        string[] data = input.text.Split(new string[] { ";", "\n" }, StringSplitOptions.RemoveEmptyEntries);

        int tableSize = data.Length - 1;
        Item[] itemList = new Item[tableSize];

        for (int i = 0; i < tableSize; i++)
            itemList[i] = new Item(data[(i + 1)].Trim());

        return itemList;
    }
}
