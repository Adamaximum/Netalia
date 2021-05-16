using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource audio;
    private CrowdAmbientNoise crowdAmbience;

    [Header("Music Tracks")]
    public AudioClip sewerMusic;
    public AudioClip cityMusic;

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
            audio.clip = sewerMusic;
            crowdAmbience.NoiseOff();
        }
        else if (roomTag == "Crowd")
        {
            crowdAmbience.NoiseOn();
        }
        else if (roomTag == "Pillars")
        {
            crowdAmbience.NoiseOff();
        }
    }
}
