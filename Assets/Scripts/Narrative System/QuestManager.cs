using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{
    public TextAsset questListCSV;
    [SerializeField]
    private Quest[] questList;
    private TimeManager timeManager;

    // singleton
    private static QuestManager instance;

    private void Awake()
    {
        // setting up singleton
        if (instance != null && instance != this)
            Destroy(this);
        instance = this;
    }

    public static QuestManager Instance()
    {
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        questList = CSVReader.ReadQuestListCSV(questListCSV);
        timeManager = GetComponent<TimeManager>();
    }

    public bool GetQuestCompletion(string questID)
    {
        Quest q = Array.Find(questList, x => string.Compare(x.GetID(), questID) == 0);
        if (q == null)
            throw new Exception("Searched quest list for non-existant quest! Did you make a typo somewhere?\n");
        return q.GetCompletion();
    }

    public void SetQuestCompletion(string questID, bool completionStatus)
    {
        Quest q = Array.Find(questList, x => x.GetID() == questID);
        if (q == null)
            throw new Exception("Searched quest list for non-existant quest! Did you make a typo somewhere?\n");
        q.SetCompletion(completionStatus);

        OverallQuestStatusChecks();
        timeManager.AddTime();
    }

    public bool CheckIfTagIsQuest(string tag)
    {
        Quest q = Array.Find(questList, x => x.GetID() == tag);
        if (q == null)
            return false;
        return true;
    }

    private void OverallQuestStatusChecks()
    {
        Quest finalQuest = Array.Find(questList, x => string.Compare(x.GetID(), "#game_over") == 0);
        if (finalQuest.GetCompletion())
        {
            StartCoroutine(EndGame());
            return;
        }

        Quest graceQuest = Array.Find(questList, x => string.Compare(x.GetID(), "#spare_change?") == 0);
        Quest jamesQuest = Array.Find(questList, x => string.Compare(x.GetID(), "#romeo_and_juliet") == 0);
        Quest flakeQuest = Array.Find(questList, x => string.Compare(x.GetID(), "#detective") == 0);
        //if (graceQuest.GetCompletion() && jamesQuest.GetCompletion() && flakeQuest.GetCompletion())
            // do something here when all quests are complete?
    }

    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadScene("EndCutScene");
    }
}

[System.Serializable]
public class Quest
{
    [SerializeField]
    private string id;
    [SerializeField]
    private bool complete;

    public string GetID() => id;
    public bool GetCompletion() => complete;

    public Quest(string questId)
    {
        id = questId;
        complete = false;
    }

    public void SetCompletion(bool completionStatus)
    {
        complete = completionStatus;
    }
}
