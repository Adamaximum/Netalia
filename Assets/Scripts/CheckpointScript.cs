using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    private GameObject col;
    public bool horizontalDoorway;
    private int angle;
    private Vector2 debugDims;

    private Vector2 rayDimensions = new Vector2(0.5f, 2.5f);
    
    void Start()
    {
        col = gameObject.transform.parent.gameObject;
        angle = horizontalDoorway ? 0 : 90;
        debugDims = horizontalDoorway ? new Vector2(rayDimensions.y, rayDimensions.x) : rayDimensions;
    }

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, rayDimensions, angle, Vector2.up);

        if (hit.collider.tag == "Player")
            GameManager.Instance.checkpoint = col;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, debugDims);
    }

    /*
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
            GameManager.Instance.checkpoint = col;
    }
    */
}
