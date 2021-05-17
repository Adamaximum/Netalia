using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    private AudioSource audio;
    public AudioClip currentClip;

    [Header("Mixer Values")]
    public AudioMixer mixer;

    public static MusicManager Instance { get; private set; }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        audio = GetComponent<AudioSource>();
    }

    
    public void ChangeTracks(AudioClip music)
    {
        StartCoroutine(FadeMixerGroup.StartFade(mixer, "MusicVolume", 2, -80f)); 
        StartCoroutine(StartNewTrack(music));
        currentClip = music;
    }
    private IEnumerator StartNewTrack(AudioClip nextTrack)
    {
        yield return new WaitForSeconds(2.5f);
        audio.clip = nextTrack;
        audio.Play();
        mixer.SetFloat("MusicVolume", 0);
    }
}
