using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// KIERAN

public class AudioManager : MonoBehaviour
{

    //Array to store audio files
    public Sound[] sfxSounds, bgmSounds;
    public AudioSource sfxSource, bgmSource;

    public static AudioManager instance;
    public static float sfxVol = 0.2f;
    public static float bgmVol = 0.2f;

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

    public void Start(){
        PlayBGM("Main Menu");
    }
    public void PlayBGM(string name)
    {
        Sound s = Array.Find(bgmSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Audio Not Found");
        }
        else
        {
            bgmSource.clip = s.clip;
            bgmSource.volume = bgmVol;
            bgmSource.Play();
        }
    }

    public void StopBGM()
    {
        if (gameObject != null)
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
            sfxSource.volume = sfxVol;
            sfxSource.Play();
        }
    }
}