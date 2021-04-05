using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform currentPosition;
    public float defaultSize;

    
    void Start()
    {
        currentPosition = GetComponent<Transform>();
        defaultSize = gameObject.GetComponent<Camera>().orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Switch()
    {
        this.transform.position = currentPosition.transform.position;
    }
}
