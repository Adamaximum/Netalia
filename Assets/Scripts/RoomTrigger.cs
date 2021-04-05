using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public CameraManager manager;
    public PatrolEnemyX[] roomPatrolsX;
    public PatrolEnemyY[] roomPatrolsY;
    public ProjectileEnemy[] roomProjEnemies;

    public int roomNum;

    
    void Start()
    {
        manager = GameObject.Find("Main Camera").GetComponent<CameraManager>();

        GameObject room = gameObject.transform.parent.gameObject;
        roomPatrolsX = room.GetComponentsInChildren<PatrolEnemyX>();
        roomPatrolsY = room.GetComponentsInChildren<PatrolEnemyY>();
        roomProjEnemies = room.GetComponentsInChildren<ProjectileEnemy>();

        GameManager.Instance.rooms[roomNum] = this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Collision.Instance.roomNum = roomNum;
            
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

    public void ActivateRoom()
    {
        for (int i = 0; i < roomPatrolsX.Length; i++)
        {
            roomPatrolsX[i].enabled = true;
            roomPatrolsX[i].Unfreeze();
        }
        
        for (int i = 0; i < roomPatrolsY.Length; i++)
        {
            roomPatrolsY[i].enabled = true;
            roomPatrolsY[i].Unfreeze();
        }
        
        for (int i = 0; i < roomProjEnemies.Length; i++)
        {
            roomProjEnemies[i].enabled = true;
        }
    }

    public void DeactivateRoom()
    {
        for (int i = 0; i < roomPatrolsX.Length; i++)
        {
            roomPatrolsX[i].Freeze();
            roomPatrolsX[i].enabled = false;
        }
        
        for (int i = 0; i < roomPatrolsY.Length; i++)
        {
            roomPatrolsY[i].Freeze();
            roomPatrolsY[i].enabled = false;
        }
        
        for (int i = 0; i < roomProjEnemies.Length; i++)
        {
            roomProjEnemies[i].enabled = false;
        }
    }
}
