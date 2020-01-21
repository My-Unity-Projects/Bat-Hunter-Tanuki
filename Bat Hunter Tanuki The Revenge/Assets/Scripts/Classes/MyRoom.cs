using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRoom : MonoBehaviour {

    public int roomNumber;

    public bool placed;

    public GameObject[] doors;

    public GameObject enterDoor, exitDoor;

    public int nextRoomSide;
    public int lastRoomSide;

    public GameObject nextRoom, lastRoom;


    public GameObject Enemy;
    public GameObject Trap;

    public int numberOfEnemies;
    public int numberOfTraps;

	// Use this for initialization
	void Start () {


		
	}
	
	// Update is called once per frame
	void Update () {

        displayDoorsOkay();

        checkAmountofEnemies();

        if(roomNumber != 0)
        {
            displayEnemies();
            displayTraps();
        }
	}

    // Set the doors of the room
    
    public void displayDoorsOkay()
    {
        if (nextRoomSide != -1)
        {
            displayExitDoor(nextRoomSide);
            nextRoomSide = -1;
        }

        if (lastRoomSide != -1)
        {
            displayEnterDoor(lastRoomSide);
            lastRoomSide = -1;
        }
    }

    public void displayExitDoor(int side)
    {
        exitDoor = doors[side];

        exitDoor.SetActive(true);

        exitDoor.GetComponent<MyDoor>().side = side;
        exitDoor.GetComponent<MyDoor>().roomTo = nextRoom;

        if (roomNumber == 8)
        {
            exitDoor.GetComponent<MyDoor>().bossDoor = true;
        }
    }

    public void displayEnterDoor(int side)
    {
        enterDoor = doors[side];

        enterDoor.SetActive(true);

        enterDoor.GetComponent<MyDoor>().side = side;
        enterDoor.GetComponent<MyDoor>().roomTo = lastRoom;       
    }


    // Set enemies

    public void checkAmountofEnemies()
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
    }

    public void displayEnemies()
    {
        if(numberOfEnemies == -1)
        {
            numberOfEnemies = ((roomNumber + 1) / 2) + 1;

            for (int i = 0; i < numberOfEnemies; i++)
            {
                GameObject displayedEnemy;
                displayedEnemy = Instantiate(Enemy, transform.position, Quaternion.identity);
                displayedEnemy.GetComponent<MyEnemy>().room = this.gameObject;
            }
        }
    }

    // Set traps

    public void displayTraps()
    {
        if(numberOfTraps == -1)
        {
            numberOfTraps = ((roomNumber + 1) / 2) + 1;

            for(int i=0; i<numberOfTraps; i++)
            {
                float xPos = Random.Range(-6, 7) * 0.64f + transform.position.x;
                float yPos = Random.Range(-3, 4) * 0.64f + transform.position.y;

                Instantiate(Trap, new Vector3(xPos, yPos, 0), Quaternion.identity);
            }
        }
    }
}
