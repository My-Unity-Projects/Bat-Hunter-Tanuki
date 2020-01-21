using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class KinematicOne : MonoBehaviour {

    public Animator transitionAni;

    GameObject dialogueManager;
    Animator dialogueAni;

    public int dialogueCont = 1;

    bool isEvent = false;


    // Actors
    public GameObject tanuki;
    public GameObject bat;
    public GameObject leaves;

    bool isBatMovingRight;
    bool isBatMovingLeft;

    bool isTanukiMoving;

    Vector3 exitPosition = new Vector3(-4.48f, 0, 0);
    Vector3 leavesPosition = new Vector3(1, 0, 0);

	// Use this for initialization
	void Start () {

        dialogueManager = GameObject.Find("DialogueManager");
        dialogueAni = GameObject.Find("Dialogue Canvas").GetComponent<Animator>();

        bat.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

        if(Input.GetKeyDown(KeyCode.RightArrow) && !isEvent)
        {
            isEvent = true;
            dialogueCont++;
            GameObject.FindObjectOfType<AudioManager>().Play("Select Option");
            
            if(dialogueCont < 5 || dialogueCont > 7)
            {
                dialogueAni.SetTrigger("Disappear");
                Invoke("DialogueTimeLine", 1f);
            }

            else
                DialogueTimeLine();
        }		

        if(isBatMovingRight)
        {
            BatMovesRight();
        }

        if(isBatMovingLeft)
        {
            BatMovesLeft();
        }

        if(isTanukiMoving)
        {
            PlayerMovesLeft();
        }
	}

    public void DialogueTimeLine()
    {
        if(dialogueCont == 2)
        {
            BatShowsUp();
        }

        else if(dialogueCont == 3)
        {
            BatApproach();
        }

        else if(dialogueCont == 4)
        {
            BatLeaves();
        }

        else if(dialogueCont == 5)
        {
            dialogueManager.GetComponent<DialogueManager>().DisplayNextSentence();
            isEvent = false;
        }

        else if(dialogueCont == 6)
        {
            dialogueManager.GetComponent<DialogueManager>().DisplayNextSentence();
            isEvent = false;
        }

        else if(dialogueCont == 7)
        {
            dialogueManager.GetComponent<DialogueManager>().DisplayNextSentence();
            isEvent = false;
        }

        else if(dialogueCont == 8)
        {
            PlayerLeaves();
        }
    }

    // Kinematic events

    public void BatShowsUp()
    {
        bat.SetActive(true);
        GameObject.FindObjectOfType<AudioManager>().Play("Bat Appears");
        StartCoroutine(NextSenetece());
    }

    public void BatApproach()
    {
        isBatMovingRight = true;
    }

    public void BatMovesRight()
    {
        Vector3 currentPosition = Vector3.MoveTowards(bat.transform.position, leavesPosition, 3f * Time.deltaTime);
        bat.transform.position = currentPosition;

        if(bat.transform.position == leavesPosition)
        {
            GameObject.FindObjectOfType<AudioManager>().Play("Leaves Steal");
            leaves.SetActive(false);
            isBatMovingRight = false;
            StartCoroutine(NextSenetece());
        }
    }

    public void BatLeaves()
    {
        isBatMovingLeft = true;
        bat.GetComponent<Animator>().SetTrigger("Left");
    }

    public void BatMovesLeft()
    {
        Vector3 currentPosition = Vector3.MoveTowards(bat.transform.position, exitPosition, 3f * Time.deltaTime);
        bat.transform.position = currentPosition;

        if (bat.transform.position == exitPosition)
        {
            bat.SetActive(false);
            isBatMovingLeft = false;
            GameObject.FindObjectOfType<AudioManager>().Play("Bat Appears");
            StartCoroutine(NextSenetece());
        }
    }

    public void PlayerLeaves()
    {
        isTanukiMoving = true;

        tanuki.GetComponent<Animator>().SetBool("Idle", false);
        tanuki.GetComponent<Animator>().SetBool("WalkingLeft", true);
    }

    public void PlayerMovesLeft()
    {
        Vector3 currentPosition = Vector3.MoveTowards(tanuki.transform.position, exitPosition, 3f * Time.deltaTime);
        tanuki.transform.position = currentPosition;

        if (tanuki.transform.position == exitPosition)
        {
            tanuki.SetActive(false);
            isTanukiMoving = false;
            GameObject.FindObjectOfType<AudioManager>().Play("Bat Appears");
            StartCoroutine(LoadScene());
        }
    }


    IEnumerator NextSenetece()
    {
        yield return new WaitForSeconds(0.5f);
        dialogueAni.SetTrigger("Appear");
        dialogueManager.GetComponent<DialogueManager>().DisplayNextSentence();
        isEvent = false;
    }

    IEnumerator LoadScene()
    {
        transitionAni.SetTrigger("End");
        StartCoroutine(FadeSound());
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("SampleScene");
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
}
