using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public AudioSource currentAudio;
    public AudioClip currentClip;
    public float fadeDuration;

    public static MusicManager Instance { get; private set; }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        currentAudio.clip = currentClip;
        currentAudio.Play();
    }


    public void ChangeTracks(AudioSource audio, AudioClip music)
    {
        StartCoroutine(FadeOut(currentAudio));
        StartCoroutine(FadeIn(audio, music));
        currentAudio = audio;
        currentClip = music;
    }
    
    private IEnumerator FadeOut(AudioSource audio)
    {
        float currentTime = fadeDuration;
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            audio.volume -= Mathf.Lerp(audio.volume, 0, currentTime / fadeDuration);
            yield return null;
        }
        yield break;
    }

    private IEnumerator FadeIn(AudioSource audio, AudioClip music)
    {
        audio.clip = music;
        audio.Play();
        
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
