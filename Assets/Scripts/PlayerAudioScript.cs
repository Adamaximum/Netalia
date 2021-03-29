﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioScript : MonoBehaviour
{
    public static PlayerAudioScript Instance { get; private set; }
    private AudioSource source;

    //audio clips
    public AudioClip jump;
    public AudioClip land;
    
    void Start()
    {
        if (Instance == null)
            Instance = this;

        source = gameObject.GetComponent<AudioSource>();
    }

    public void JumpSound()
    {
        source.PlayOneShot(jump);
    }

    public void LandSound()
    {
        source.PlayOneShot(land);
    }
}
