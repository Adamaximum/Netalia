using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public CameraManager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("Main Camera").GetComponent<CameraManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            manager.currentPosition.transform.position = this.transform.position;
            manager.Switch();
        }
    }
}
