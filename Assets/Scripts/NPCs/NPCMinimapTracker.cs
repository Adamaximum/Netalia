using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMinimapTracker : MonoBehaviour
{
    public int orderInGame;
    public SpriteRenderer minimapMarker;

    private void Awake()
    {
        minimapMarker = gameObject.GetComponent<SpriteRenderer>();

        if (orderInGame == 0)
        {
            minimapMarker.color = new Color(0, 1, 1, 1);
        }
    }

    private void Update()
    {
        if (Time.frameCount % 15 == 0)
        {
            if (!PlayerConditionTracker.Instance.spokeToNPCs[orderInGame-1])
            {
                minimapMarker.color = new Color(1, 1, 1, 0);
            }
            else if (orderInGame == 0 && PlayerConditionTracker.Instance.spokeToNPCs[orderInGame])
            {
                minimapMarker.color = new Color(1, 1, 1, 0);
            }
            else
            {
                minimapMarker.color = new Color(0, 1, 1, 1);
            }
        }
    }

    public void SpokeToPlayer()
    {
        PlayerConditionTracker.Instance.spokeToNPCs[orderInGame] = true;
    }
}
