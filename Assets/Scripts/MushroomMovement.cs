using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomMovement : MonoBehaviour {

    private Vector3 startMovingPosition;
    private Rigidbody2D rb;
    private bool goingRight = true;
    private Collider2D c2d;
    private bool startMoving = false;

	// Use this for initialization
	void Start () {
        startMovingPosition = new Vector3(transform.position.x, transform.position.y + GetComponent<Renderer>().bounds.size.y - 0.05f, transform.position.z);
        rb = GetComponent<Rigidbody2D>();
        c2d = GetComponent<Collider2D>();
        c2d.enabled = false;
        rb.isKinematic = true;
    }
	
	// Update is called once per frame
	void Update () {
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
                startMoving = true;
            }
        }
        else
        {
            if (goingRight)
            {
                rb.velocity = new Vector2(0.5f, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(-0.5f, rb.velocity.y);
            }
        }
		
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Ground"))
        {
            goingRight = !goingRight;
        }
    }
}
