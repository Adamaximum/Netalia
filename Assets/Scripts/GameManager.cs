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
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        Debug.Log(Collision.Instance.hit);

        if (Collision.Instance.hit)
        {

            Reset();
        }
    }

    void Reset()
    {
        player.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        Movement.Instance.enabled = false;
        
        //place player at checkpoint and freeze it
        player.transform.position = checkpoint.transform.position;
        
        //cover screen
        mask.Transition();
    }
}
