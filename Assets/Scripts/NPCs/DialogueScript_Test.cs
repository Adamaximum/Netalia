using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueScript_Test : MonoBehaviour
{
  
   public Collision playerColl;
  
   //keep track of speaker
   private bool dialogueRunning;
   private int speakerTurn = 0;
  
   [Space(10)]
   [Header("UI")]
   private GameObject DialogueUI;
   private GameObject LeftSpeechBubble;
   private GameObject RightSpeechBubble;
   private GameObject ButtonPrompt;
  
   [Space(10)]
   [Header("JSON Assets")]
   private DialogueSystem Dialogue;
   public TextAsset JsonFile;
  
   public struct DialogueSystem
   {
       public string[] SpeakingOrder;
       public string[] Netalia;
       public string[] NPC;
   }

   void Awake()
   {
       dialogueRunning = false;
      
       //assign components, set UI as inactive
       playerColl = GameObject.Find("Player").GetComponent<Collision>();
       Debug.Log(playerColl);

       //assign blank inspector components
       if (LeftSpeechBubble == null)
       {
           LeftSpeechBubble = GameObject.Find("LeftCharacter");
       }
       LeftSpeechBubble.SetActive(false);

       if (RightSpeechBubble == null)
       {
           RightSpeechBubble = GameObject.Find("RightCharacter");
       }
       RightSpeechBubble.SetActive(false);
       
       if (DialogueUI == null)
       {
           DialogueUI = GameObject.Find("DialoguePanel");
       }
       DialogueUI.SetActive(false);


       ButtonPrompt = GameObject.Find("ButtonPrompt");
      
       //deserialize JSON
       Dialogue = JsonUtility.FromJson<DialogueSystem>(JsonFile.text);
       
       //correct button prompt text
       if (Input.GetJoystickNames().Length > 0)
       {
           ButtonPrompt.GetComponentInChildren<Text>().text = "Press A";
       }
       else
       {
           ButtonPrompt.GetComponentInChildren<Text>().text = "Press Enter";
       }
       
       //set the NPCleft bool, depending on where Netalia is meant to stand
   }

  
   void Update()
   {
       Debug.Log(playerColl.interact);
       
       if (playerColl.interact && !dialogueRunning)
       {
           //show button prompt
           ButtonPrompt.SetActive(true);

           //if button is pressed:
           if (Input.GetButtonDown("Submit"))
           {
               //Netalia walks into position
               //disable Netalia movement script, play idle animation
              
               ZoomIn();
              
               //start running dialogue
               dialogueRunning = true;
           }
       }
       else
       {
           //hide button prompt
           ButtonPrompt.SetActive(false);
       }

       if (dialogueRunning)
       {
          
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

   void Speaking(int lineNum, bool isNPCLeft)
   {
       GameObject currentPanel;
       
       //display proper panel
       if (Dialogue.SpeakingOrder[lineNum] == "NPC" && isNPCLeft)
       {
           currentPanel = LeftSpeechBubble;
       }
       else if (Dialogue.SpeakingOrder[lineNum] == "NPC" && !isNPCLeft)
       {
           currentPanel = RightSpeechBubble;
       }
       else if (Dialogue.SpeakingOrder[lineNum] == "Netalia" && !isNPCLeft)
       {
           currentPanel = LeftSpeechBubble;
       }
       else if (Dialogue.SpeakingOrder[lineNum] == "Netalia" && isNPCLeft)
       {
           currentPanel = RightSpeechBubble;
       }
       
       
       
       
   }


}

