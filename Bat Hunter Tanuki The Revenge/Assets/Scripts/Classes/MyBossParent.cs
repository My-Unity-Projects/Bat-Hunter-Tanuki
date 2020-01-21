using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyBossParent : MonoBehaviour {

    public GameObject spawnParent;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void shootThree()
    {
        if (spawnParent.transform.eulerAngles.z == 35)
        {
            spawnParent.transform.eulerAngles = new Vector3(0, 0, 0);
        }

        else
        {
            spawnParent.transform.eulerAngles = new Vector3(0, 0, 35);

        }

        for (int i = 0; i < 8; i++)
        {
            spawnParent.transform.GetChild(i).GetComponent<MyBulletSpawn>().shoot();
        }

    }
}
