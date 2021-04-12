using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    private Animator anim;
    private Collision coll;
    [HideInInspector]
    public SpriteRenderer sr;

    void Start()
    {
        anim = GetComponent<Animator>();
        coll = GetComponentInParent<Collision>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Debug.Log(Movement.Instance);
        
        anim.SetBool("onGround", coll.onGround);
        anim.SetBool("onRightWall", coll.onRightWall);
        anim.SetBool("wallGrab", coll.onWall);
        anim.SetBool("wallSlide", MovementTest.Instance.wallSlide);
        anim.SetBool("canMove", MovementTest.Instance.canMove);
        anim.SetBool("idle", GameManager.Instance.idle);
        //anim.SetBool("isDashing", move.isDashing);
    }

    public void SetHorizontalMovement(float x,float y, float yVel)
    {
        anim.SetFloat("HorizontalAxis", x);
        anim.SetFloat("VerticalAxis", y);
        anim.SetFloat("VerticalVelocity", yVel);
    }

    public void SetTrigger(string trigger)
    {
        anim.SetTrigger(trigger);
    }
    

    public void Flip(bool side)
    {
        sr.flipX = side;
    }
}
