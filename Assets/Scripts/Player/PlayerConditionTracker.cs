using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerConditionTracker : MonoBehaviour
{
    
    public bool[] spokeToNPCs;
    
    public static PlayerConditionTracker Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        DialogueScript_Test[] npcs = GameObject.FindObjectsOfType<DialogueScript_Test>();
        spokeToNPCs = new bool[npcs.Length];

        for (int i = 0; i < spokeToNPCs.Length; i++)
        {
            spokeToNPCs[i] = false;
        }
    }
}