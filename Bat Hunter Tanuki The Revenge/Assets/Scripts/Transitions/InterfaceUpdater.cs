using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceUpdater : MonoBehaviour {

    public Sprite greyLife;

    [Header("UI Life Settings")]
    public GameObject[] lifes;
    public Sprite redLife;

    [Header("UI Leave Settings")]
    public GameObject[] leaves;
    public Sprite greenLeave;


    PlayerMovement pm;

	// Use this for initialization
	void Start () {

        pm = GameObject.Find("Tanuki").GetComponent<PlayerMovement>();
	}
	
	// Update is called once per frame
	void Update () {

        updateLifesUI();
        updateLeavesUI();
	}

    public void updateLifesUI()
    {    
        if(pm.health == 6)
        {
            lifes[0].GetComponent<Image>().sprite = redLife;
            lifes[0].GetComponent<Animator>().enabled = true;
            lifes[0].transform.localScale = new Vector3(1, 1, 1);
        }

        else
        {
            lifes[0].GetComponent<Image>().sprite = greyLife;
            lifes[0].GetComponent<Animator>().enabled = false;
            lifes[0].transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        }

        if (pm.health >= 5)
        {
            lifes[1].GetComponent<Image>().sprite = redLife;
            lifes[1].GetComponent<Animator>().enabled = true;
            lifes[1].transform.localScale = new Vector3(1, 1, 1);
        }

        else
        {
            lifes[1].GetComponent<Image>().sprite = greyLife;
            lifes[1].GetComponent<Animator>().enabled = false;
            lifes[1].transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        }


        if (pm.health >= 4)
        {
            lifes[2].GetComponent<Image>().sprite = redLife;
            lifes[2].GetComponent<Animator>().enabled = true;
            lifes[2].transform.localScale = new Vector3(1, 1, 1);
        }

        else
        {
            lifes[2].GetComponent<Image>().sprite = greyLife;
            lifes[2].GetComponent<Animator>().enabled = false;
            lifes[2].transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        }


        if (pm.health >= 3)
        {
            lifes[3].GetComponent<Image>().sprite = redLife;
            lifes[3].GetComponent<Animator>().enabled = true;
            lifes[3].transform.localScale = new Vector3(1, 1, 1);
        }

        else
        {
            lifes[3].GetComponent<Image>().sprite = greyLife;
            lifes[3].GetComponent<Animator>().enabled = false;
            lifes[3].transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        }


        if (pm.health >= 2)
        {
            lifes[4].GetComponent<Image>().sprite = redLife;
            lifes[4].GetComponent<Animator>().enabled = true;
            lifes[4].transform.localScale = new Vector3(1, 1, 1);
        }

        else
        {
            lifes[4].GetComponent<Image>().sprite = greyLife;
            lifes[4].GetComponent<Animator>().enabled = false;
            lifes[4].transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        }


        if (pm.health >= 1)
        {
            lifes[5].GetComponent<Image>().sprite = redLife;
            lifes[5].GetComponent<Animator>().enabled = true;
            lifes[5].transform.localScale = new Vector3(1, 1, 1);
        }

        else
        {
            lifes[5].GetComponent<Image>().sprite = greyLife;
            lifes[5].GetComponent<Animator>().enabled = false;
            lifes[5].transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        }
    }

    public void updateLeavesUI()
    {
        for(int j=0; j<leaves.Length; j++)
        {
            leaves[j].GetComponent<Image>().sprite = greyLife;
            leaves[j].GetComponent<Animator>().enabled = false;
            leaves[j].transform.localScale = new Vector3(0.65f, 0.65f, 0.65f);
        }

        for(int i = leaves.Length - 1; i > leaves.Length - 1 - pm.leaves; i--)
        {
            leaves[i].GetComponent<Image>().sprite = greenLeave;
            leaves[i].GetComponent<Animator>().enabled = true;
            leaves[i].transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        }
    }
}
