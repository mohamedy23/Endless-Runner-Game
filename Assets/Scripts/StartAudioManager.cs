using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAudioManager : MonoBehaviour
{
    public Sound[] sounds;
    // Start is called before the first frame update
    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.clip;
            s.audioSource.volume = s.volume;
            s.audioSource.pitch = s.pitch;
            s.audioSource.loop = true;
        }
    }
    
    public void play(String name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.audioSource.Play();
        
    }
    public void Stop(String name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.audioSource.Stop();
    }
    public void Pause(String name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.audioSource.Pause();
    }

}
