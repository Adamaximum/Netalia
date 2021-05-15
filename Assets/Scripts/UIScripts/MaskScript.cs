using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskScript : MonoBehaviour
{
    private SpriteRenderer sprite;
    public float secondsToFlash;
    private float flashIncrement;
    
    void Awake()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();

        flashIncrement = secondsToFlash / Time.deltaTime;
    }

    
    public void Flash()
    {
        /*
        for (float i = 0; i < 1; i += flashIncrement)
        {
            sprite.color = new Color(1, 1, 1, i);
        }
        
        for (float i = 1; i > 0; i -= flashIncrement)
        {
            sprite.color = new Color(1, 1, 1, i);
        }
        */
    }
}
