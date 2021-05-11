using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapScript : MonoBehaviour
{

    private Vector2 scaleFactor;
    private Canvas map;

    void Start()
    {
        scaleFactor = new Vector2(.3f, .18f);
        map = GameObject.Find("Minimap").GetComponent<Canvas>();
    }

    
    void Update()
    {
        if (Time.frameCount % 10 == 0 && map.enabled)
        {
            RepositionMarker();
        }

        if (Input.GetButtonDown("Minimap"))
        {
            ToggleMap();
        }
    }

    Vector3 TranslatePlayerPos(Vector3 playerPos)
    {
        Vector3 scaledPos;
        scaledPos = new Vector3(playerPos.x*scaleFactor.x, playerPos.y*scaleFactor.x, 1);

        return scaledPos;
    }

    void RepositionMarker()
    {
        gameObject.transform.localPosition = TranslatePlayerPos(Movement.Instance.gameObject.transform.position);
    }

    void ToggleMap()
    {
        if (map.enabled)
            map.enabled = false;
        else if (!map.enabled)
            map.enabled = true;
    }
}
