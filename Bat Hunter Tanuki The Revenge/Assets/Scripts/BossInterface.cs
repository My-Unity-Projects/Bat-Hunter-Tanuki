using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossInterface : MonoBehaviour {

    public Slider bossLife;

    public GameObject boss;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if(boss == null)
        {
            boss = GameObject.Find("BossParent(Clone)").transform.GetChild(0).gameObject;
        }

        UpdateBossLife();

	}

    public void UpdateBossLife()
    {
        if(boss != null)
        {
            bossLife.value = boss.GetComponent<MyBoss>().health;
        }
    }
}
