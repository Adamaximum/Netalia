using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform currentPosition;

    // Start is called before the first frame update
    void Start()
    {
        currentPosition = GetComponent<Transform>();
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
