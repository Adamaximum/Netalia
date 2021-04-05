﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemyX : MonoBehaviour
{
    //NOTE: endCoords value needs a greater x-value than the startCoords
    
    public Vector2 startCoords, endCoords;
    public float speed;

    private Rigidbody2D rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        gameObject.transform.position = startCoords;
        Unfreeze();
    }

    
    void Update()
    {
        if (gameObject.transform.position.x < startCoords.x)
            MoveToEndCoords();
        
        else if (gameObject.transform.position.x > endCoords.x)
            MoveToStartCoords();
    }

    void MoveToStartCoords()
    {
        Vector2 dir = startCoords - endCoords;
        rb.velocity = dir * speed;
    }

    void MoveToEndCoords()
    {
        Vector2 dir = endCoords - startCoords;
        rb.velocity = dir * speed;
    }

    public void Freeze()
    {
        Debug.Log("freeze");
        rb.velocity = new Vector2(0, 0);
        gameObject.transform.position = startCoords;
    }

    public void Unfreeze()
    {
        rb.velocity = speed * (startCoords - endCoords);
    }

}