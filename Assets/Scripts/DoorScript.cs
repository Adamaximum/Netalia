using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public int[] npcsToTalkTo;
    
    private Collider2D collider;
    private Animator anim;

    private float detectionRange;
    private Vector2 detectionOrigin;
    private AudioSource audio;
    private bool unlock;
    
    void Start()
    {
        collider = gameObject.GetComponent<Collider2D>();
        anim = gameObject.GetComponent<Animator>();
        audio = gameObject.GetComponent<AudioSource>();

        detectionRange = 5;
        float castHeight = transform.position.y - transform.localScale.y + (Collision.Instance.gameObject.transform.localScale.y/2);
        detectionOrigin = new Vector2(transform.position.x - (detectionRange/2), castHeight);
        unlock = false;
    }

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(detectionOrigin, Vector2.right, detectionRange, LayerMask.GetMask("Player"));
        Debug.DrawRay(detectionOrigin, Vector2.right*detectionRange, Color.red);

        if (hit != null)
        {
            try
            {
                if (hit.collider.tag == "Player" && UnlockConditions() && !unlock)
                {
                    anim.SetTrigger("Unlock");
                    audio.PlayOneShot(audio.clip);
                    StartCoroutine(UnlockDoor());
                    unlock = true;
                }
            }
            catch (NullReferenceException error)
            {
                
            }
        }
    }

    private bool UnlockConditions()
    {
        for (int i = 0; i < npcsToTalkTo.Length; i++)
        {
            if (!PlayerConditionTracker.Instance.spokeToNPCs[npcsToTalkTo[i]])
            {
                return false;
            }
        }

        return true;
    }

    private IEnumerator UnlockDoor()
    {
       yield return new WaitForSeconds(1);
       collider.enabled = false;
    }
}
