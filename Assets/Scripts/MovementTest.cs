using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Experimental.PlayerLoop;

public class MovementTest : MonoBehaviour
{
    //player components
    private Rigidbody2D rb;
    
    //particle systems
    public ParticleSystem jumpParticle;
    public ParticleSystem wallJumpParticle;
    public ParticleSystem slideParticle;
    
    //player bools
    public bool canMove = true;

    //physics
    public float speed;
    public float jumpForce;
    public Vector2 wallJumpForce;

    //controller inputs
    private float x, y, xRaw, yRaw;
    private Vector2 dir;
    
    public static MovementTest Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //NOTES:
        //find better way to handle gravity changes
        
        //get controller input
        UpdateControllerInputs();
        
        //see if player is on wall
        if (Collision.Instance.onGround)
        {
            AnimationState(true);
        }
        
        if (Collision.Instance.onWall)
        {
            rb.gravityScale = 0;
            ClimbWall();
            AnimationState(false);
        }
        else
        {
            rb.gravityScale = 3;
        }

        //check for wall jump
        if (!Collision.Instance.onGround && Collision.Instance.onLeftWall && xRaw > 0)
            WallJump(Vector2.right);
        else if (!Collision.Instance.onGround && Collision.Instance.onRightWall && xRaw < 0 )
            WallJump(Vector2.left);

        //if yes, allow player to let go of wall
    }

    void FixedUpdate()
    {
        //
        
        HorizontalMovement();
    }

    void UpdateControllerInputs()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
        xRaw = Input.GetAxisRaw("Horizontal");
        yRaw = Input.GetAxisRaw("Vertical");
        dir = new Vector2(xRaw, yRaw);
    }


    void ClimbWall()
    {
        float speedModifier = y > 0 ? .5f : 1;
        rb.velocity = new Vector2(0, y * (speed * speedModifier));
    }

    void AnimationState(bool grounded)
    {
        
    }

    void HorizontalMovement()
    {
        Vector2 test = new Vector2(dir.x * speed, rb.velocity.y);
        rb.velocity = test;
    }

    void WallJump(Vector2 dir)
    {
        //play jump sound
        PlayerAudioScript.Instance.JumpSound();
        
        StopCoroutine(DisableMovement(0));
        StartCoroutine(DisableMovement(.1f));
        
        //play particle effect
        wallJumpParticle.Play();
        
        //jump
        Vector2 jumpDir = (Vector2.up * wallJumpForce.y) + (dir * wallJumpForce.x);
        rb.AddForce(jumpDir*jumpForce, ForceMode2D.Impulse);

    }


    void Jump()
    {
        
    }
    
    IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }
}
