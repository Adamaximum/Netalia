using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeirdBugFix : MonoBehaviour
{

    public Vector2 realPosition;
    
    void Start()
    {
        gameObject.transform.position = realPosition;
    }

   
}
