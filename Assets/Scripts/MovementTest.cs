using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovementTest : MonoBehaviour
{
    private Collision coll;
    [HideInInspector]
    public Rigidbody2D rb;
    private AnimationScript anim;

    [Space]
    [Header("Stats")]
    public float speed = 10;
    public float jumpForce = 50;
    public float slideSpeed = 5;
    public float wallJumpLerp = 10;
    public float dashSpeed = 20;
    public Vector2 wallJumpForce = new Vector2 (1.5f, 1.5f);

    [Space]
    [Header("Booleans")]
    public bool canMove;
    public bool wallGrab;
    public bool wallJumped;
    //public bool wallSlide;
    public bool isDashing;

    [Space]

    private bool groundTouch;
    private bool hasDashed;

    public int side = 1;

    [Space]
    [Header("Polish")]
    public ParticleSystem dashParticle;
    public ParticleSystem jumpParticle;
    public ParticleSystem wallJumpParticle;
    public ParticleSystem slideParticle;
    
    //input controls
    float x, y, xRaw, yRaw;
    Vector2 dir;

    void Start()
    {
        coll = GetComponent<Collision>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<AnimationScript>();
    }


    void Update()
    {
        //NOTES TO SELF - Kyra
        //wallGrab bool can be edited out, redundant
        //change to singleton for easier access
        //separate particle effects into self-contained functions
        
        //controller input
        FetchInputs();

        if (!canMove)
            return;

        //set animation state
        anim.SetHorizontalMovement(x, y, rb.velocity.y);

        //if on wall, vertical movement only
        SetClimbingState(DetectWallCollisions());

        //jump controls
        if (Input.GetButtonDown("Jump"))
        {
            anim.SetTrigger("jump");
            
            if (coll.onWall && !coll.onGround)
                WallJump();
            
            else if (!coll.onWall && coll.onGround)
                Jump(Vector2.up, false);
        }

        //move in x direction
        HorizontalMovement();
        
        //reset bools and play particle effect on touchdown
        DetectGround();

        WallParticle(y);
        
        //set side var to check side of wall grabbed
        CheckWallSide();
    }

    void HorizontalMovement()
    {
        if (!wallJumped)
        {
            Vector2 test = new Vector2(dir.x * speed, rb.velocity.y);
            rb.velocity = test;
        }
        else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(dir.x * speed, rb.velocity.y)), wallJumpLerp * Time.deltaTime);
        }
    }
    
    void FetchInputs()
    {
        //note: streamline later
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
        xRaw = Input.GetAxisRaw("Horizontal");
        yRaw = Input.GetAxisRaw("Vertical");
        dir = new Vector2(xRaw, yRaw);
    }

    void DetectGround()
    {
        //reset wallJumped bool
        if (coll.onGround)
        {
            wallJumped = false;
        }
        
        //touchdown visuals on ground touch
        if (coll.onGround && !groundTouch)
        {
            GroundTouch();
            groundTouch = true;
        }

        //reset groundTouch bool
        else if(!coll.onGround && groundTouch)
        {
            groundTouch = false;
        }
    }
    
    bool DetectWallCollisions()
    {
        if (coll.onWall && canMove)
        {
            if (side != coll.wallSide)
                anim.Flip(side * -1);

            wallGrab = true;
            return true;
        }
        else
        {
            wallGrab = false;
            return false;
        }
    }

    void SetClimbingState(bool onWall)
    {
        if (onWall)
        {
            //set new rigidbody behavior
            rb.gravityScale = 0;
            if(x > .2f || x < -.2f)
                rb.velocity = new Vector2(rb.velocity.x, 0);
            
            //
            float speedModifier = y > 0 ? .5f : 1;
            if (coll.wallSide == 1 && Input.GetAxis("Horizontal") > 0 || coll.wallSide == -1 && Input.GetAxis("Horizontal") < 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, y * (speed * speedModifier));
            }
            else
            {
                rb.velocity = new Vector2(0, y * (speed * speedModifier));
            }
        }
        else
        {
            rb.gravityScale = 3;
        }
        
    }

    void GroundTouch()
    {
        //fix sprite direction
        side = anim.sr.flipX ? -1 : 1;

        //particle effect - landing
        jumpParticle.Play();
    }
    private void WallJump()
    {
        if ((side == 1 && coll.onRightWall) || side == -1 && !coll.onRightWall)
        {
            side *= -1;
            anim.Flip(side);
        }

        StopCoroutine(DisableMovement(0));
        StartCoroutine(DisableMovement(.1f));

        Vector2 wallDir = coll.onRightWall ? Vector2.left : Vector2.right;
        Vector2 jumpDir = (Vector2.up / wallJumpForce.y) + wallDir / wallJumpForce.x;
        //Vector2 tempDir = new Vector2(wallDir.x, 20);
        Jump(jumpDir, true);
        //Jump(tempDir, true);

        wallJumped = true;

        Debug.Log(jumpDir);
    }

    private void Jump(Vector2 dir, bool wall)
    {
        //set particle values
        slideParticle.transform.parent.localScale = new Vector3(ParticleSide(), 1, 1);
        ParticleSystem particle = wall ? wallJumpParticle : jumpParticle;
        Debug.Log(dir * jumpForce);
        
        //begin jump
        rb.AddForce(dir * jumpForce*50);
        
        particle.Play();
        
    }

    void CheckWallSide()
    {
        if (wallGrab || !canMove)
            return;

        if(x > 0)
        {
            side = 1;
            anim.Flip(side);
        }
        if (x < 0)
        {
            side = -1;
            anim.Flip(side);
        }
    }

    IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    void RigidbodyDrag(float x)
    {
        rb.drag = x;
    }

    void WallParticle(float vertical)
    {
        var main = slideParticle.main;

        if (wallGrab && vertical < 0)
        {
            slideParticle.transform.parent.localScale = new Vector3(ParticleSide(), 1, 1);
            main.startColor = Color.white;
        }
        else
        {
            main.startColor = Color.clear;
        }
    }

    int ParticleSide()
    {
        int particleSide = coll.onRightWall ? 1 : -1;
        return particleSide;
    }
}
