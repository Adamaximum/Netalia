using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskScript : MonoBehaviour
{
    public float maxSize;
    private bool shrink;
    
    void Start()
    {
        
    }

    
    void Update()
    {
       
    }

    public void Transition()
    {
        StartCoroutine(GrowCoroutine());
    }

    IEnumerator ShrinkCoroutine()
    {
        Debug.Log("running shrink");
        
        while (gameObject.transform.localScale.y > 0)
        {
            gameObject.transform.localScale -= new Vector3(30f, 30f, 0);
            yield return null;
        }
        if (gameObject.transform.localScale.y <= 0)
        {
            Movement.Instance.enabled = true;
            GameManager.Instance.player.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
    }

    IEnumerator GrowCoroutine()
    {
        while (gameObject.transform.localScale.y < maxSize)
        {
            gameObject.transform.localScale += new Vector3(30f, 30f, 0);
            yield return null;
        }
        if (gameObject.transform.localScale.y >= maxSize)
        {
            StartCoroutine(ShrinkCoroutine());
        }
    }
}
