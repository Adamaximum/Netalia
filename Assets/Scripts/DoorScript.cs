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
    
    void Start()
    {
        collider = gameObject.GetComponent<Collider2D>();
        anim = gameObject.GetComponent<Animator>();

        detectionRange = 4;
        detectionOrigin = new Vector2(transform.position.x - (detectionRange/2), transform.position.y - 0.4f);
    }

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(detectionOrigin, Vector2.right, detectionRange, LayerMask.GetMask("Player"));
        Debug.DrawRay(detectionOrigin, Vector2.right*detectionRange, Color.red);

        if (hit != null && hit.collider.tag == "Player")
        {
            Debug.Log("player raycast");
            if (UnlockConditions())
            {
                UnlockDoor();
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

    private void UnlockDoor()
    {
       anim.SetTrigger("Unlock");
       collider.enabled = false;
    }
}
