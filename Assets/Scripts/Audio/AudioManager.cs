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
    bool musicFadingIn;
    bool soundsFadingIn;
    bool musicFadingout;
    bool soundsFadingout;
    Music queuedClip;
    float fadeCounter;
    public UnityEvent OnFadeCompletion;

    private void Awake()
    {
        // setting up singleton
        if (instance != null && instance != this)
            Destroy(this);
        instance = this;

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.volume = 0.0f;
        musicSource.loop = true;

        musicFadingIn = false;
        musicFadingout = false;
        fadeCounter = 0.0f;
        sources = new List<AudioSource>();
    }

    public static AudioManager Instance()
    {
        return instance;
    }

    private void Update()
    {
        // sounds fade in / out
        if (soundsFadingIn)
        {
            fadeCounter += Time.deltaTime / 2.0f;
            foreach (AudioSource src in sources)
                src.volume += Time.deltaTime;

            if (fadeCounter >= 1.0f)
            {
                soundsFadingIn = false;
            }
        }
        else if (soundsFadingout)
        {
            fadeCounter -= Time.deltaTime;

            foreach (AudioSource src in sources)
                src.volume -= Time.deltaTime;

            if (fadeCounter <= 0.0f)
            {
                for (int i = 0; i < sources.Count; i++)
                {
                    AudioSource src = sources[i];
                    src.Stop();
                    sources.Remove(src);
                    Destroy(src);
                }
                soundsFadingIn = true;
                soundsFadingout = false;
            }
        }

        // music fade in / out
        if (musicFadingIn)
        {
            musicSource.volume += Time.deltaTime / 2.0f;
            if (musicSource.volume >= 0.7f)
                musicFadingIn = false;
        }
        else if (musicFadingout)
        {
            musicSource.volume -= Time.deltaTime;


            if (fadeCounter <= 0.0f)
            {
                musicSource.clip = null;
                OnFadeCompletion.Invoke();
                musicFadingout = false;
            }
        }
    }

    public void TransitionAudio(string musicClipName)
    {
        SoundsFadeOut();

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
        musicFadingIn = true;
        OnFadeCompletion.RemoveListener(MusicFadeIn);
    }

    private void MusicFadeOut()
    {
        musicFadingout = true;
    }

    private void SoundsFadeIn()
    {
        fadeCounter = 0.0f;
        soundsFadingIn = true;
    }

    private void SoundsFadeOut()
    {
        fadeCounter = 1.0f;
        soundsFadingout = true;
    }


    public void PlaySound(string soundName)
    {
        //// don't play the sound if we're in the middle of a transition
        //if (fadeCounter < 1.0f)
        //    return;

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