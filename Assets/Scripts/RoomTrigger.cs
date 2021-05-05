using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public CameraManager manager;
    public TestCamera managerTest;
    public List<PatrolEnemyX> roomPatrolsX;
    public List<PatrolEnemyY> roomPatrolsY;
    public List<ProjectileEnemy> roomProjEnemies;

    public int roomNum;
    private ProjectileScript bullet;
    private bool activated = false;
    public SpriteRenderer background;


    void Start()
    {
        manager = GameObject.Find("Main Camera").GetComponent<CameraManager>();
        managerTest = GameObject.Find("Main Camera").GetComponent<TestCamera>();

        //GameObject room = gameObject.transform.parent.gameObject;
        //roomPatrolsX = room.GetComponentsInChildren<PatrolEnemyX>();
        //roomPatrolsY = room.GetComponentsInChildren<PatrolEnemyY>();
        //roomProjEnemies = room.GetComponentsInChildren<ProjectileEnemy>();
        //background = room.transform.Find("background").GetComponent<SpriteRenderer>();
        
        GameManager.Instance.rooms[roomNum] = this;

        DeactivateRoom();
        background.enabled = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Time.frameCount % 5 == 0 && collision.tag == "Player")
        {
            Collision.Instance.roomNum = roomNum;

            if (!activated)
            {
                ActivateRoom();
                activated = true;
                
                if (background != null)
                    background.enabled = true;
            }

            if (manager != null)
            {
                manager.currentPosition.transform.position = this.transform.position;
                manager.Switch();
            }
            
            //test camera stuff
            if (managerTest != null && managerTest.enabled)
            {
                managerTest.roomPos = this.transform;
                managerTest.Switch();
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && activated)
        {
            DeactivateRoom();
            activated = false;
            
            if (background != null)
                background.enabled = false;
        }
    }

    public void ActivateRoom()
    {
        for (int i = 0; i < roomPatrolsX.Count; i++)
        {
            roomPatrolsX[i].enabled = true;
            roomPatrolsX[i].Unfreeze();
        }
        
        for (int i = 0; i < roomPatrolsY.Count; i++)
        {
            roomPatrolsY[i].enabled = true;
            roomPatrolsY[i].Unfreeze();
        }
        
        for (int i = 0; i < roomProjEnemies.Count; i++)
        {
            roomProjEnemies[i].enabled = true;
        }
    }

    public void DeactivateRoom()
    {
        for (int i = 0; i < roomPatrolsX.Count; i++)
        {
            roomPatrolsX[i].Freeze();
            roomPatrolsX[i].enabled = false;
        }
        
        for (int i = 0; i < roomPatrolsY.Count; i++)
        {
            roomPatrolsY[i].Freeze();
            roomPatrolsY[i].enabled = false;
        }
        
        for (int i = 0; i < roomProjEnemies.Count; i++)
        {
            roomProjEnemies[i].enabled = false;
        }
        
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Projectile");
        foreach(GameObject bullet in bullets)
            GameObject.Destroy(bullet);
    }
}
