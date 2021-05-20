using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MaskScript : MonoBehaviour
{
    private SpriteRenderer sprite;
    
    void Awake()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();

        gameObject.transform.localScale = new Vector3(Screen.width, Screen.height);
        sprite.color = new Color(1, 1, 1, 0);
    }
    

    public IEnumerator Flash()
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

    public void QuitToMainMenu()
    {
        StartCoroutine(FadeToTitle());
    }
    
    private IEnumerator FadeToTitle()
    {
        for (float i = 0; i < 1; i += .01f)
        {
            yield return new WaitForSeconds(.005f);
            sprite.color = new Color(0, 0, 0, i);
        }
        
        //load main menu
        SceneManager.LoadScene("Main Menu");
    }
    
    public IEnumerator FadeIn()
    {
        sprite.color = new Color(0, 0, 0, 1);
        
        for (float i = 1; i > 0; i -= .01f)
        {
            yield return new WaitForSeconds(.005f);
            sprite.color = new Color(0, 0, 0, i);
        }
    }
}
