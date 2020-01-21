using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEnemy : MonoBehaviour {

    public GameObject room;

    public GameObject reward;
    public GameObject lifeReward;

    [Header("Movement Settings")]
    public float speed;
    Vector3 targetPosition;
    Vector3 currentPositiion;
    int xOryFirst = -1;

    [Header("Animation Settings")]
    Animator ani;

    [Header("Shoot Settings")]
    public GameObject enemyBullet;
    

	// Use this for initialization
	void Start () {

        ani = transform.GetComponent<Animator>();
        targetPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        if(room.GetComponent<MyRoom>().roomNumber != 0)
        {
            Movement();
        }                         
	}

    public void Movement()
    {
        if (transform.position == targetPosition)
        {
            targetPosition.x = Random.Range(-7, 8) * 0.64f + room.transform.position.x;
            targetPosition.y = Random.Range(-4, 5) * 0.64f + room.transform.position.y;

            xOryFirst = Random.Range(0, 2);
        }

        else
        {
            if(xOryFirst == 0)
            {
                if(targetPosition.x > transform.position.x)
                {
                    ani.SetBool("Up", false);
                    ani.SetBool("Down", false);
                    ani.SetBool("Right", true);
                    ani.SetBool("Left", false);
                }

                else if(targetPosition.x < transform.position.x)
                {
                    ani.SetBool("Up", false);
                    ani.SetBool("Down", false);
                    ani.SetBool("Right", false);
                    ani.SetBool("Left", true);
                }

                currentPositiion = Vector3.MoveTowards(transform.position, new Vector3(targetPosition.x, transform.position.y, 0), speed * Time.deltaTime);
                transform.position = currentPositiion;

                if(targetPosition.x == transform.position.x && targetPosition.y != transform.position.y)
                {
                    xOryFirst = 1;
                }
            }

            if (xOryFirst == 1)
            {
                if (targetPosition.y > transform.position.y)
                {
                    ani.SetBool("Up", true);
                    ani.SetBool("Down", false);
                    ani.SetBool("Right", false);
                    ani.SetBool("Left", false);
                }

                else if (targetPosition.y < transform.position.y)
                {
                    ani.SetBool("Up", false);
                    ani.SetBool("Down", true);
                    ani.SetBool("Right", false);
                    ani.SetBool("Left", false);
                }
                currentPositiion = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, targetPosition.y, 0), speed * Time.deltaTime);
                transform.position = currentPositiion;

                if (targetPosition.x != transform.position.x && targetPosition.y == transform.position.y)
                {
                    xOryFirst = 0;
                }
            }
        }
    }


    public void Shoot()
    {

        GameObject Tanuki = GameObject.Find("Tanuki");
        int chance = Random.Range(0, 2);

        if(Tanuki.GetComponent<PlayerMovement>().currentRoom == room)
            GameObject.FindObjectOfType<AudioManager>().Play("Bat Attack");

        if (Tanuki != null && Tanuki.GetComponent<PlayerMovement>().currentRoom == room && chance == 0 && room.GetComponent<MyRoom>().roomNumber != 0)
        {
            GameObject bullet = (GameObject)Instantiate(enemyBullet);
            bullet.transform.position = transform.position + new Vector3(0, 0.48f, 0);
            Vector2 direction = Tanuki.transform.position - bullet.transform.position;
            bullet.GetComponent<MyEnemyBullet>().SetDirection(direction);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Weapon")
        {
            ani.SetBool("Death", true);
            room.GetComponent<MyRoom>().numberOfEnemies -= 1;

            int random = Random.Range(0, 3);
            
            if(random == 0)
            {
                Instantiate(reward, transform.position, Quaternion.identity);
            }

            else if(random == 1)
            {
                Instantiate(lifeReward, transform.position, Quaternion.identity);
            }
                
            transform.GetComponent<SpriteRenderer>().sortingOrder = 1;
            Camera.main.transform.parent.GetComponent<Animator>().SetTrigger("shake");
            speed = 0;
            GameObject.FindObjectOfType<AudioManager>().Play("Kill Enemy");

        }
    }

    public void PlayBatWings()
    {
        if(room == GameObject.FindObjectOfType<PlayerMovement>().currentRoom)
            GameObject.FindObjectOfType<AudioManager>().Play("Bat Wings");
    }
}
