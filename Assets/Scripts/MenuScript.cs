using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    //public MainMenuFade fader;

    public Canvas canvas1;
    public Canvas canvas2;
    public Canvas canvas3;

    public Button playButton;
    public Button controlsButton;
    public Button creditsButton;
    public Button quitButton;
    public Button backButton;

    public SpriteRenderer logo;
    
    void Start()
    {
        playButton.onClick.AddListener(Play);
        controlsButton.onClick.AddListener(Controls);
        creditsButton.onClick.AddListener(Credits);
        quitButton.onClick.AddListener(Quit);
        backButton.onClick.AddListener(Back);
    }

    void Play()
    {
        Debug.Log("Starting game");
        //StartCoroutine(fader.FadeOut());
        SceneManager.LoadScene("Main Scene");
    }

    void Controls()
    {
        Debug.Log("Controls menu");
        canvas1.enabled = false;
        canvas2.enabled = true;
        logo.enabled = false;
    }

    void Credits()
    {
        Debug.Log("Credits menu");
        canvas1.enabled = false;
        canvas3.enabled = true;
        logo.enabled = false;
    }

    void Quit()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }

    void Back()
    {
        Debug.Log("Back to Main Menu");
        canvas1.enabled = true;
        canvas2.enabled = false;
        canvas3.enabled = false;
        logo.enabled = true;
    }
}
