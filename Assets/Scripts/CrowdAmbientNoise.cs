using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdAmbientNoise : MonoBehaviour
{

    private AudioSource audio;

    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    public void NoiseOn()
    {
        audio.mute = false;
    }

    public void NoiseOff()
    {
        audio.mute = true;
    }
}
