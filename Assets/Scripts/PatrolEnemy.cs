using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemy : MonoBehaviour
{
    //NOTE: right now, the endCoords value needs a greater x-value than the startCoords
    
    public Vector2 startCoords, endCoords;
    public float speed;

    private Rigidbody2D rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
        rb.velocity = new Vector2(0, 0);
    }

    public void Unfreeze()
    {
        rb.velocity = speed * (startCoords - endCoords);
    }

}