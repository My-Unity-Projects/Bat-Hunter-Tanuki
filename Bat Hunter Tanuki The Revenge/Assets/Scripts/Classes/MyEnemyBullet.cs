using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEnemyBullet : MonoBehaviour {

    bool isReady;
    float speed = 3f;
    Vector2 idirection;

    // Use this for initialization
    void Start () {

        Destroy(this.gameObject, 2.5f);
	}
	
	// Update is called once per frame
	void Update () {
        bulletMovement();
	}

    public void SetDirection(Vector2 direction)
    {
        idirection = direction.normalized;
        isReady = true;
    }

    public void bulletMovement()
    {
        if (isReady)
        {
            Vector2 position = transform.position;
            position += idirection * speed * Time.deltaTime;

            transform.position = position;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }
}
