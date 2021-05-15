using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskScript : MonoBehaviour
{
    private SpriteRenderer sprite;
    public float secondsToFlash;
    private float flashIncrements;
    
    void Awake()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();

        secondsToFlash = secondsToFlash*Time.deltaTime;
        flashIncrements = secondsToFlash / 100;

        gameObject.transform.localScale = new Vector3(Screen.width, Screen.height);
        sprite.color = new Color(1, 1, 1, 0);
    }

    
    public void Flash()
    {
        StartCoroutine(FlashCoroutine());
    }

    IEnumerator FlashCoroutine()
    {
        for (float i = 0; i < 1; i += .05f)
        {
            yield return new WaitForSeconds(.01f);
            sprite.color = new Color(1, 1, 1, i);
        }
        
        for (float i = 1; i > 0; i -= .05f)
        {
            yield return new WaitForSeconds(.01f);
            sprite.color = new Color(1, 1, 1, i);
        }
        
        GameManager.Instance.EndReset();
        GameManager.Instance.EnablePlayer();
    }
}
