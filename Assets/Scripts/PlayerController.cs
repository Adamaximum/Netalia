﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public int playerState;
    // 0 = Alive
    // 1 = Heaven
    // 2 = Hell

    public float playerSpeed = 0.2f;

    float horizontalInput;
    float verticalInput;
    
    float edgeLeft = -11.63f;
    float edgeRight = 11.63f;
    float edgeUp = 6.13f;
    float edgeDown = -6.13f;

    [Header ("Jump Settings")]
    public float jumpVelocity = 2;
    public bool jumpCheck;
    public float fallMultiplier = 2.5f;

    public LayerMask groundMask;

    [HideInInspector]
    public Rigidbody2D playerRB;

    SpriteRenderer playerSR;

    [Header("State Sprites")]
    public Sprite alive;
    public Sprite heaven;
    public Sprite hell;

    public bool hellContact;

    public Color[] transparentLevel;

    AudioSource source;
    public AudioClip[] soundFX;

    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();

        playerSR = GetComponent<SpriteRenderer>();

        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        MovementInput();
        GroundCheck();

        if(playerState == 0)
        {
            playerSR.sprite = alive;
            GetComponent<BoxCollider2D>().isTrigger = false;
        }
        else if (playerState == 1)
        {
            playerSR.sprite = heaven;
            GetComponent<BoxCollider2D>().isTrigger = false;
        }
        else if (playerState == 2)
        {
            playerSR.sprite = hell;
            GetComponent<BoxCollider2D>().isTrigger = true;

            if (hellContact)
            {
                playerRB.velocity = new Vector2(0f, 0f);
            }
        }
    }

    void MovementInput()
    {

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (transform.position.x > edgeLeft)
            {
                horizontalInput = -playerSpeed;
            }
            else
            {
                horizontalInput = 0;
            }
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (transform.position.x < edgeRight)
            {
                horizontalInput = playerSpeed;
            }
            else
            {
                horizontalInput = 0;
            }
        }
        else
        {
            horizontalInput = 0;
        }

        if (playerState == 2 && hellContact)
        {
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                if (transform.position.y > edgeDown)
                {
                    verticalInput = -playerSpeed;
                }
                else
                {
                    verticalInput = 0;
                }
            }
            else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                if (transform.position.y < edgeUp)
                {
                    verticalInput = playerSpeed;
                }
                else
                {
                    verticalInput = 0;
                }
            }
            else
            {
                verticalInput = 0;
            }
        }
        else
        {
            verticalInput = 0;
        }

        transform.position += new Vector3(horizontalInput, verticalInput, 0f);

        if (playerState == 0) // Jumping is Normal
        {
            if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && jumpCheck == true)
            {
                playerRB.velocity += Vector2.up * jumpVelocity;
                source.PlayOneShot(soundFX[3]);
            }
        }
        else if (playerState == 1) // Jumping is Infinite
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                playerRB.velocity = Vector2.up * jumpVelocity;
                source.PlayOneShot(soundFX[3]);
            }
        }
        if (playerState < 2) // Fall Multiplier is not active when Hell is active
        {
            playerRB.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    void GroundCheck()
    {
        float rayCastDist = 1;
        RaycastHit2D hitLeft;
        RaycastHit2D hitRight;
        Vector2 rayPositionRight = new Vector2(transform.position.x + 0.3f, transform.position.y);
        Vector2 rayPositionLeft = new Vector2(transform.position.x - 0.3f, transform.position.y);
        hitLeft = Physics2D.Raycast(rayPositionLeft, Vector2.down, rayCastDist, groundMask);
        hitRight = Physics2D.Raycast(rayPositionRight, Vector2.down, rayCastDist, groundMask);

        if (hitLeft.collider != null || hitRight.collider != null)
        {
            jumpCheck = true;
        }
        else
        {
            jumpCheck = false;
        }

        Debug.DrawRay(rayPositionLeft, rayCastDist * Vector2.down, Color.red);
        Debug.DrawRay(rayPositionRight, rayCastDist * Vector2.down, Color.red);
    }

    private void OnTriggerEnter2D(Collider2D collision) // Trigger Enter
    {
        if (collision.gameObject.tag == "HeavenDeath" && playerState == 0)
        {
            playerState = 1;
            playerSR.color = transparentLevel[1];

            source.clip = soundFX[0];
            source.Play();
        }
        if (collision.gameObject.tag == "HellDeath" && playerState == 0)
        {
            playerState = 2;
            playerSR.color = transparentLevel[1];

            source.clip = soundFX[1];
            source.Play();
        }

        if (collision.gameObject.name == "Resurrector" && playerState > 0)
        {
            playerState = 0;
            playerSR.color = transparentLevel[0];

            source.clip = soundFX[2];
            source.Play();
        }

        if (collision.gameObject.tag == "Hazard" && playerState == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if (collision.gameObject.tag == "Angel" && playerState == 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if (collision.gameObject.tag == "Demon" && playerState == 2)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if (collision.gameObject.tag == "AllEnemy")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void OnTriggerStay2D(Collider2D collision) // Trigger Stay
    {
        if (collision.gameObject.tag == "Ground" && playerState == 2)
        {
            playerRB.gravityScale = 0;
            hellContact = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) // Trigger Exit
    {
        if (collision.gameObject.tag == "Ground" && playerState == 2)
        {
            if (playerState == 2)
            {
                playerRB.gravityScale = 1;
            }
            hellContact = false;
        }
    }
}
