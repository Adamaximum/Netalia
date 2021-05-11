using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemyY : MonoBehaviour
{
    //NOTE: endCoords value needs a greater y-value than the startCoords
    
    public Vector2 startCoords, endCoords;
    private Vector2 restartCoords;
    public float speed;

    private Rigidbody2D rb;
    private Vector2 xBounds, yBounds;
    
    void Awake()
    {
        DetectRoom();
    }
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        restartCoords = gameObject.transform.position;
        Unfreeze();
    }

    
    void Update()
    {
        Debug.DrawRay(transform.position, Vector2.down*0.25f, Color.red);
        
        if (gameObject.transform.position.y < startCoords.y)
            MoveToEndCoords();
        
        else if (gameObject.transform.position.y > endCoords.y)
            MoveToStartCoords();
    }
    
    private void DetectRoom()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.25f, LayerMask.GetMask("Rooms"));

        RoomTrigger room = hit.collider.gameObject.GetComponent<RoomTrigger>();
        room.roomPatrolsY.Add(this);
        
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
        gameObject.transform.position = restartCoords;
    }

    public void Unfreeze()
    {
        rb.velocity = speed * (startCoords - endCoords);
    }

}