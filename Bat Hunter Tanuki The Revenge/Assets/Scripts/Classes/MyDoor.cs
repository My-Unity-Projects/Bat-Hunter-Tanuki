using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDoor : MonoBehaviour {

    public GameObject roomTo;

    [Header("State Settings")]
    public bool opened;

    [Header("Position Settings")]
    public int side;

    [Header("Animation Settings")]
    Animator ani;

    [Header("Boss Settings")]
    public bool bossDoor = false;



	// Use this for initialization
	void Start () {

        opened = false;
        ani = transform.GetComponent<Animator>();
       
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void openDoor()
    {
        if (!opened)
        {
            opened = true;

            ani.SetBool("opened", true);
            ani.SetBool("closed", false);

            GameObject.FindObjectOfType<AudioManager>().Play("Open Door");
        }
    }

    public void closeDoor()
    {
        if(opened)
        {
            opened = false;

            ani.SetBool("closed", true);
            ani.SetBool("opened", false);

            GameObject.FindObjectOfType<AudioManager>().Play("Open Door");
        }
    
    }

    // Methods with delay

    public void openDoorDelayed()
    {
        Invoke("openDoor", 2);
    }

    public void closeDoorDelayed()
    {
        // Debug.Log("Closing doors with delay");
        Invoke("closeDoor", 0.35f);
    }
}
