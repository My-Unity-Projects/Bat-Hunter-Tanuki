using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TransitionWait : MonoBehaviour {

	// Use this for initialization
	void Start () {

        // StartGame();
            
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartGame()
    {
        if(SceneManager.GetActiveScene().name == "BossFight")
        {
           PlayerMovement pm = GameObject.Find("Tanuki").GetComponent<PlayerMovement>();
           pm.startBossFight = true;
        }

        else if(SceneManager.GetActiveScene().name == "Kinematic One" || SceneManager.GetActiveScene().name == "Kinematic Two")
        {
            StartCoroutine(StartDialogue());
        }
            
    }

    IEnumerator StartDialogue()
    {
        yield return new WaitForSeconds(1f);
        GameObject.FindObjectOfType<DialogueManager>().GetComponent<DialogueTrigger>().TriggerDialogue();
    }
}
