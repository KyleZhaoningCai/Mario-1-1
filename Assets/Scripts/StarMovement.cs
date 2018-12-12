using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarMovement : MonoBehaviour
{

    private Vector3 startMovingPosition;
    private Rigidbody2D rb;
    private bool goingRight = true;
    private Collider2D c2d;
    private bool startMoving = false;
    private bool firstLeave = true;
    private float changeStatusTimer = 0.3f;
    private bool startCounting = false;
    private bool startBouncing = false;

    // Use this for initialization
    void Start()
    {
        startMovingPosition = new Vector3(transform.position.x, transform.position.y + GetComponent<Renderer>().bounds.size.y - 0.05f, transform.position.z);
        rb = GetComponent<Rigidbody2D>();
        c2d = GetComponent<Collider2D>();
        c2d.enabled = false;
        rb.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!startMoving)
        {
            if (transform.position.y < startMovingPosition.y)
            {
                transform.position = Vector3.MoveTowards(transform.position, startMovingPosition, Time.deltaTime * 0.15f);
            }
            else
            {
                c2d.enabled = true;
                rb.isKinematic = false;
                rb.AddForce(new Vector2(0, 150f));
                startMoving = true;
            }
        }
        else
        {
            if (goingRight)
            {
                rb.velocity = new Vector2(0.7f, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(-0.7f, rb.velocity.y);
            }
        }

        if (startCounting)
        {
            changeStatusTimer -= Time.deltaTime;
            if (changeStatusTimer <= 0)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, -1);
                startBouncing = true;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Ground"))
        {
            if (collision.gameObject.CompareTag("Reflect"))
            {
                goingRight = false;
            }
        }
        else
        {
            if (startBouncing)
            {
                if (goingRight)
                {
                    rb.velocity = new Vector2(0.7f, 2.5f);
                }
                else
                {
                    rb.velocity = new Vector2(-0.7f, 2.5f);
                }
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && firstLeave)
        {
            startCounting = true;
            firstLeave = false;
        }
    }
}
