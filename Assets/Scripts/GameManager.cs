using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public BoxCollider2D checkpoint;
    public GameObject player;

    public MaskScript mask;

    //temp fixes, resolve these later
    public bool idle;
    public SpriteRenderer visualSprite;
    public SpriteRenderer silhouetteSprite;

    public RoomTrigger[] rooms;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        rooms = new RoomTrigger[50];
    }
    
    
    void Update()
    {
        if (Collision.Instance.hit)
        {
            Reset();
        }
    }

    void Reset()
    {
        visualSprite.color = new Color(1, 1, 1, 0);
        silhouetteSprite.color = new Color(1, 1, 1, 0);
        
        DisablePlayer();

        //place player at checkpoint and freeze it
        player.transform.position = checkpoint.transform.position;
        
        //cover screen
        mask.Transition();
    }
    
    public void EndReset()
    {
        visualSprite.color = new Color(1, 1, 1, 1);
        silhouetteSprite.color = new Color(1, 1, 1, 1);
    }

    public void DisablePlayer()
    {
        idle = true;
        Movement.Instance.rb.velocity = new Vector2(0, 0);
        Movement.Instance.enabled = false;
    }

    public void EnablePlayer()
    {
        //restart enemies
        rooms[Collision.Instance.roomNum].ActivateRoom();
        Debug.Log("reset room " + Collision.Instance.roomNum);
        
        //turn player back on
        Movement.Instance.enabled = true;
        idle = false;
    }
}
