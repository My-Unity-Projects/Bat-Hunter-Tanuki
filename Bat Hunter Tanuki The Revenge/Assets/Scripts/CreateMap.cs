using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMap : MonoBehaviour {

    public GameObject roomPrefab;

    [Header("Map room array")]
    public GameObject[] mapRooms = new GameObject[9];

    int cont = 1;
    Vector3 newRoomPosition;
    bool freePosition;

    [Header("Access to the room properties")]
    MyRoom oldRoom;
    MyRoom newRoom; 


	// Use this for initialization
	void Start () {

        for(int i=1; i<9; i++)
        {
            createRoom();
            cont++;
        }		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void createRoom()
    {

        newRoomPosition = mapRooms[cont - 1].transform.position; // Before choose the place where is gonna be displayed the room it needs to displayed somewhere else


        mapRooms[cont] = Instantiate(roomPrefab, newRoomPosition, Quaternion.identity); // Display the room in the same position of the last one

        oldRoom = mapRooms[cont - 1].GetComponent<MyRoom>();
        newRoom = mapRooms[cont].GetComponent<MyRoom>();
        

        // Registering the room number for each room
        oldRoom.roomNumber = cont - 1;
        newRoom.roomNumber = cont;

        placeRoom(mapRooms[cont]);
    }


    public void placeRoom(GameObject room)
    {
        int nextPlace = Random.Range(0, 4);

        if(cont == 8)
        {
            while(nextPlace == 3)
            {
                nextPlace = Random.Range(0, 2);
            }

            newRoom.nextRoomSide = 1;
        }

        // Next room in the left
        if (nextPlace == 0)
        {
            oldRoom.nextRoomSide = 0;
            newRoom.lastRoomSide = 2;

            newRoomPosition.x -= 14;
        }

        // Next room in the top
        else if (nextPlace == 1)
        {
            oldRoom.nextRoomSide = 1;
            newRoom.lastRoomSide = 3;

            newRoomPosition.y += 8;
        }

        // Next room in the right
        else if (nextPlace == 2)
        {
            oldRoom.nextRoomSide = 2;
            newRoom.lastRoomSide = 0;

            newRoomPosition.x += 14;
        }

        // Next room in the bottom
        else if (nextPlace == 3)
        {
            oldRoom.nextRoomSide = 3;
            newRoom.lastRoomSide = 1;

            newRoomPosition.y -= 8;
        }

        for(int i = 0; i < cont; i++)
        {
            if(mapRooms[i].transform.position == newRoomPosition)
            {
                freePosition = false;
                break;
            }

            freePosition = true;
        }

        if(freePosition)
        {
            mapRooms[cont].transform.position = newRoomPosition;
            oldRoom.nextRoom = newRoom.gameObject;
            newRoom.lastRoom = oldRoom.gameObject;
            newRoom.placed = true;
        }

        else
        {
            newRoomPosition = oldRoom.gameObject.transform.position;
            placeRoom(room);
        }          
    }

}
