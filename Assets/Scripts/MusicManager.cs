using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    private AudioSource audio;
    public AudioClip currentClip;

    public static MusicManager Instance { get; private set; }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        audio = GetComponent<AudioSource>();
    }

    private void Start()
    {
        audio.clip = currentClip;
        audio.Play();
    }


    public void ChangeTracks(AudioClip music)
    {
        currentClip = music;
        StartCoroutine(FadeOut());
    }
    
    private IEnumerator FadeOut()
    {
        float fadeDuration = 6;
        float currentTime = fadeDuration;
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            audio.volume -= Mathf.Lerp(audio.volume, 0, currentTime / fadeDuration);
            yield return null;
        }
        
        CallFadeIn(currentClip);
        yield break;
    }

    private void CallFadeIn(AudioClip music)
    {
        StartCoroutine(FadeIn(music));
    }

    private IEnumerator FadeIn(AudioClip music)
    {
        audio.clip = music;
        audio.Play();
        
        float fadeDuration = 6;
        float currentTime = fadeDuration;
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            audio.volume += Mathf.Lerp(audio.volume, 1, currentTime / fadeDuration);
            yield return null;
        }
        yield break;
    }
}
