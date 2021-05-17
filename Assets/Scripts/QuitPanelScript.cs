using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class QuitPanelScript : MonoBehaviour
{
    public Button yesButton;
    public Button noButton;

    void Start () {
       
        yesButton.onClick.AddListener(Quit);
        noButton.onClick.AddListener(ClosePanel);
    }

    void Quit()
    {
        Application.Quit();
    }

    void ClosePanel()
    {
        gameObject.GetComponent<Canvas>().enabled = false;
    }
}
