using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioScript : MonoBehaviour
{
    public static PlayerAudioScript Instance { get; private set; }
    private AudioSource source;

    //audio clips
    public AudioClip jump;
    public AudioClip land;
    public AudioClip slide;

    private bool playOnce;
    
    void Start()
    {
        if (Instance == null)
            Instance = this;

        source = gameObject.GetComponent<AudioSource>();
    }

    private void Update()
    {
        //SlideSound();
    }

    public void JumpSound()
    {
        source.PlayOneShot(jump);
    }

    public void LandSound()
    {
        if (!source.isPlaying)
            source.PlayOneShot(land);
    }

    public void SlideSound()
    {
        if (Movement.Instance.wallSlide && playOnce)
        {
            source.PlayOneShot(slide);
            playOnce = false;
        }
        
    }
}
