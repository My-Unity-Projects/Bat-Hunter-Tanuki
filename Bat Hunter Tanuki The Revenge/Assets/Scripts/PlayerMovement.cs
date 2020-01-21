using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {

    public Animator transitionAni;
    public string sceneName;

    // Sounds 

    public enum Direction
    {
        up, down, right, left
    }

    Animator ani;
    Vector3 targetPosition;
    Direction direction;

    Camera main;
    Vector3 targetRotation;
    Vector3 currentRotation;

    public GameObject currentRoom;

    public bool gameStarted;

    public bool startBossFight = false;

    [Header("Movement Settings")]
    float slabSize = 0.08f;
    float speed = 10f;

    [Header("Exploration Settings")]
    bool doorAtZero;
    bool doorAtOne;
    bool doorAtTwo;
    bool doorAtThree;

    [Header("Combat Settings")]
    public GameObject weapon;
    public int health;

    [Header("Reward Settings")]
    public int leaves;

    [Header("Hit Animation Settings")]
    bool hit = false;
    int cont = 0;
    float temp = 0;

    [Header("Live Settings")]
    public Canvas normalCanvas;
    public Canvas deathCanvas;
    public bool dead = false;

	// Use this for initialization
	void Start () {

        if (SceneManager.GetActiveScene().name != "Kinematic One")
        {

            GameObject.FindObjectOfType<AudioManager>().Play("Music");

            // Setting interfaces
            normalCanvas.enabled = true;
            deathCanvas.gameObject.SetActive(false);
            dead = false;

            startBossFight = false;

            weapon = transform.GetChild(1).gameObject;

            // Setting health and other values
            health = PlayerPrefs.GetInt("Lifes");

            if (health == 0)
            {
                health = 6;
            }

            if (currentRoom.tag == "BossRoom")
                weapon.SetActive(true);
            else
                weapon.SetActive(false);

            leaves = PlayerPrefs.GetInt("Leaves");

            // Setting general variables
            ani = transform.GetComponent<Animator>();
            main = Camera.main;
            direction = Direction.up;
            targetPosition = transform.position;
        }

        gameStarted = false;       
    }
	
	// Update is called once per frame
	void Update () {

        if(currentRoom.tag != "BossRoom")
            findDoors(currentRoom);
  

        if ((gameStarted && !dead) || (startBossFight && currentRoom.tag == "BossRoom" && !dead))
        {
            Movement();
            changeDirection();
            moveCamera();
            orientatePlayer();            
        }

        hitAnimation();

        if (!gameStarted && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) && SceneManager.GetActiveScene().name == "SampleScene")
        {
            if(Input.GetKeyDown(KeyCode.LeftArrow))
            {
                direction = Direction.left;
            }

            else
            {
                direction = Direction.right;
            }

            ani.SetBool("Idle", false);
            gameStarted = true;
        }

        if(SceneManager.GetActiveScene().name == "BossFight")
        {
            ani.SetBool("Idle", false);
        }
	}


    public void Movement()
    {
        if(transform.position == targetPosition)
        {
            if(direction == Direction.up)
            {
                targetPosition.y += slabSize;

                if (targetPosition.y > currentRoom.transform.position.y + 2.75f)
                {
                    if (!doorAtOne || (doorAtOne && (transform.position.x > .35f + currentRoom.transform.position.x || transform.position.x < -.35f + currentRoom.transform.position.x)))
                    {
                        targetPosition = transform.position;
                        FixDirection();
                    }
                }
                    

                // Animation
                ani.SetBool("WalkingUp", true);
                ani.SetBool("WalkingDown", false);
                ani.SetBool("WalkingRight", false);
                ani.SetBool("WalkingLeft", false);
            }

            if(direction == Direction.down)
            {
                targetPosition.y -= slabSize;

                if (targetPosition.y < currentRoom.transform.position.y - 2.75f  )
                {
                    if(!doorAtThree || (doorAtThree && (transform.position.x > .35f + currentRoom.transform.position.x || transform.position.x < -.35f + currentRoom.transform.position.x)))
                    {
                        targetPosition = transform.position;
                        FixDirection();
                    }
                   
                }

                // Animation
                ani.SetBool("WalkingUp", false);
                ani.SetBool("WalkingDown", true);
                ani.SetBool("WalkingRight", false);
                ani.SetBool("WalkingLeft", false);
            }

            if(direction == Direction.right)
            {
                targetPosition.x += slabSize;

                if (targetPosition.x > currentRoom.transform.position.x + 4.70f)
                {
                    if (!doorAtTwo || (doorAtTwo && (transform.position.y > .35f + currentRoom.transform.position.y || transform.position.y < -.35f + currentRoom.transform.position.y)))
                    {
                        targetPosition = transform.position;
                        FixDirection();
                    }
                }

                // Animation
                ani.SetBool("WalkingUp", false);
                ani.SetBool("WalkingDown", false);
                ani.SetBool("WalkingRight", true);
                ani.SetBool("WalkingLeft", false);

            }

            if (direction == Direction.left)
            {
                targetPosition.x -= slabSize;

                if (targetPosition.x < currentRoom.transform.position.x - 4.70f)
                {
                    if (!doorAtZero || (doorAtZero && (transform.position.y > .35f + currentRoom.transform.position.y || transform.position.y < -.35f + currentRoom.transform.position.y)))
                    {
                        targetPosition = transform.position;
                        FixDirection();
                    }
                }

                // Animation
                ani.SetBool("WalkingUp", false);
                ani.SetBool("WalkingDown", false);
                ani.SetBool("WalkingRight", false);
                ani.SetBool("WalkingLeft", true);
            }
        }

        else
        {
            Vector3 currentPosition = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            transform.position = currentPosition;
        }
    }


    public void changeDirection()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (direction == Direction.up)
            {
                direction = Direction.left;
            }

            else if (direction == Direction.down)
            {
                direction = Direction.right;
            }

            else if (direction == Direction.right)
            {
                direction = Direction.up;

            }

            else if (direction == Direction.left)
            {
                direction = Direction.down;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (direction == Direction.up)
            {
                direction = Direction.right;
            }

            else if (direction == Direction.down)
            {
                direction = Direction.left;
            }

            else if (direction == Direction.right)
            {
                direction = Direction.down;
            }

            else if (direction == Direction.left)
            {
                direction = Direction.up;

            }
        }
    }

    // Collisions
    public void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;

        if(tag == "Stake")
        {
            Destroy(collision.gameObject);
            GameObject.FindObjectOfType<AudioManager>().Play("Take Item");
            weapon.SetActive(true);
        }

        if (tag == "Door")
        {
            MyDoor md = collision.gameObject.GetComponent<MyDoor>();
            if (md.opened)
            {
                if(md.bossDoor)
                {
                    PlayerPrefs.SetInt("Lifes", health);
                    PlayerPrefs.SetInt("Leaves", leaves);
                    sceneName = "BossFight";
                    StartCoroutine(LoadScene());
                   
                }

                else
                {
                    changeRoom(md.side, md.roomTo);
                }               
            }
            else
                FixDirection();
               
        }

        if(tag == "Trap" || tag == "Enemy" || tag == "Bullet")
        {
            hit = true;
            health -= 1;
            main.transform.parent.GetComponent<Animator>().SetTrigger("shake");
            GameObject.FindObjectOfType<AudioManager>().Play("Hurt");

            if(health <= 0)
            {
                dead = true;
                deathCanvas.gameObject.SetActive(true);
                normalCanvas.enabled = false;
                deathCanvas.GetComponent<Animator>().SetTrigger("Appear");
                transform.GetComponent<Rigidbody2D>().simulated = false;
                transform.GetComponent<BoxCollider2D>().enabled = false;
                weapon.SetActive(false);
                hit = false;
                ani.SetBool("Death", true);

                PlayerPrefs.SetInt("Lifes", 6);
                PlayerPrefs.SetInt("Leaves", 0);
            }
        }
        
        if(tag == "Leave")
        {
            GameObject.FindObjectOfType<AudioManager>().Play("Take Item");

            Destroy(collision.gameObject);

            if(leaves < 12)
            {
                leaves += 1;
            }
                
        }

        if(tag == "Life")
        {
           
            if (health < 6)
            {
                Destroy(collision.gameObject);
                health += 1;
                GameObject.FindObjectOfType<AudioManager>().Play("Take Item");
            }
        }
    }

    // Change current room
    public void changeRoom(int side, GameObject room)
    {
        currentRoom = room;

        if(side == 0)
        {
            transform.position = room.transform.position + new Vector3(4.48f, 0, 0);
        }

        if(side == 1)
        {
            transform.position = room.transform.position - new Vector3(0, 2.6f, 0);
        }

        if (side == 2)
        {
            transform.position = room.transform.position - new Vector3(4.48f, 0, 0);
        }

        if (side == 3)
        {
            transform.position = room.transform.position + new Vector3(0, 2.6f, 0);
        }

        targetPosition = transform.position;

        MyRoom mr = room.GetComponent<MyRoom>();

        if (mr.numberOfEnemies > 0)
        {
            if(mr.enterDoor != null)
            {
                mr.enterDoor.GetComponent<MyDoor>().openDoor();
                mr.enterDoor.GetComponent<MyDoor>().closeDoorDelayed();
               
            }

            if (mr.exitDoor != null)
            {
                mr.exitDoor.GetComponent<MyDoor>().openDoor();
                mr.exitDoor.GetComponent<MyDoor>().closeDoorDelayed();
            }
        }

        findDoors(room);
    }

    // Move camera when current room is changed

    public void moveCamera()
    {
        if(currentRoom != null)
        {
            Vector3 targetCameraPosition = currentRoom.transform.position;
            targetCameraPosition.z -= 10;

            if(main.transform.position != targetCameraPosition)
            {
                Vector3 currentPosition = Vector3.MoveTowards(main.transform.position, targetCameraPosition, 0.5f);
                main.transform.position = currentPosition;
            }
        }

    }

    // Fix direction when the player is going out of the room

    public void FixDirection()
    {
        if (direction == Direction.up)
        {
            direction = Direction.down;
        }

        else if (direction == Direction.down)
        {
            direction = Direction.up;
        }

        else if (direction == Direction.right)
        {
            direction = Direction.left;
        }

        else if (direction == Direction.left)
        {
            direction = Direction.right;
        }
    }

    // Orientate weapon

    public void orientatePlayer()
    {
        if(direction == Direction.up)
        {
            weapon.transform.eulerAngles = new Vector3(0, 0, 180);
            weapon.transform.localPosition = new Vector3(0, 0.76f, 0);
        }

        else if(direction == Direction.down)
        {
            weapon.transform.eulerAngles = new Vector3(0, 0, 0);
            weapon.transform.localPosition = new Vector3(0, -0.38f, 0);

        }

        else if(direction == Direction.right)
        {
            weapon.transform.eulerAngles = new Vector3(0, 0, 90);
            weapon.transform.localPosition = new Vector3(0.45f, 0.25f, 0);

        }

        else if(direction == Direction.left)
        {
            weapon.transform.eulerAngles = new Vector3(0, 0, 270);
            weapon.transform.localPosition = new Vector3(-0.45f, 0.25f, 0);

        }
    }

    // Hit animation

    public void hitAnimation()
    {
        if(hit)
        {
            transform.GetComponent<BoxCollider2D>().enabled = false;
            transform.GetComponent<Rigidbody2D>().simulated = false;

            temp -= Time.deltaTime;

            if(temp < 0)
            {
                if(cont%2 == 0)
                {
                    transform.GetComponent<SpriteRenderer>().enabled = false;  
                    transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
                    transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
                }

                else
                {
                    transform.GetComponent<SpriteRenderer>().enabled = true;
                    transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
                    transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
                }

                if(cont == 0 || cont == 1 || cont == 2)
                {
                    temp = 0.2f;
                    cont++;
                }

                else if(cont == 3 || cont == 4 || cont == 5 || cont == 6)
                {
                    temp = 0.15f;
                    cont++;
                }

                else if (cont == 7 || cont == 8 || cont == 9 || cont == 10 || cont == 11)
                {
                    temp = 0.07f;
                    cont++;
                }

                else if(cont == 12)
                {
                    cont = 0;
                    hit = false;
                    transform.GetComponent<SpriteRenderer>().enabled = true;
                    transform.GetComponent<BoxCollider2D>().enabled = true;
                    transform.GetComponent<Rigidbody2D>().simulated = true;
                    transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
                    transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
                }

            }
        }
    }


    // Find the doors of the room

    public void findDoors(GameObject room)
    {
        MyRoom mr = room.GetComponent<MyRoom>();
        MyDoor enterDoor = null;
        MyDoor exitDoor = null;

        if (mr.enterDoor != null)
        {
            enterDoor = mr.enterDoor.GetComponent<MyDoor>();
        }

        if(mr.exitDoor != null)
        {
            exitDoor = mr.exitDoor.GetComponent<MyDoor>();
        }
            
        if ((enterDoor != null && enterDoor.side == 0) || (exitDoor != null && exitDoor.side == 0))
        {
            doorAtZero = true;
        }

        else
            doorAtZero = false;

        if ((enterDoor != null && enterDoor.side == 1) || (exitDoor != null && exitDoor.side == 1))
        {
            doorAtOne = true;
        }

        else
            doorAtOne = false;

        if ((enterDoor != null && enterDoor.side == 2) || (exitDoor != null && exitDoor.side == 2))
        {
            doorAtTwo = true;
        }

        else
            doorAtTwo = false;

        if ((enterDoor != null && enterDoor.side == 3) || (exitDoor != null && exitDoor.side == 3))
        {
            doorAtThree = true;
        }

        else
            doorAtThree = false;
    }


    // Coroutines
    public IEnumerator LoadScene()
    {
        transitionAni.SetTrigger("End");
        StartCoroutine(FadeSound());
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("BossFight");
    }


    IEnumerator FadeSound()
    {
        Sound s = GameObject.FindObjectOfType<AudioManager>().GetSound("Music");

        while (s.volume > 0f)
        {
            s.volume -= Time.deltaTime / 1.5f;
            s.source.volume = s.volume;
            yield return null;
        }
    }


    // Sounds

    public void playStep()
    {
         GameObject.FindObjectOfType<AudioManager>().Play("Step");        
    }

}
