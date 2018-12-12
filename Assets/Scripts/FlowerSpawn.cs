using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSpawn : MonoBehaviour {

    private Vector3 targetPosition;
    private PlayerController playerC;


    // Use this for initialization
    void Start () {
        playerC = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        targetPosition = new Vector3(transform.position.x, transform.position.y + GetComponent<Renderer>().bounds.size.y - 0.05f, transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y < targetPosition.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 0.15f);
        }
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerC.bodyStatus == 0)
            {
                playerC.EatGrowMushroom();
                
                
            }
            else
            {
                playerC.EatFlower();
            }
            Destroy(this.gameObject);
        }
    }
}
