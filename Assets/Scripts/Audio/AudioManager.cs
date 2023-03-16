using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioManager : MonoBehaviour
{
    public List<Sound> sounds;
    public List<Music> music;
    private List<AudioSource> sources;
    private AudioSource musicSource;

    // singleton
    private static AudioManager instance;

    // music fade
    bool fadingIn;
    bool fadingout;
    Music queuedClip;
    public UnityEvent OnFadeCompletion;

    private void Awake()
    {
        // setting up singleton
        if (instance != null && instance != this)
            Destroy(this);
        instance = this;

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
    }

    public static AudioManager Instance()
    {
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        fadingIn = false;
        fadingout = false;
        sources = new List<AudioSource>();
    }

    private void Update()
    {
        // music fade in / out
        if (fadingIn)
        {
            musicSource.volume += Time.deltaTime / 2.0f;
            if (musicSource.volume >= 1.0f)
            {
                fadingIn = false;
            }
        }

        if (fadingout)
        {
            musicSource.volume -= Time.deltaTime;
            if (musicSource.volume <= 0.0f)
            {
                OnFadeCompletion.Invoke();
                fadingout = false;
            }
        }
    }

    public void SetMusicTrack(string musicClipName)
    {
        // no music on this screen
        if (musicClipName == "")
        {
            MusicFadeOut();
            return;
        }

        // find music
        Music musicClip = music.Find(x => x.name == musicClipName);
        if (musicClip == null)
            throw new Exception("Tried to play music " + musicClipName + ", music doesn't exist in audio manager list");
        
        // already playing this song
        if (musicClip.clip == musicSource.clip)
            return;

        // set new music!
        queuedClip = musicClip;
        if (musicSource.volume <= 0.0f)
            MusicFadeIn();
        else
        {
            OnFadeCompletion.AddListener(MusicFadeIn);
            MusicFadeOut();
        }
    }

    private void MusicFadeIn()
    {
        musicSource.clip = queuedClip.clip;
        musicSource.Play();
        fadingIn = true;
        OnFadeCompletion.RemoveListener(MusicFadeIn);
    }

    private void MusicFadeOut()
    {
        fadingout = true;
    }

    public void PlaySound(string soundName)
    {
        AudioSource selectedSource = null;

        // search existing sources for a free one
        foreach (AudioSource s in sources)
        {
            if (!s.isPlaying)
            {
                selectedSource = s;
                break;
            }
        }

        // no audio sources available, so make a new one
        if (selectedSource == null)
        {
            selectedSource = gameObject.AddComponent<AudioSource>();
            sources.Add(selectedSource);
        }

        Sound currentSound = sounds.Find(x => x.name == soundName);

        if (currentSound == null)
            throw new Exception("Tried to play sound " + soundName + ", sound doesn't exist in audio manager list");


        selectedSource.volume = UnityEngine.Random.Range(currentSound.baseVolume - currentSound.volumeVariationLow,
            currentSound.baseVolume + currentSound.volumeVariationHigh);
        selectedSource.pitch = UnityEngine.Random.Range(currentSound.basePitch - currentSound.pitchVariationLow,
            currentSound.basePitch + currentSound.pitchVariationHigh);
        selectedSource.clip = currentSound.clip;
        selectedSource.Play();
    }
}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    // volume settings
    public float volumeVariationLow = 0.0f;
    public float volumeVariationHigh = 0.0f;
    public float baseVolume = 1.0f;
    // pitch settings
    public float pitchVariationLow = 0.0f;
    public float pitchVariationHigh = 0.0f;
    public float basePitch = 1.0f;
}

[System.Serializable]
public class Music
{
    public string name;
    public AudioClip clip;
}