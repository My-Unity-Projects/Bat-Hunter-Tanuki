using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKinematic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayBatWings()
    {
        GameObject.FindObjectOfType<AudioManager>().Play("Bat Wings");
    }

    public void PlayBatAttack()
    {
        GameObject.FindObjectOfType<AudioManager>().Play("Bat Attack");

    }
}
