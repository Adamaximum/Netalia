using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    private AudioSource audio;
    private CrowdAmbientNoise crowdAmbience;

    [Header("Music Tracks")]
    public AudioClip sewerMusic;
    public AudioClip cityMusic;

 
    [Header("Mixer Values")]
    public AudioMixer mixer;

    public static MusicManager Instance { get; private set; }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        audio = GetComponent<AudioSource>();
        crowdAmbience = GameObject.FindObjectOfType<CrowdAmbientNoise>();
    }

   

    public void ChangeTracks(string roomTag)
    {
        if (roomTag == "Sewer")
        {
            StartCoroutine(FadeMixerGroup.StartFade(mixer, "MusicVolume", 2, -80f));
            crowdAmbience.NoiseOff();
            StartCoroutine(StartNewTrack(sewerMusic));
        }
        else if (roomTag == "Crowd")
        {
            crowdAmbience.NoiseOn();
        }
        else if (roomTag == "Pillars")
        {
            crowdAmbience.NoiseOff();
        }
        else if (roomTag == "City")
        {
            StartCoroutine(FadeMixerGroup.StartFade(mixer, "MusicVolume", 2, -80f));
            StartCoroutine(StartNewTrack(cityMusic));
        }
    }
    private IEnumerator StartNewTrack(AudioClip nextTrack)
    {
        yield return new WaitForSeconds(2.5f);
        StopCoroutine(FadeMixerGroup.StartFade(mixer, "MusicVolume", 2, -80f));
        audio.clip = nextTrack;
        audio.Play();
        mixer.SetFloat("MusicVolume", 0);
    }
}
