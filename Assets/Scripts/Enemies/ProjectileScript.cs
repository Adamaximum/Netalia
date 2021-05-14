using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public static bool destroyAll = false;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 8)
        {
            Destroy(gameObject);
        }
        
    }

    private void Update()
    {
        if (destroyAll)
            Destroy(gameObject);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.25f, LayerMask.GetMask("Rooms"));
    }
}
