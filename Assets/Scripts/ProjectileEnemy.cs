using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemy : MonoBehaviour
{

    public GameObject projectile;
    public float bulletSpeed;
    public float fireRate;
    private bool readyToFire = true;
    
    //for bullet instantiation calculations
    private float radius;

    void Start()
    {
        radius = GetComponent<CircleCollider2D>().radius;
    }

    
    void Update()
    {
        if (readyToFire)
            StartCoroutine(PauseThenFire());
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
}
