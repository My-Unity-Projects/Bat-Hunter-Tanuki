using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MyBoss : MonoBehaviour
{

    [Header("Transition Settings")]
    public Animator transitionAni;
    string sceneName;

    public Animator ani;

    public GameObject enemyBullet;

    public GameObject bossBullet;

    public GameObject room;

    [Header("Health Settings")]
    public int health;
    bool hit = false;
    float temp = 0;
    int cont = 0;
    bool isDead = false;

    [Header("Movement Settings")]
    public float speed;
    Vector3 targetPosition;
    Vector3 currentPosition;
    int xOryFirst = -1;

    [Header("General Atack Settings")]
    bool isAtacking = false;
    float atackTemp = 5f;

    [Header("Atack One Settings")]
    bool one = false;
    public GameObject shadowPrefab;
    GameObject shadow = null;
    Vector3 shadowTargetPosition;

    [Header("Atack Two Settings")]
    bool two = false;
    Vector3 movementPosition;
    bool readyTwo = false;
    float fireTemp = 0.25f;
    int movementCont = 0;

    [Header("Atack Three Settings")]
    bool three = false;
    bool readyThree = false;
    Animator parentAni;
    float threeTemp = 6f;


    [Header("Tanuki Settings")]
    GameObject Tanuki;


    // Use this for initialization
    void Start()
    {

        transitionAni = GameObject.Find("Transition Canvas").transform.GetChild(0).GetComponent<Animator>();

        Tanuki = GameObject.Find("Tanuki");
        ani = transform.GetComponent<Animator>();

        // Movement
        targetPosition = transform.position;

        // Atack One
        shadowTargetPosition = Tanuki.transform.position;

        // Atack Two
        movementPosition = new Vector3(0, 1.8f, 0);

        // Atack Three
        parentAni = transform.parent.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!isDead && Tanuki.GetComponent<PlayerMovement>().startBossFight)
        {
            if (!isAtacking)
            {
                Movement();
            }

            else if (isAtacking)
            {
                if (one)
                {
                    atackOne();
                }

                if (two)
                {
                    atackTwo();
                }

                if (three)
                {
                    atackThree();
                }
            }

            hitAnimation();
            atack();
        }
    }

    public void Movement()
    {
        if (transform.position == targetPosition)
        {
            targetPosition.x = Random.Range(-7, 8) * 0.64f + room.transform.position.x;
            targetPosition.y = Random.Range(-4, 4) * 0.64f + room.transform.position.y;

            xOryFirst = Random.Range(0, 2);
        }

        else
        {
            if (xOryFirst == 0)
            {
                if (targetPosition.x > transform.position.x)
                {
                    ani.SetBool("Up", false);
                    ani.SetBool("Down", false);
                    ani.SetBool("Right", true);
                    ani.SetBool("Left", false);
                }

                else if (targetPosition.x < transform.position.x)
                {
                    ani.SetBool("Up", false);
                    ani.SetBool("Down", false);
                    ani.SetBool("Right", false);
                    ani.SetBool("Left", true);
                }

                currentPosition = Vector3.MoveTowards(transform.position, new Vector3(targetPosition.x, transform.position.y, 0), speed * Time.deltaTime);
                transform.position = currentPosition;


                if (targetPosition.x == transform.position.x && targetPosition.y != transform.position.y)
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
                currentPosition = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, targetPosition.y, 0), speed * Time.deltaTime);
                transform.position = currentPosition;

                if (targetPosition.x != transform.position.x && targetPosition.y == transform.position.y)
                {
                    xOryFirst = 0;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;

        if (tag == "Weapon")
        {
            GameObject.FindObjectOfType<AudioManager>().Play("Kill Enemy");
            hit = true;
            Camera.main.transform.parent.GetComponent<Animator>().SetTrigger("shake");
            health -= 30;

            if (health <= 0)
            {
                ani.SetBool("Death", true);
                transform.GetComponent<BoxCollider2D>().enabled = false;
                transform.GetChild(0).transform.gameObject.SetActive(false);
                parentAni.enabled = false;
                speed = 0;
                isDead = true;
                sceneName = "Kinematic Two";
                StartCoroutine(LoadScene());
            }
        }
    }

    public void hitAnimation()
    {
        if (hit)
        {
            transform.GetComponent<BoxCollider2D>().enabled = false;
            // transform.GetComponent<Rigidbody2D>().simulated = false;

            temp -= Time.deltaTime;

            if (temp < 0)
            {
                if (cont % 2 == 0)
                {
                    transform.GetComponent<SpriteRenderer>().enabled = false;
                    transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
                }

                else
                {
                    transform.GetComponent<SpriteRenderer>().enabled = true;
                    transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
                }

                if (cont == 0 || cont == 1 || cont == 2)
                {
                    temp = 0.1f;
                    cont++;
                }

                else if (cont == 3 || cont == 4 || cont == 5 || cont == 6)
                {
                    temp = 0.05f;
                    cont++;
                }

                else if (cont == 7 || cont == 8 || cont == 9 || cont == 10 || cont == 11)
                {
                    temp = 0.02f;
                    cont++;
                }

                else if (cont == 12)
                {
                    cont = 0;
                    hit = false;
                    transform.GetComponent<SpriteRenderer>().enabled = true;
                    transform.GetComponent<BoxCollider2D>().enabled = true;
                    // transform.GetComponent<Rigidbody2D>().simulated = true;
                    transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
                }

            }
        }
    }


    // Special attacks

    public void atack()
    {
        atackTemp -= Time.deltaTime;

        if (atackTemp < 0 && !isAtacking)
        {
            isAtacking = true;

            int atack = Random.Range(1, 3);

            if (atack == 0)
            {
                one = true;
            }

            else if (atack == 1)
            {
                two = true;

                ani.SetBool("Up", false);
                ani.SetBool("Down", true);
                ani.SetBool("Right", false);
                ani.SetBool("Left", false);
            }

            else if (atack == 2)
            {
                three = true;

                ani.SetBool("Up", false);
                ani.SetBool("Down", true);
                ani.SetBool("Right", false);
                ani.SetBool("Left", false);
            }
        }
    }

    /*Special atack one still in process*/
    public void atackOne()
    {
        Vector3 atackPosition = new Vector3(transform.position.x, 7f, 0);

        if (shadow == null)
        {
            shadow = Instantiate(shadowPrefab, transform.position, Quaternion.identity);
            shadow.transform.localScale = new Vector3(3, 3, 3);
        }

        if (transform.position != atackPosition)
        {
            Vector3 position = Vector3.MoveTowards(transform.position, atackPosition, 0.7f);
            transform.position = position;
        }

        else
        {
            if (shadow.transform.position == shadowTargetPosition)
            {
                shadowTargetPosition.x = Random.Range(-7, 8) * 0.64f;
                shadowTargetPosition.y = Random.Range(-4, 5) * 0.64f;

                GameObject shadowCopy = Instantiate(shadowPrefab, shadow.transform.position, Quaternion.identity);
                GameObject bossBulletCopy = Instantiate(bossBullet, new Vector3(shadow.transform.position.x, 5f, 0), Quaternion.identity);
                Vector2 direction = bossBulletCopy.transform.position - shadowCopy.transform.position;
                bossBullet.GetComponent<MyBossBullet>().SetDirection(direction);

            }

            else
            {
                Vector3 position = Vector3.MoveTowards(shadow.transform.position, shadowTargetPosition, 0.1f);
                shadow.transform.position = position;
            }

            /*isAtacking = false;
            one = false;
            atackTemp = 7;*/
        }
    }

    public void atackTwo()
    {
        Vector3 atackPosition = new Vector3(0, 1.8f, 0);

        if (transform.position != atackPosition && !readyTwo)
        {
            Vector3 position = Vector3.MoveTowards(transform.position, atackPosition, 0.06f);
            transform.position = position;
        }

        else
        {
            readyTwo = true;
            fireTemp -= Time.deltaTime;

            if (transform.position == movementPosition)
            {
                movementCont++;

                if (movementPosition.x == 0)
                {
                    movementPosition.x = 7 * 0.64f;
                }

                else
                {
                    movementPosition.x = movementPosition.x * -1;
                }

                if (movementCont == 6)
                {
                    isAtacking = false;
                    atackTemp = 7f;
                    two = false;
                    movementPosition = new Vector3(0, 1.8f, 0);
                    readyTwo = false;
                    fireTemp = 0.25f;
                    movementCont = 0;
                }

            }

            else
            {
                Vector3 position = Vector3.MoveTowards(transform.position, movementPosition, 0.07f);
                transform.position = position;
            }

            if (fireTemp < 0)
            {
                shootTwo();
                fireTemp = 0.25f;
            }

        }

    }

    public void atackThree()
    {
        Vector3 atackPosition = new Vector3(0, -0.64f, 0);

        if (transform.position != atackPosition && !readyThree)
        {
            Vector3 position = Vector3.MoveTowards(transform.position, atackPosition, 0.06f);
            transform.position = position;
        }

        else
        {
            readyThree = true;

            if (parentAni.GetBool("atack") == false)
            {
                parentAni.SetBool("atack", true);
            }

            else
            {
                threeTemp -= Time.deltaTime;

                if (threeTemp < 0)
                {
                    isAtacking = false;
                    three = false;
                    readyThree = false;
                    threeTemp = 6;
                    parentAni.SetBool("atack", false);
                    atackTemp = 7;
                }
            }
        }

    }

    public void shootTwo()
    {
        GameObject bullet = (GameObject)Instantiate(enemyBullet);
        bullet.transform.position = transform.position + new Vector3(0, 0.48f, 0);
        Vector2 direction = new Vector3(bullet.transform.position.x, -7, 0) - bullet.transform.position;
        bullet.GetComponent<MyEnemyBullet>().SetDirection(direction);
        GameObject.FindObjectOfType<AudioManager>().Play("Bat Attack");

    }

    public void normalShoot()
    {
        GameObject Tanuki = GameObject.Find("Tanuki");
        int chance = Random.Range(0, 4);
        GameObject.FindObjectOfType<AudioManager>().Play("Bat Attack");

        if (Tanuki != null && Tanuki.GetComponent<PlayerMovement>().currentRoom == room && chance > 0 && !isAtacking)
        {
            GameObject bullet = (GameObject)Instantiate(enemyBullet);
            bullet.transform.position = transform.position + new Vector3(0, 0.48f, 0);
            Vector2 direction = Tanuki.transform.position - bullet.transform.position;
            bullet.GetComponent<MyEnemyBullet>().SetDirection(direction);
        }
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(2f);
        StartCoroutine(LoadSceneTwo());
    }

    IEnumerator LoadSceneTwo()
    {
        transitionAni.SetTrigger("End");
        StartCoroutine(FadeSound());
        yield return new WaitForSeconds(1f);
        PlayerPrefs.SetInt("Lifes", 6);
        PlayerPrefs.SetInt("Leaves", 0);
        SceneManager.LoadScene(sceneName);
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

    public void PlayBatWings()
    {
        GameObject.FindObjectOfType<AudioManager>().Play("Bat Wings");
    }
}
