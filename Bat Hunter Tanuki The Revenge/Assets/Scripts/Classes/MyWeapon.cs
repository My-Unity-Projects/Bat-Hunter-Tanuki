using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyWeapon : MonoBehaviour {

    PlayerMovement pm;

	// Use this for initialization
	void Start () {

        pm = transform.parent.GetComponent<PlayerMovement>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;

        if (tag == "Leave")
        {
            Destroy(collision.gameObject);
            GameObject.FindObjectOfType<AudioManager>().Play("Take Item");

            if (pm.leaves < 12)
            {
                pm.leaves += 1;
            }

        }

        if (tag == "Life")
        {
            if (pm.health < 6)
            {
                Destroy(collision.gameObject);
                pm.health += 1;
                GameObject.FindObjectOfType<AudioManager>().Play("Take Item");
            }
        }

        if (tag == "Door")
        {
            MyDoor md = collision.gameObject.GetComponent<MyDoor>();
            if (md.opened)
            {
                if (md.bossDoor)
                {
                    PlayerPrefs.SetInt("Lifes", pm.health);
                    PlayerPrefs.SetInt("Leaves", pm.leaves);
                    pm.sceneName = "BossFight";
                    StartCoroutine(pm.LoadScene());

                }

                else
                {
                    pm.changeRoom(md.side, md.roomTo);
                }
            }
            else
                pm.FixDirection();

        }
    }
}
