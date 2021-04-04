using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemyY : MonoBehaviour
{
    //NOTE: endCoords value needs a greater y-value than the startCoords
    
    public Vector2 startCoords, endCoords;
    public float speed;

    private Rigidbody2D rb;
    private Vector2 xBounds, yBounds;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        gameObject.transform.position = startCoords;
        Unfreeze();
    }

    
    void Update()
    {
        if (gameObject.transform.position.y < startCoords.y)
            MoveToEndCoords();
        
        else if (gameObject.transform.position.y > endCoords.y)
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
        rb.velocity = new Vector2(0, 0);
    }

    public void Unfreeze()
    {
        gameObject.transform.position = startCoords;
        rb.velocity = speed * (startCoords - endCoords);
    }

}