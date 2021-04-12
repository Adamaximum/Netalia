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
    private bool jumped;
    private bool checkForBoost;
    private bool groundTouch;

    //controller inputs
    private float x, y, xRaw, yRaw;
    private Vector2 dir;
    
    //animation
    public AnimationScript anim;
    public AnimationScript silhouetteAnim;
    public bool wallGrab;
    public bool wallSlide;

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
        //wall jump is short boost. wall jump + jump key is long boost.
        //assign particle effects and anim through script in v4
        
        //get controller input
        UpdateControllerInputs();

        if (!canMove)
            return;
        
        DetectGround();
        
        //control animation
        AnimationState();

        //allow player to jump when not on wall
        if (Collision.Instance.onGround && Input.GetButtonDown("Jump"))
            Jump();

        //if player is on wall, allow climb
        if (Collision.Instance.onWall)
        {
            rb.gravityScale = 0;
            
            ClimbWall();

            if (yRaw < 0)
                ParticlePlay(slideParticle);

        }
        else
        {
            rb.gravityScale = 3;
        }

        //check for wall jump, and what type
        if (Collision.Instance.onLeftWall && xRaw > 0)
            WallJump(Vector2.right);
        else if (Collision.Instance.onRightWall && xRaw < 0 )
            WallJump(Vector2.left);

    }

    void FixedUpdate()
    {
        if (!Collision.Instance.onWall)
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
        //flip player sprite
        anim.Flip(Collision.Instance.onRightWall);
        silhouetteAnim.Flip(Collision.Instance.onRightWall);
        
        //control climbing physics
        float speedModifier = y > 0 ? .5f : 1;
        rb.velocity = new Vector2(0, y * (speed * speedModifier));

    }

    void AnimationState()
    {
        anim.SetHorizontalMovement(x, y, rb.velocity.y);
        silhouetteAnim.SetHorizontalMovement(x, y, rb.velocity.y);

        
        if (xRaw < 0)
        {
            anim.Flip(true);
            silhouetteAnim.Flip(true);
        }

        if (xRaw > 0)
        {
            anim.Flip(false);
            silhouetteAnim.Flip(false);
        }
    }

    void HorizontalMovement()
    {
        Vector2 test = new Vector2(dir.x * speed, rb.velocity.y);
        rb.velocity = test;
    }

    void DetectGround()
    {
        //reset bools
        if (Collision.Instance.onGround)
        {
            jumped = false;
            wallGrab = false;
            checkForBoost = false;
        }
        
        //particles and sound on landing
        if (Collision.Instance.onGround && !groundTouch)
        {
            //play landing particles
            jumpParticle.Play();
            
            //landing audio
            PlayerAudioScript.Instance.LandSound();
            
            groundTouch = true;
        }

        //reset groundTouch bool
        else if (!Collision.Instance.onGround && groundTouch)
        {
            groundTouch = false;
        }
    }

    void WallJump(Vector2 dir)
    {
        //play jump sound
        PlayerAudioScript.Instance.JumpSound();
        
        //temp disable movement
        StopCoroutine(DisableMovement(0));
        StartCoroutine(DisableMovement(.1f));
        
        //play particle effect
        ParticlePlay(wallJumpParticle);
        
        //animate
        JumpAnim();
        
        //jump
        Vector2 jumpDir = (Vector2.up / wallJumpForce.y) + (dir / wallJumpForce.x);
        rb.AddForce(jumpDir*jumpForce, ForceMode2D.Impulse);

    }

    void Jump()
    {
        JumpAnim();
        
        //signal that player has jumped
        jumped = true;

        //add force
        rb.AddForce(Vector2.up*jumpForce, ForceMode2D.Impulse);
    }

    void JumpAnim()
    {
        //animation
        anim.SetTrigger("jump");
        silhouetteAnim.SetTrigger("jump");
    }

    void ParticlePlay(ParticleSystem particle)
    {
        int particleSide = Collision.Instance.onRightWall ? 1 : -1;
        particle.transform.parent.localScale = new Vector3(particleSide, 1, 1);
        particle.Play();
    }


    IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }
}