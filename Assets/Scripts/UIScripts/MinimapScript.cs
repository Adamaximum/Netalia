using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapScript : MonoBehaviour
{

    private float scaleFactor;
    private Canvas map;

    void Start()
    {
        scaleFactor = .45f;
        map = GameObject.Find("Minimap").GetComponent<Canvas>();
    }

    
    void Update()
    {
        if (Time.frameCount % 10 == 0 && map.enabled)
        {
            RepositionMarker();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMap();
        }
    }

    Vector3 TranslatePlayerPos(Vector3 playerPos)
    {
        Vector3 scaledPos;
        scaledPos = new Vector3(playerPos.x*scaleFactor, playerPos.y*scaleFactor, 1);

        return scaledPos;
    }

    void RepositionMarker()
    {
        gameObject.transform.localPosition = TranslatePlayerPos(MovementTest.Instance.gameObject.transform.position);
    }

    void ToggleMap()
    {
        if (map.enabled)
            map.enabled = false;
        else if (!map.enabled)
            map.enabled = true;
    }
}
