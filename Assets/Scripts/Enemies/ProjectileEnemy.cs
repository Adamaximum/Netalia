using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemy : MonoBehaviour
{
    //on awake room assignment
    private bool assignedRoom;
    
    public GameObject projectile;
    public float bulletSpeed;
    public float fireRate;
    private bool readyToFire = true;

    //audio
    private AudioSource audio;
    private AudioClip fireSound;
    
    //anim
    private Animator anim;
    private Animation fireAnim;

    //for bullet instantiation calculations
    private float radius;

    void Awake()
    {
        DetectRoom();
    }
    
    void Start()
    {
        radius = GetComponent<CircleCollider2D>().radius;

        audio = GetComponent<AudioSource>();
        fireSound = GetComponent<AudioSource>().clip;

        anim = GetComponent<Animator>();
        
    }

    
    void Update()
    {
        if (readyToFire)
            StartCoroutine(PauseThenFire());
    }

    private void DetectRoom()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.25f, 9);

        if (hit.collider.gameObject.tag == "RoomTrigger")
        {
            RoomTrigger room = hit.collider.gameObject.GetComponent<RoomTrigger>();
            room.roomProjEnemies.Add(this);
        }
    }

    IEnumerator PauseThenFire()
    {
        readyToFire = false;
        yield return new WaitForSeconds(fireRate);
        Fire();
        readyToFire = true;
    }

    void Fire()
    {
        for (int i = 0; i < 5; i++)
        {
            CreateProjectile(FetchBulletCoords(i));
        }
        
        PlayFireAnim();

        PlayFireSound();
    }

    void CreateProjectile(Vector3 pos)
    {
        GameObject bullet = Instantiate(projectile, pos, Quaternion.identity);
        
        Vector2 dir = new Vector2(pos.x - gameObject.transform.position.x, pos.y - gameObject.transform.position.y);
        bullet.GetComponent<Rigidbody2D>().velocity = dir*bulletSpeed;
    }

    Vector3 FetchBulletCoords(int bulletNum)
    {
        Vector3 distFromCenter;
        float degreesToRadians;

        degreesToRadians = (float)(bulletNum*72 * (Math.PI / 180));
        distFromCenter = new Vector3(radius*Mathf.Cos(degreesToRadians), radius*Mathf.Sin(degreesToRadians), 0);

        Vector3 finalCoords;
        finalCoords = gameObject.transform.position + distFromCenter;
        return finalCoords;
    }

    private void PlayFireAnim()
    {
        anim.SetTrigger("Fire");
    }

    void PlayFireSound()
    {
        audio.PlayOneShot(fireSound);
    }
}
