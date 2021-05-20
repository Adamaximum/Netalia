using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicAssignments : MonoBehaviour
{
   private AudioSource audio;
   public AudioClip music;

   private void Awake()
   {
      audio = gameObject.GetComponent<AudioSource>();
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.gameObject.tag == "Player" && MusicManager.Instance.currentClip != music)
      {
         MusicManager.Instance.ChangeTracks(audio, music);
      }
   }
}
