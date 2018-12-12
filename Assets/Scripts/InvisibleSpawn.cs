using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleSpawn : MonoBehaviour
{

    public GameObject spawnContent;
    public GameObject postHit;
    public GameObject equalHeight;

    private bool stop = true;
    private Vector3 originalPosition;
    private float moveDuration = 0.05f;
    private AudioSource clip;
    private Collider2D c2d;
    private GameObject postHitObject;
    private float resetTimer = 0.5f;
    private bool resetStart = false;
    private GameObject player;
    private Collider2D playerC2d;

    void Start()
    {
        originalPosition = transform.position;
        clip = GetComponent<AudioSource>();
        c2d = GetComponent<Collider2D>();
        player = GameObject.FindWithTag("Player");
        playerC2d = player.GetComponent<Collider2D>();
    }

    void Update()
    {
        if (!stop)
        {
            moveDuration -= Time.deltaTime;
            if (moveDuration >= 0)
            {
                postHitObject.transform.position = new Vector3(postHitObject.transform.position.x, postHitObject.transform.position.y + Time.deltaTime * 5, postHitObject.transform.position.z);
            }
            else
            {
                postHitObject.transform.position = Vector3.MoveTowards(postHitObject.transform.position, originalPosition, Time.deltaTime * 5);
                if (transform.position.y <= originalPosition.y)
                {
                    stop = true;
                    Destroy(this.gameObject);
                }
            }
        }

        if (resetStart)
        {
            resetTimer -= Time.deltaTime;
            if (resetTimer <= 0)
            {
                Physics2D.IgnoreCollision(playerC2d, c2d, false);
                resetTimer = 0.5f;
                resetStart = false;
            }
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (((collision.gameObject.transform.position.y + collision.gameObject.GetComponent<Renderer>().bounds.size.y / 2) < (transform.position.y - equalHeight.GetComponent<Renderer>().bounds.size.y / 2)) && (collision.gameObject.GetComponent<Rigidbody2D>().velocity.y >= 0))
            {
                postHitObject = Instantiate(postHit, transform.position, transform.rotation);
                stop = false;
                clip.Play();                
                Instantiate(spawnContent, new Vector3(transform.position.x, transform.position.y + 0.05f, 3), transform.rotation);
            }
            else
            {
                Physics2D.IgnoreCollision(collision.collider, c2d);
                resetStart = true;
            }
        }
    }
}

