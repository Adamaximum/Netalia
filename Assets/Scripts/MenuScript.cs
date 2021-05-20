using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public Button playButton;
    public Button controlsButton;
    public Button creditsButton;
    public Button quitButton;
    
    void Start()
    {
        playButton.onClick.AddListener(Play);
        controlsButton.onClick.AddListener(Controls);
        creditsButton.onClick.AddListener(Credits);
        quitButton.onClick.AddListener(Quit);
    }

    void Play()
    {

    }

    void Controls()
    {

    }

    void Credits()
    {

    }

    void Quit()
    {
        Application.Quit();
    }
}
