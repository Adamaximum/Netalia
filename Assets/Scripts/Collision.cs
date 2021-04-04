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
    
    //player interaction
    public bool interact;
    public bool hit;
    
    [Space]

    [Header("Collision")]

    public float collisionRadius = 0.25f;
    public Vector2 bottomOffset, rightOffset, leftOffset;
    private Color debugCollisionColor = Color.red;

    public static Collision Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
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
        
        //interaction sphere
        Gizmos.DrawWireSphere((Vector2)transform.position, rightOffset.x);
    }
}
