using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyBulletSpawn : MonoBehaviour {

    public GameObject bullet;
    float bulletSpeed = 3f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void shoot()
    {
        Rigidbody2D bulletInstance;
        bulletInstance = Instantiate(bullet.GetComponent<Rigidbody2D>(), transform.position, Quaternion.identity);
        bulletInstance.name = "Bullet(Clone)";
        bulletInstance.velocity = transform.up * bulletSpeed;

        Destroy(bulletInstance.gameObject, 2);
    }
}
