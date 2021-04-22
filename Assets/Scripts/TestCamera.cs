using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamera : MonoBehaviour
{
    public Transform roomPos;

    private Transform player;
    private Vector2 stagger = new Vector2(8, 15);
    private bool update = true;
    
    void Start()
    {
        player = GameObject.Find("Player").transform;
    }
    
    void Update()
    {
        this.transform.position = newCameraPos(PlayerPosDelta());
    }

    public void Switch()
    {
        this.transform.position = roomPos.position;
    }

    Vector2 PlayerPosDelta()
    {
        Vector2 playerDelta = new Vector2(player.position.x - roomPos.position.x, player.position.y - roomPos.position.y);
        return playerDelta;
    }

    Vector3 newCameraPos(Vector2 delta)
    {
        Vector3 convertCoords = new Vector3(roomPos.position.x + (delta.x/stagger.x), roomPos.position.y + (delta.y/stagger.y), roomPos.position.z);
        return convertCoords;
    }
}
