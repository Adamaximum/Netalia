using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuFade : MonoBehaviour
{
    private Image fadeImg;

    private void Start()
    {
        fadeImg = GetComponent<Image>();

        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        for (float i = 1; i > 0; i -= .01f)
        {
            yield return new WaitForSeconds(.005f);
            fadeImg.color = new Color(0, 0, 0, i);
        }
    }
    
    //Adam - change this to public if you need to
    private IEnumerator FadeOut()
    {
        for (float i = 0; i < 1; i += .01f)
        {
            yield return new WaitForSeconds(.005f);
            fadeImg.color = new Color(0, 0, 0, i);
        }
    }
}
