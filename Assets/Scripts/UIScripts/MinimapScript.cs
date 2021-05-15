using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapScript : MonoBehaviour
{
    private Canvas map;

    void Start()
    {
        map = GameObject.Find("Minimap").GetComponent<Canvas>();
        map.enabled = false;
    }

    
    void Update()
    {
        if (Input.GetButtonDown("Minimap"))
        {
            ToggleMap();
        }
    }
    

    void ToggleMap()
    {
        if (map.enabled)
            map.enabled = false;
        else if (!map.enabled)
            map.enabled = true;
    }
}
