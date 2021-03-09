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
    private bool canGrabWall = true;
    private bool checkForBoost;

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

        //check for wall
        JoystickDirection();
        DetectWallCollisions();
        
        SetClimbingState(wallGrab);

        //jump controls
        if (Input.GetButtonDown("Jump"))
        {
            //can no longer check for wall boost
            checkForBoost = false;
            
            if (wallGrab)
            {
                //joystick/arrow controls
                if (xRaw == 0 && yRaw == 0)
                {
                    canGrabWall = false;
                }
                else
                {
                    WallJump();
                }
            }
            else if (!coll.onWall && coll.onGround)
                Jump(Vector2.up, false);
        }
        else
        {
            //if player hits top of wall without jumping, extra boost
            HeightBoost();
        }

        //test for wall stick
        if (Input.GetAxis("WallStick") > 0 && wallGrab)
        {
            rb.velocity = new Vector2(0, 0);
        }
        else
        {
            //move in x direction
            HorizontalMovement(wallGrab);
        }

        //reset bools and play particle effect on touchdown
        DetectGround();

        WallParticle(y);
        
        //set sprite direction
        FixAnim();
    }

    //version where player sticks to wall unless Y is pressed
    void HorizontalMovement(bool onWall)
    {
        if (onWall)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else if (!wallJumped)
        {
            Vector2 test = new Vector2(dir.x * speed, rb.velocity.y);
            rb.velocity = test;
        }
        else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(dir.x * speed, rb.velocity.y)), wallJumpLerp * Time.deltaTime);
        }
    }
    
    /*
     //version where player can auto-detach
    void HorizontalMovement(bool onWall)
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
    */
    
    void FetchInputs()
    {
        //note: streamline later
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
        xRaw = Input.GetAxisRaw("Horizontal");
        yRaw = Input.GetAxisRaw("Vertical");
        dir = new Vector2(xRaw, yRaw);
    }

    void JoystickDirection()
    {
        if (coll.onRightWall && Input.GetAxis("Horizontal") > 0 ||
            coll.onLeftWall && Input.GetAxis("Horizontal") < 0)
        {
            canGrabWall = true;
        }
    }

    void DetectGround()
    {
        //reset wallJumped bool
        if (coll.onGround)
        {
            wallJumped = false;
            wallGrab = false;
            canGrabWall = true;
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
    
    void DetectWallCollisions()
    {
        if (coll.onWall && !coll.onGround && canMove && canGrabWall)
        {
            if (side != coll.wallSide)
                anim.Flip(side * -1);

            wallGrab = true;
            checkForBoost = true;
        }
        else
        {
            wallGrab = false;
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

    void HeightBoost()
    {
        if (!wallGrab && checkForBoost)
        {
            if (yRaw > 0)
            {
                rb.AddForce(Vector2.up*5, ForceMode2D.Impulse);
                checkForBoost = false;
            }
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
        Vector2 jumpDir = (Vector2.up / wallJumpForce.y) + (wallDir / wallJumpForce.x);
        //Vector2 jumpDir = (Vector2.up) + (wallDir);
        
        Jump(jumpDir, true);

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

    void FixAnim()
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
