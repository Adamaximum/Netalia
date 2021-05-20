using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerConditionTracker : MonoBehaviour
{
    
    public bool[] spokeToNPCs;
    public bool[] showMarker;
    public static PlayerConditionTracker Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        DialogueScript_Test[] npcs = GameObject.FindObjectsOfType<DialogueScript_Test>();
        spokeToNPCs = new bool[npcs.Length];
        showMarker = new bool[spokeToNPCs.Length];

        for (int i = 0; i < spokeToNPCs.Length; i++)
        {
            spokeToNPCs[i] = false;
            showMarker[i] = false;
        }

        showMarker[0] = true;
    }

    public void UpdateMarkers()
    {
        //turn marker for NPC 1 on if NPC 0 has been spoken to
        if (spokeToNPCs[0])
        {
            showMarker[1] = true;
            showMarker[0] = false;
        }
        
        //turn marker for NPCs 2 and 3 on if NPC 1 has been spoken to
        if (spokeToNPCs[1])
        {
            showMarker[2] = true;
            showMarker[3] = true;
            showMarker[1] = false;
            showMarker[0] = false;
        }

        if (spokeToNPCs[2])
        {
            showMarker[2] = false;
        }
        
        if (spokeToNPCs[3])
        {
            showMarker[3] = false;
        }

        //turn marker for NPC 4 on if NPCs 2 and 3 have been spoken to
        if (spokeToNPCs[2] && spokeToNPCs[3])
        {
            showMarker[4] = true;
        }

        //turn marker for NPC 5 on if NPC 4 has been spoken to
        if (spokeToNPCs[4])
        {
            showMarker[5] = true;
            showMarker[4] = false;
        }
    }
}