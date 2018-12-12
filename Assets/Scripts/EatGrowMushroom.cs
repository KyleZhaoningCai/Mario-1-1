using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatGrowMushroom : MonoBehaviour {

    private PlayerController playerC;

    // Use this for initialization
    void Start () {
        playerC = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerC.EatGrowMushroom();
            Destroy(this.gameObject);
        }        
    }
}
