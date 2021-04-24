using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapScript : MonoBehaviour
{

    private float scaleFactor;
    
    void Start()
    {
        scaleFactor = .45f;
    }

    
    void Update()
    {
        if (Time.frameCount % 10 == 0)
        {
            RepositionMarker();
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
}
