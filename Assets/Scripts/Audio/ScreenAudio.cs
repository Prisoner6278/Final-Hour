using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenAudio : MonoBehaviour
{
    public BackgroundSound[] backgroundSounds;
    public string music;
    private static GameObject activeScreen;

    // Start is called before the first frame update
    void Start()
    {
        // initial sound schedules for the screen
        foreach (BackgroundSound sound in backgroundSounds)
            sound.SchedulePlay(Time.time);
    }

    // Update is called once per frame
    void Update()
    {
        // this is kinda expensive. better way to do it??
        if (activeScreen == this.gameObject)
        {
            foreach (BackgroundSound sound in backgroundSounds)
                sound.TimeCheck(Time.time);
        }
    }

    public void SetToActiveScreen()
    {
        Debug.Log(gameObject.name + " set to active screen");
        activeScreen = gameObject;
        AudioManager.Instance().SetMusicTrack(music);
    }
}

[System.Serializable]
public class BackgroundSound
{
    public string soundName;
    // these control how often the sound will play!
    public float randomFrequencyMin;
    public float randomFrequencyMax;

    [HideInInspector]
    public float currentWaitTime = 0.0f;
    [HideInInspector]
    public float lastPlayTimeStamp = 0.0f;

    public void TimeCheck(float currentTime)
    {
        if (currentTime >= lastPlayTimeStamp + currentWaitTime)
        {
            AudioManager.Instance().PlaySound(soundName);
            SchedulePlay(currentTime);
        }
    }

    public void SchedulePlay(float currentTime)
    {
        currentWaitTime = Random.Range(randomFrequencyMin, randomFrequencyMax);
        lastPlayTimeStamp = currentTime;
    }
}