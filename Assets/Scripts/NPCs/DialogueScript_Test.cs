using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueScript_Test : MonoBehaviour
{
    
    private Collision playerColl;

    [Header("UI")]
    [Space(10)]
    
    private Canvas DialogueUI;
    private GameObject NPCSpeechBubble;
    private GameObject NetaliaSpeechBubble;

    [Header("JSON Assets")]
    [Space(10)]
    
    private DialogueSystem Dialogue;
    private TextAsset JsonFile;
    
    public struct DialogueSystem
    {
        public string[] SpeakingOrder;
        public string[] Netalia;
        public string[] NPC;
    }

    void Awake()
    {
        //assign components, set UI as inactive
        playerColl = GameObject.Find("Player").GetComponent<Collision>();

        if (DialogueUI == null)
        {
            DialogueUI = GameObject.Find("DialoguePanel").GetComponent<Canvas>();
        }

        DialogueUI.enabled = false;
        
        if (NPCSpeechBubble == null)
        {
            NPCSpeechBubble = GameObject.Find("LeftCharacter");
        }
        
        NPCSpeechBubble.SetActive(false);

        if (NetaliaSpeechBubble == null)
        {
            NetaliaSpeechBubble = GameObject.Find("RightCharacter");
        }

        NetaliaSpeechBubble.SetActive(false);
        
        //deserialize JSON
        Dialogue = JsonUtility.FromJson<DialogueSystem>(JsonFile.text);
    }

    
    void Update()
    {
        if (playerColl.interact)
        {
            //show button prompt
            
            //if button is pressed:
            if (Input.GetButtonDown("Interact"))
            {
                
            }
        }
    }

    void ZoomIn()
    {
        //disable camera movement script
        
        //move camera to appropriate spacing
    }

    void ZoomOut()
    {
        //enable camera movement script
        
        //move camera back to former spacing
    }

}
