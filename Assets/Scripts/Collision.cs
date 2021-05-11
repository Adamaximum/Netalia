using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{

    [Header("Layers")]
    public LayerMask groundLayer;
    public LayerMask NPCLayer;
    public LayerMask enemyLayer;

    [Space]

    //wall collisions
    public bool onGround;
    public bool onWall;
    public bool onRightWall;
    public bool onLeftWall;
    public int wallSide;
    
    //edge collisions
    public bool onWallEdge;
    private bool leftEdge, rightEdge;
    
    //player interaction
    public bool interact;
    public bool hit;
    public int roomNum;
    
    [Space]

    [Header("Collision")]

    public float collisionRadius = 0.25f;
    public Vector2 bottomOffset, rightOffset, leftOffset;
    public Vector2 leftEdgeDetector, rightEdgeDetector;
    private Color debugCollisionColor = Color.red;

    public static Collision Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        roomNum = 0;
    }

    void Start()
    {
        groundLayer = LayerMask.GetMask("Walls");
    }

    // Update is called once per frame
    void Update()
    {  
        //check for wall collisions
        onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer);

        onRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer);
        onLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);

        if (onRightWall || onLeftWall)
            onWall = true;
        else
            onWall = false;

        wallSide = onRightWall ? -1 : 1;
        
        //check to see if player is on wall edge
        leftEdge = Physics2D.OverlapCircle((Vector2)transform.position + leftEdgeDetector, collisionRadius, groundLayer);
        rightEdge = Physics2D.OverlapCircle((Vector2)transform.position + rightEdgeDetector, collisionRadius, groundLayer);

        if (!leftEdge && onLeftWall)
            onWallEdge = true;
        else if (!rightEdge && onRightWall)
            onWallEdge = true;
        else
            onWallEdge = false;
        
        Debug.Log(onWallEdge);
        
        //check for NPC or item interactions
        interact = Physics2D.OverlapCircle((Vector2)transform.position, rightOffset.x, NPCLayer);
        
        //enemy contact
        hit = Physics2D.OverlapCircle((Vector2)transform.position, rightOffset.x, enemyLayer);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        var positions = new Vector2[] { bottomOffset, rightOffset, leftOffset };

        Gizmos.DrawWireSphere((Vector2)transform.position  + bottomOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftEdgeDetector, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightEdgeDetector, collisionRadius);
        
        //interaction sphere
        Gizmos.DrawWireSphere((Vector2)transform.position, rightOffset.x);
    }
}
