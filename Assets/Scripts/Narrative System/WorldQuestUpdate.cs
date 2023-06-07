using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldQuestUpdate : MonoBehaviour
{
    public string questName;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
            QuestManager.Instance().SetQuestCompletion(questName, true);
    }
}
