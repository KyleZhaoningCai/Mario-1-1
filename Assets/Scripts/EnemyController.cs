using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public Transform[] waypoints;
    public float enemySpeed = 5.0f;
    public bool dead = false;

    private int waypointIndex;
    private bool facingRight;
    private Animator enemyAnimator;
    private bool playedOnce = false;

	// Use this for initialization
	void Start () {
        waypointIndex = 0;
        facingRight = true;
        enemyAnimator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (dead)
        {
            enemyAnimator.SetInteger("dead", 1);
            if (!playedOnce)
            {
                GetComponent<AudioSource>().Play();
                playedOnce = true;
            }
            
            GetComponent<Collider2D>().enabled = false;
   
        }
        else
        {
            if (facingRight)
            {
                transform.position = new Vector3(transform.position.x + enemySpeed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
            {
                transform.position = new Vector3(transform.position.x - enemySpeed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            if (Vector3.Distance(transform.position, waypoints[waypointIndex % 2].position) < 0.5)
            {
                waypointIndex++;
                Vector3 theScale = gameObject.transform.localScale;
                theScale.x *= -1;
                gameObject.transform.localScale = theScale;
                facingRight = !facingRight;
            }
        }
        
	}
}
