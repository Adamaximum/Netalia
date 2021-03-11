using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueScript_Test : MonoBehaviour
{
   public GameObject player;
   public Collision playerColl;
  
   //keep track of speaker
   private bool dialogueRunning;
   private int speakerTurn = 0;
   private bool textSlowRevealing;

   [Space(10)]
   [Header("Misc")]
   public float distFromNetalia;
  
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
       public string[] SpokenLines;
   }

   void Awake()
   {
       dialogueRunning = false;
      
       //assign components, set UI as inactive
       player = GameObject.Find("Player");
       playerColl = player.GetComponent<Collision>();

       //assign blank inspector components
       if (LeftSpeechBubble == null) 
           LeftSpeechBubble = GameObject.Find("LeftCharacter");
       LeftSpeechBubble.SetActive(false);

       if (RightSpeechBubble == null) 
           RightSpeechBubble = GameObject.Find("RightCharacter");
       RightSpeechBubble.SetActive(false);
       
       if (DialogueUI == null)
           DialogueUI = GameObject.Find("DialoguePanel");
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
   }

  
   void Update()
   {
       if (playerColl.interact && !dialogueRunning)
       {
           //show button prompt
           ButtonPrompt.SetActive(true);

           //if button is pressed:
           if (Input.GetButtonDown("Submit"))
           {
               //MoveNetalia(player);
              
               ZoomIn();
              
               //start running dialogue
               dialogueRunning = true;
               DialogueUI.SetActive(true);
           }
       }
       else
       {
           //hide button prompt
           ButtonPrompt.SetActive(false);
       }

       //if dialogue is over, disable this script
       if (speakerTurn >= Dialogue.SpokenLines.Length)
       {
           EmptyDialogue();
       }

       if (dialogueRunning)
       {
           if (Input.GetButtonDown("Submit"))
           {
               //find correct line # and speaker
               //fetch correct panel to display
               //start slow revealing text
               //if button is pressed again, show all text at once

               DisplayText(SetPanel(speakerTurn), speakerTurn);
               CheckForNextLine(speakerTurn);
           }
       }
       
   }

   void MoveNetalia(GameObject net)
   {
       //deactivate player scripts
       net.GetComponent<MovementTest>().enabled = false;
       net.GetComponentInChildren<AnimationScript>().enabled = false;
       
       //move Netalia into place
       distFromNetalia = gameObject.GetComponent<SpriteRenderer>().flipX ? distFromNetalia : -distFromNetalia;
       
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

   GameObject SetPanel(int lineNum)
   {
       bool isSpeakerLeft;
       isSpeakerLeft = !gameObject.GetComponent<SpriteRenderer>().flipX && Dialogue.SpeakingOrder[lineNum] == "NPC";
       
       //show the correct panel
       GameObject currentPanel;
       currentPanel = isSpeakerLeft ? LeftSpeechBubble : RightSpeechBubble;

       GameObject inactivePanel;
       inactivePanel = isSpeakerLeft ? RightSpeechBubble : LeftSpeechBubble;
       
       currentPanel.SetActive(true);
       inactivePanel.SetActive(false);

       return currentPanel;

       //display the appropriate text
       //Text textBox = currentPanel.GetComponentInChildren<Text>();
       //StartCoroutine(SlowRevealText(textBox, Dialogue.SpokenLines[lineNum]));

       //update profile picture
   }

   void DisplayText(GameObject panel, int lineNum)
   {
       Text textBox = panel.GetComponentInChildren<Text>();

       if (!textSlowRevealing)
       {
           StartCoroutine(SlowRevealText(textBox, Dialogue.SpokenLines[lineNum]));
       }
       else
       {
           StopAllCoroutines();
           textSlowRevealing = false;
           textBox.text = Dialogue.SpokenLines[lineNum];
       }
   }

   IEnumerator SlowRevealText(Text textBox, string textToReveal)
   {
       textSlowRevealing = true;
       string revealedText;

       for (int i = 0; i < textToReveal.Length; i++)
       {
           revealedText = textToReveal.Substring(0, i);
           textBox.text = revealedText;
           yield return new WaitForSeconds(0.1f);
       }

       textSlowRevealing = false;
       
       Debug.Log(textSlowRevealing);
       yield break;
   }

   void CheckForNextLine(int lineNum)
   {
       string displayedText;
       displayedText = SetPanel(lineNum).GetComponentInChildren<Text>().text;

       if (displayedText == Dialogue.SpokenLines[lineNum])
           speakerTurn++;
   }

   void EmptyDialogue()
   {
       DialogueUI.SetActive(false);

   }
   
}

