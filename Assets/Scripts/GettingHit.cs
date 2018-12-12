using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GettingHit : MonoBehaviour {

    private float moveDuration = 0.05f;
    private Vector3 originalPosition;
    private bool justSpawn = true;

    void Start()
    {
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update () {
        if (justSpawn)
        {
            moveDuration -= Time.deltaTime;
            if (moveDuration >= 0)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime * 5, transform.position.z);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, originalPosition, Time.deltaTime * 5);
                if (transform.position.y <= originalPosition.y)
                {
                    justSpawn = false;
                }
            }
        }     
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if ((collision.gameObject.transform.position.y + collision.gameObject.GetComponent<Renderer>().bounds.size.y / 2) < (transform.position.y - GetComponent<Renderer>().bounds.size.y / 2) && collision.gameObject.GetComponent<Rigidbody2D>().velocity.y >= 0)
            {
                GetComponent<AudioSource>().Play();
            }
        }
    }
}
