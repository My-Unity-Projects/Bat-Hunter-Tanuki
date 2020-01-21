using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyBossRoom : MonoBehaviour {















    public GameObject Boss;
    public GameObject Trap;

    // Use this for initialization
    void Start()
    {
        displayBoss();
        displayTraps();


    }

    // Update is called once per frame
    void Update()
    {

    }

    // Set enemies
    /*public void checkAmountofEnemies()
    {
        if (numberOfEnemies == 0)
        {
            if (enterDoor != null)
            {
                enterDoor.GetComponent<MyDoor>().openDoor();
            }

            if (exitDoor != null)
            {
                exitDoor.GetComponent<MyDoor>().openDoor();
            }
        }
    }*/

    public void displayBoss()
    {
        GameObject displayedEnemy;
        displayedEnemy = Instantiate(Boss, transform.position, Quaternion.identity);
        displayedEnemy.transform.GetChild(0).GetComponent<MyBoss>().room = this.gameObject;                    
    }

    // Set traps
    public void displayTraps()
    {
        for (int i = 0; i < 4; i++)
        {
            float xPos = Random.Range(-6, 7) * 0.64f + transform.position.x;

            while(xPos == 0)
            {
                xPos = Random.Range(-6, 7) * 0.64f + transform.position.x;
            }

            float yPos = Random.Range(-3, 4) * 0.64f + transform.position.y;

            Instantiate(Trap, new Vector3(xPos, yPos, 0), Quaternion.identity);
        }       
    }
}
