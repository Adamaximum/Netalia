﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public BoxCollider2D checkpoint;
    public GameObject player;

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
            Reset();
    }

    void Reset()
    {
        //place player at checkpoint
        player.transform.position = checkpoint.transform.position;
    }
}