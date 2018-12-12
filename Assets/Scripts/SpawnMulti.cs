using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMulti : MonoBehaviour
{

    public GameObject spawnContent;
    public GameObject postHit;

    private bool stop = true;
    private Vector3 originalPosition;
    private float moveDuration = 0.05f;
    private AudioSource clip;
    private int count = 6;

    void Start()
    {
        originalPosition = transform.position;
        clip = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!stop)
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
                    stop = true;
                    moveDuration = 0.05f;
                }
            }
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (((collision.gameObject.transform.position.y + collision.gameObject.GetComponent<Renderer>().bounds.size.y / 2) < (transform.position.y - GetComponent<Renderer>().bounds.size.y / 2)) && (collision.gameObject.GetComponent<Rigidbody2D>().velocity.y >= 0))
            {
                stop = false;
                clip.Play();
                Instantiate(spawnContent, new Vector3(transform.position.x, transform.position.y + 0.05f, 3), transform.rotation);
                count--;
                if (count == 0)
                {
                    Instantiate(postHit, transform.position, transform.rotation);
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
