using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemy : MonoBehaviour
{
    //NOTE: right now, the endCoords value needs a greater x-value than the startCoords
    
    public Vector2 startCoords, endCoords;
    public float speed;

    private Rigidbody2D rb;

    private Vector2 dir;
    private Vector2 xBounds, yBounds;
    
    void Start()
    {
        if (startCoords.x <= endCoords.x)
        {
            xBounds.x = startCoords.x;
            xBounds.y = endCoords.x;
        }
        else
        {
            xBounds.x = endCoords.x;
            xBounds.y = startCoords.x;
        }
        
        if (startCoords.y <= endCoords.y)
        {
            yBounds.x = startCoords.y;
            yBounds.y = endCoords.y;
        }
        else
        {
            yBounds.x = endCoords.y;
            yBounds.y = startCoords.y;
        }
        
        Debug.Log(xBounds);

        //start moving enemy
        rb = GetComponent<Rigidbody2D>();
        dir = startCoords - endCoords;
        rb.velocity = speed * dir;
    }

    
    void Update()
    {
        if (gameObject.transform.position.x < xBounds.x || gameObject.transform.position.x > xBounds.y)
            ChangeDirection();
        
        else if (gameObject.transform.position.y < yBounds.x || gameObject.transform.position.y > yBounds.y)
            ChangeDirection();
    }

    void ChangeDirection()
    {
        dir = dir*-1;
        rb.velocity = dir * speed;
    }

    /*
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
    */

}
