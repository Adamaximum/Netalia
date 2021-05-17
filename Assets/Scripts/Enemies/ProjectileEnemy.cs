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
    public float delayStart;
    
    private bool readyToFire = true;
    private bool startDelayed = false;

    //audio
    private AudioSource audio;
    private AudioClip fireSound;
    
    //anim
    private Animator anim;
    private Animation fireAnim;

    //for bullet instantiation calculations
    private float radius;

    private void Awake()
    {
        DetectRoom();
    }
    
    private void Start()
    {
        radius = GetComponent<CircleCollider2D>().radius;
        audio = GetComponent<AudioSource>();
        fireSound = GetComponent<AudioSource>().clip;
        anim = GetComponent<Animator>();

        startDelayed = false;
    }

    
    private void Update()
    {
        if (!startDelayed)
        {
            StartCoroutine(DelayStart());
            startDelayed = true;
        }

        if (readyToFire)
            StartCoroutine(PauseThenFire());
    }

    private void DetectRoom()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.25f, LayerMask.GetMask("Rooms"));

        if (hit != null)
        {
            RoomTrigger room = hit.collider.gameObject.GetComponent<RoomTrigger>();
            room.roomProjEnemies.Add(this);
        }
    }

    private IEnumerator DelayStart()
    {
        readyToFire = false;
        yield return new WaitForSeconds(delayStart);
        readyToFire = true;
    }

    private IEnumerator PauseThenFire()
    {
        readyToFire = false;
        yield return new WaitForSeconds(fireRate);
        anim.SetTrigger("Fire");
        //yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length+anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
        //Fire();
        readyToFire = true;
    }

    public void Fire()
    {
        for (int i = 0; i < 5; i++)
        {
            CreateProjectile(FetchBulletCoords(i));
        }

        PlayFireSound();
    }

    private void CreateProjectile(Vector3 pos)
    {
        GameObject bullet = Instantiate(projectile, pos, Quaternion.identity);
        
        Vector2 dir = new Vector2(pos.x - gameObject.transform.position.x, pos.y - gameObject.transform.position.y);
        bullet.GetComponent<Rigidbody2D>().velocity = dir*bulletSpeed;
    }

    private Vector3 FetchBulletCoords(int bulletNum)
    {
        Vector3 distFromCenter;
        float degreesToRadians;

        degreesToRadians = (float)(bulletNum*72 * (Math.PI / 180));
        Debug.Log(degreesToRadians);
        distFromCenter = new Vector3(radius*Mathf.Sin(degreesToRadians), radius*Mathf.Cos(degreesToRadians), 0);

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
