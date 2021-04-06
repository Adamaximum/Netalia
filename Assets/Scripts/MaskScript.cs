using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskScript : MonoBehaviour
{
    public float maxSize;
    private bool shrink;
    public bool grow;

    public float growOrShrinkSize = 30;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        /*
        if (grow)
        {
            while (gameObject.transform.localScale.y < maxSize)
            {
                gameObject.transform.localScale += new Vector3(growOrShrinkSize, growOrShrinkSize, 0);
            }

            if (gameObject.transform.localScale.y >= maxSize)
            {
                grow = false;
                shrink = true;
            }
        }

        if (shrink)
        {
            while (gameObject.transform.localScale.y > 0)
            {
                gameObject.transform.localScale -= new Vector3(growOrShrinkSize, growOrShrinkSize, 0);
            }

            if (gameObject.transform.localScale.y == 0)
            {
                
            }
        }
        */
    }

    public void Transition()
    {
        StartCoroutine(GrowCoroutine());
    }

    IEnumerator ShrinkCoroutine()
    {
        while (gameObject.transform.localScale.y > 0)
        {
            gameObject.transform.localScale -= new Vector3(growOrShrinkSize, growOrShrinkSize, 0);
            yield return null;
        }

        gameObject.transform.localScale = new Vector3(0, 0, 0);
        GameManager.Instance.EnablePlayer();
        GameManager.Instance.EnablePlayer();
    }

    IEnumerator GrowCoroutine()
    {
        while (gameObject.transform.localScale.y < maxSize)
        {
            gameObject.transform.localScale += new Vector3(growOrShrinkSize, growOrShrinkSize, 0);
            yield return null;
        }

        GameManager.Instance.EndReset();
        StartCoroutine(ShrinkCoroutine());
    }
}
