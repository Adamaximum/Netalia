using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovementTest : MonoBehaviour
{

    [HideInInspector]
    public Rigidbody2D rb;
    private AnimationScript anim;
    private bool canGrabWalls;

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

    //singleton
    public static MovementTest Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<AnimationScript>();
    }


    void Update()
    {
        //NOTES TO SELF - Kyra
        //wallGrab bool can be edited out, redundant
        //change to singleton for easier access
        //separate particle effects into self-contained functions
        //animation controls are inefficient, reduce and simplify
        //you're losing readability, take a weekend off to properly edit and comment this sometime
        
        //controller input
        FetchInputs();

        if (!canMove)
            return;

        //set animation state
        anim.SetHorizontalMovement(x, y, rb.velocity.y);
        
        //check for wall
        DetectWallCollisions();
        
        //KYRA c'mon man this is embarrassing, fix this ASAP
        //detach from wall if wall-facing key is pressed
        if ((Collision.Instance.onLeftWall && Input.GetButtonDown("WallDetachLeft"))
            || (Collision.Instance.onRightWall && Input.GetButtonDown("WallDetachRight")))
        {
            canGrabWalls = !canGrabWalls;
            StartCoroutine(DisableMovement(.1f));
        }
        else
        {
            SetClimbingState(Collision.Instance.onWall);
        }

        //jump
        if (Collision.Instance.onGround && Input.GetButtonDown("Jump"))
        {
            anim.SetTrigger("jump");
            Jump(Vector2.up, false);
        }

        //move in x direction
        if (!Collision.Instance.onGround && Collision.Instance.onLeftWall && xRaw > 0)
            WallJump(Vector2.right);
        else if (!Collision.Instance.onGround && Collision.Instance.onRightWall && xRaw < 0 )
            WallJump(Vector2.left);
        
        HorizontalMovement();

        //reset bools and play particle effect on touchdown
        DetectGround();

        //if (Input.GetButton("Jump"))
            //HeightBoost();

        WallParticle(y);
        
        //set sprite direction
        FixAnim();
    }

    //version where player sticks to wall unless Y is pressed
    
    void HorizontalMovement()
    {
        if (Collision.Instance.onGround)
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
        if (Collision.Instance.onGround)
        {
            wallJumped = false;
            wallGrab = false;
            canGrabWalls = true;
        }
        
        //touchdown visuals on ground touch
        if (Collision.Instance.onGround && !groundTouch)
        {
            GroundTouch();
            groundTouch = true;
        }

        //reset groundTouch bool
        else if(!Collision.Instance.onGround && groundTouch)
        {
            groundTouch = false;
        }
    }
    
    void DetectWallCollisions()
    {
        //this is pretty much exclusively animation controls now. Can delete in V3
        
        if (Collision.Instance.onWall && !Collision.Instance.onGround && canMove && canGrabWalls)
        {
            if (side != Collision.Instance.wallSide)
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
        //create separate handler for rigidbody controls, separate out animation controls
        
        if (onWall && canGrabWalls)
        {
            //set new rigidbody behavior
            rb.gravityScale = 0;
            if(x > .2f || x < -.2f)
                rb.velocity = new Vector2(rb.velocity.x, 0);
            
            //
            float speedModifier = y > 0 ? .5f : 1;
            if (Collision.Instance.wallSide == 1 && Input.GetAxis("Horizontal") > 0 || Collision.Instance.wallSide == -1 && Input.GetAxis("Horizontal") < 0)
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
        if (!Collision.Instance.onWall && !Collision.Instance.onGround && checkForBoost && !wallJumped)
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
    private void WallJump(Vector2 wallDir)
    {
        PlayerAudioScript.Instance.JumpSound();
        
        side *= -1;
        anim.Flip(side);

        StopCoroutine(DisableMovement(0));
        StartCoroutine(DisableMovement(.1f));

        Vector2 jumpDir = (Vector2.up / wallJumpForce.y) + (wallDir / wallJumpForce.x);
        //Vector2 jumpDir = (Vector2.up) + (wallDir);

        Jump(jumpDir, true);

        wallJumped = true;

    }
    
    private void Jump(Vector2 dir, bool wall)
    {
        //set particle values
        slideParticle.transform.parent.localScale = new Vector3(ParticleSide(), 1, 1);
        ParticleSystem particle = wall ? wallJumpParticle : jumpParticle;
        
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
        int particleSide = Collision.Instance.onRightWall ? 1 : -1;
        return particleSide;
    }
}
