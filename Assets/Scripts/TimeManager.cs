using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public int minuteIncreasePerQuest = 150;
    public int startTimeHour;
    int hour;
    int minute;
    string timeSuffix;
    TMP_Text timeText;
    CameraCover camCover;


    private void Awake()
    {
        timeText = GameObject.Find("ClockCanvas").GetComponentInChildren<TMP_Text>();
        if (timeText == null)
            throw new System.Exception("Couldn't locate Clock Canvas! Make sure it's added in the scene!");

        camCover = GameObject.Find("CameraCover").GetComponent<CameraCover>();
        if (camCover == null)
            throw new System.Exception("Couldn't locate CameraCover! Make sure it's added in the scene!");

        hour = startTimeHour;
        minute = 0;
        timeSuffix = "PM";
        timeText.text = hour + ":" + minute.ToString("#00") + " " + timeSuffix;
    }

    public void AddTime()
    {
        minute += minuteIncreasePerQuest;
        while (minute > 59)
        {
            minute -= 60;
            hour += 1;
        }
        if (hour > 12)
        {
            hour = 1;
            if (timeSuffix == "PM")
                timeSuffix = "AM";
            else
                timeSuffix = "PM";
        }

        camCover.UpdateTime(hour);
        timeText.text = hour + ":" + minute.ToString("#00") + " " + timeSuffix;
    }
}
