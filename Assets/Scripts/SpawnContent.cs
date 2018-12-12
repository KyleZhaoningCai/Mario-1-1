using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnContent : MonoBehaviour {

    public GameObject spawnContent;
    public GameObject spawnContentAlt;
    public GameObject postHit;
    public bool onlyBig;

    private bool stop = true;
    private Vector3 originalPosition;
    private float moveDuration = 0.05f;
    private AudioSource clip;

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
                if (onlyBig && collision.gameObject.GetComponent<PlayerController>().bodyStatus == 0)
                {
                    stop = false;
                    clip.Play();
                }
                else
                {
                    if (postHit != null)
                    {
                        Instantiate(postHit, transform.position, transform.rotation);
                    }
                    if (spawnContent != null && collision.gameObject.GetComponent<PlayerController>().bodyStatus == 0)
                    {
                        Instantiate(spawnContent, new Vector3(transform.position.x, transform.position.y + 0.05f, 3), transform.rotation);
                    }
                    else if (collision.gameObject.GetComponent<PlayerController>().bodyStatus != 0)
                    {
                        if (spawnContentAlt != null)
                        {
                            Instantiate(spawnContentAlt, new Vector3(transform.position.x, transform.position.y + 0.05f, 3), transform.rotation);
                        }
                        else
                        {
                            Instantiate(spawnContent, new Vector3(transform.position.x, transform.position.y + 0.05f, 3), transform.rotation);

                        }
                    }
                    Destroy(this.gameObject);
                }                
            }
        }
    }
}
