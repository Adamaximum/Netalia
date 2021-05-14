using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueScript_Test : MonoBehaviour
{
   public GameObject player;
   public Camera mainCamera;
   private Vector3 cameraPos;
   private bool interact;
  
   //keep track of speaker
   private bool dialogueRunning;
   private int speakerTurn = 0;
   private bool textSlowRevealing;
   private bool textEmptied;

   [Space(10)]
   [Header("Misc")]
   public float distFromNetalia;
  
   [Space(10)]
   [Header("UI")]
   private GameObject DialogueUI;
   private GameObject LeftSpeechBubble;
   private GameObject RightSpeechBubble;
   private GameObject ButtonPrompt;
   private Image leftImage;
   private Image rightImage;
   public Sprite NPCImage;
   public Sprite playerImage;
  
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
       textEmptied = false;

       //assign components, set UI as inactive
       player = GameObject.Find("Player");

       //assign blank inspector components
       if (LeftSpeechBubble == null)
           LeftSpeechBubble = GameObject.Find("LeftCharacter");

       if (RightSpeechBubble == null) 
           RightSpeechBubble = GameObject.Find("RightCharacter");

       if (DialogueUI == null)
           DialogueUI = GameObject.Find("DialoguePanel");

       ButtonPrompt = gameObject.transform.Find("ButtonPrompt").gameObject;
      
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
       
       //assign images
       leftImage = LeftSpeechBubble.GetComponentInChildren<Image>();
       rightImage = RightSpeechBubble.GetComponentInChildren<Image>();
       if (GetComponent<SpriteRenderer>().flipX)
       {
           leftImage.sprite = NPCImage;
           rightImage.sprite = playerImage;
       }
       else
       {
           leftImage.sprite = playerImage;
           rightImage.sprite = NPCImage;
       }
   }

   private void Start()
   {
       //set dist from Netalia direction
       distFromNetalia = gameObject.GetComponent<SpriteRenderer>().flipX ? distFromNetalia : -distFromNetalia;
       
       //deactivate panels
       LeftSpeechBubble.SetActive(false);
       RightSpeechBubble.SetActive(false);
       DialogueUI.SetActive(false);
   }


   void Update()
   {  
       if (interact && !dialogueRunning)
       {
           //show button prompt
           ButtonPrompt.SetActive(true);

           //if button is pressed:
           if (Input.GetButtonDown("Submit"))
           {
               MoveNetalia(player);

               //start running dialogue
               dialogueRunning = true;
               DialogueUI.SetActive(true);
               
               //zoom in camera
               //ZoomIn();
           }
       }
       else
       {
           //hide button prompt
           ButtonPrompt.SetActive(false);
       }

       //if dialogue is over, disable this script
       if (speakerTurn >= Dialogue.SpokenLines.Length && !textEmptied)
       {
           EmptyDialogue();
           textEmptied = true;
       }

       if (dialogueRunning && Input.GetButtonDown("Submit"))
       {
           CheckForNextLine(speakerTurn);
           DisplayText(SetPanel(speakerTurn), speakerTurn);
       }
       
   }

   /*
   private void FixedUpdate()
   {
       interact = Physics2D.OverlapCircle(transform.position, 2f, LayerMask.GetMask("Player"));
   }
   */
   
   private void OnTriggerStay2D(Collider2D other)
   {
       if (other.gameObject.tag == "Player")
       {
           interact = true;
       }
   }

   private void OnTriggerExit(Collider other)
   {
       if (other.gameObject.tag == "Player")
       {
           interact = false;
       }
   }

   void MoveNetalia(GameObject net)
   {
       //deactivate player scripts
       GameManager.Instance.DisablePlayer();
       
       //move Netalia into place
       Vector2 playerPos = new Vector2(gameObject.transform.position.x + distFromNetalia, gameObject.transform.position.y);
       net.transform.position = playerPos;

   }

   void ZoomIn()
   {
       //save original camera position
       cameraPos = mainCamera.transform.position;
       
       //disable camera script
       mainCamera.GetComponent<TestCamera>().enabled = false;
       
       //move camera closer
       mainCamera.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2, -8);
   }

   void ZoomOut()
   {
       //enable camera script
       mainCamera.GetComponent<TestCamera>().enabled = true;
       
       //move camera back
       mainCamera.transform.position = cameraPos;
   }

   GameObject SetPanel(int lineNum)
   {
       bool isSpeakerLeft;
       isSpeakerLeft = Dialogue.SpeakingOrder[lineNum] == "Netalia";
       
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

       for (int i = 0; i <= textToReveal.Length; i++)
       {
           revealedText = textToReveal.Substring(0, i);
           textBox.text = revealedText;
           yield return new WaitForSeconds(0.05f);
       }

       textSlowRevealing = false;
   }

   void CheckForNextLine(int lineNum)
   {
       string displayedText;
       displayedText = SetPanel(lineNum).GetComponentInChildren<Text>().text;

       if (displayedText == Dialogue.SpokenLines[lineNum])
           speakerTurn++;
       
       Debug.Log(speakerTurn);
   }

   void EmptyDialogue()
   {
       GameManager.Instance.EnablePlayer();
       DialogueUI.SetActive(false);
       //ZoomOut();
   }
   
}

