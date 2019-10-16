using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioListener))]
public class AudioManager : Singleton<AudioManager>
{

    // Use this to mute game during production
    public bool mute;
    public float musicVolume;

    private AudioSource musicChannel;
    private AudioSource soundChannel;
    private Dictionary<string, AudioClip> soundMap;

    void Start()
    {
        soundMap = new Dictionary<string, AudioClip>();


        musicChannel = new GameObject().AddComponent<AudioSource>();
        musicChannel.transform.SetParent(transform);
        musicChannel.name = "MusicChannel";
        musicChannel.loop = true;

        soundChannel = new GameObject().AddComponent<AudioSource>();
        soundChannel.transform.SetParent(transform);
        soundChannel.name = "SoundChannel";

        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio");
        foreach (AudioClip clip in clips)
        {
            soundMap.Add(clip.name, clip);
        }

        ToggleMute(mute);

        PlayMusic("Home");
    }

    public void PlayMusic(string name)
    {
        musicChannel.clip = soundMap[name];
        musicChannel.loop = true;
        musicChannel.Play();
    }


    public void PlaySound(string name)
    {
        AudioClip clip = soundMap[name];
        soundChannel.PlayOneShot(clip, 1f);
    }

    public void PlaySound(string name, float volume)
    {
        AudioClip clip = soundMap[name];
        soundChannel.PlayOneShot(clip, volume);
    }

    public void ToggleMute(bool mute)
    {
        if (mute)
        {
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = 1;
        }
    }
}
