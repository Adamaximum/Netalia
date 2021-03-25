using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public CameraManager manager;
    public PatrolEnemy[] roomPatrols;
    public ProjectileEnemy[] roomProjEnemies;

    
    void Start()
    {
        manager = GameObject.Find("Main Camera").GetComponent<CameraManager>();

        GameObject room = gameObject.transform.parent.gameObject;
        roomPatrols = room.GetComponentsInChildren<PatrolEnemy>();
        roomProjEnemies = room.GetComponentsInChildren<ProjectileEnemy>();
    }

   
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ActivateRoom();
            
            manager.currentPosition.transform.position = this.transform.position;
            manager.Switch();
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            DeactivateRoom();
        }
    }

    private void ActivateRoom()
    {
        for (int i = 0; i < roomPatrols.Length; i++)
        {
            roomPatrols[i].enabled = true;
            roomPatrols[i].Unfreeze();
        }
        
        for (int i = 0; i < roomProjEnemies.Length; i++)
        {
            roomProjEnemies[i].enabled = true;
        }
    }

    private void DeactivateRoom()
    {
        for (int i = 0; i < roomPatrols.Length; i++)
        {
            roomPatrols[i].Freeze();
            roomPatrols[i].enabled = false;
        }
        
        for (int i = 0; i < roomProjEnemies.Length; i++)
        {
            roomProjEnemies[i].enabled = false;
        }
    }
}
