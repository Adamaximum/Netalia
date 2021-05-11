using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject checkpoint;
    public GameObject player;

    public MaskScript mask;

    public bool controllerConnected;
    
    //backgrounds
    public GameObject sewerBG;
    public GameObject drainPipeBG;
    public GameObject cityBG;
    public string currentBG;

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
        
        //find better solution for this later
        sewerBG.SetActive(true);
        drainPipeBG.SetActive(false);
        cityBG.SetActive(false);
    }
    
    
    void Update()
    {
        if (Collision.Instance.hit)
        {
            Reset();
        }

        if (Time.frameCount % 10 == 0)
        {
            CheckForController();
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
        //restart enemies
        rooms[Collision.Instance.roomNum].DeactivateRoom();
        
        idle = true;
        //MovementTest.Instance.rb.velocity = new Vector2(0, 0);
        Movement.Instance.enabled = false;
    }

    public void EnablePlayer()
    {
        //restart enemies
        rooms[Collision.Instance.roomNum].ActivateRoom();
        
        //turn player back on
        Movement.Instance.enabled = true;
        idle = false;
    }

    public void ChangeBackground(string nextBG)
    {
        //deactivate current bg
        if (currentBG == "Sewer")
        {
            sewerBG.SetActive(false);
        }
        else if (currentBG == "DrainPipe")
        {
            drainPipeBG.SetActive(false);
        }
        else if (currentBG == "City")
        {
            cityBG.SetActive(false);
        }

        //activate new bg
        if (nextBG == "Sewer")
        {
            sewerBG.SetActive(true);
        }
        else if (nextBG == "DrainPipe")
        {
            drainPipeBG.SetActive(true);
        }
        else if (nextBG == "City")
        {
            cityBG.SetActive(true);
        }
        
        //set new currentBG var
        currentBG = nextBG;
    }

    private void CheckForController()
    {
        string[] joysticks = Input.GetJoystickNames();

        if (joysticks.Length > 0)
        {
            for(int i =0; i < joysticks.Length; ++i)
            {
                //Check if the string is empty or not
                if(!string.IsNullOrEmpty(joysticks[i]))
                {
                    //Not empty, controller temp[i] is connected
                    controllerConnected = true;
                }
                else
                {
                    //If it is empty, controller i is disconnected
                    //where i indicates the controller number
                    controllerConnected = false;
                }
            }
        }
    }

}
