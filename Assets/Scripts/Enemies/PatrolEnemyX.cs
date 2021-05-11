using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemyX : MonoBehaviour
{
    //NOTE: endCoords value needs a greater x-value than the startCoords
    
    public Vector2 startCoords, endCoords;
    private Vector2 restartCoords;
    public float speed;

    private Rigidbody2D rb;
    
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
        if (gameObject.transform.position.x < startCoords.x)
            MoveToEndCoords();
        
        else if (gameObject.transform.position.x > endCoords.x)
            MoveToStartCoords();
    }
    
    private void DetectRoom()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.25f, LayerMask.GetMask("Rooms"));

        Debug.Log(hit.collider.name);
        RoomTrigger room = hit.transform.gameObject.GetComponent<RoomTrigger>();
        room.roomPatrolsX.Add(this);
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