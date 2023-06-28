using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{

    //Array to store audio files
    public Sound[] sfxSounds;
    public AudioSource sfxSource;

    public static AudioManager instance;
    public static float sfxVol = 0.5f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Audio Not Found");
        }

        else
        {
            sfxSource.clip = s.clip;
            sfxSource.Play();
        }
    }
}