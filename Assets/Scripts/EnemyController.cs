using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public float enemySpeed = 1.0f;
    public int turtleStatus = 0;
    public bool dead = false;
    public bool motionPaused = false;

    private bool facingRight;
    private Animator enemyAnimator;
    private bool playedOnce = false;
    private PlayerController playerC;
    private float removeTimer = 1f;
    private Rigidbody2D rb;
    private bool reviving = false;
    private float reviveTimer = 5f;
    private bool isReviving = false;
    private bool pushed = false;
    private AudioSource[] sounds;


    // Use this for initialization
    void Start () {
        facingRight = false;
        enemyAnimator = GetComponent<Animator>();
        playerC = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        sounds = GetComponents<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (reviving)
        {
            reviveTimer -= Time.deltaTime;
            if (reviveTimer <= 0)
            {
                enemyAnimator.SetInteger("state", 2);
                reviveTimer = 1.5f;
                isReviving = true;
                reviving = false;
            }
        }

        else if (isReviving)
        {
            reviveTimer -= Time.deltaTime;
            if (reviveTimer <= 0)
            {
                enemyAnimator.SetInteger("state", 0);
                reviveTimer = 6f;
                isReviving = false;
                turtleStatus = 1;
            }
        }
        else if (dead)
        {
            removeTimer -= Time.deltaTime;
            if (removeTimer <= 0)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            if (!motionPaused)
            {
                if (facingRight)
                {
                    if (pushed)
                    {
                        rb.velocity = new Vector2(3f, rb.velocity.y);
                    }
                    else
                    {
                        rb.velocity = new Vector2(enemySpeed, rb.velocity.y);
                    }
                    
                }
                else
                {
                    if (pushed)
                    {
                        rb.velocity = new Vector2(-3f, rb.velocity.y);
                    }
                    else
                    {
                        rb.velocity = new Vector2(enemySpeed * -1, rb.velocity.y);
                    }                    
                }
            }
        }  
        
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Ground") && !collision.gameObject.CompareTag("Player"))
        {
            
            if (collision.gameObject.CompareTag("Enemy") && turtleStatus == 3)
            {
                collision.gameObject.GetComponent<EnemyController>().KillEnemyInstant();
            }
            else
            {
                Vector3 theScale = gameObject.transform.localScale;
                theScale.x *= -1;
                gameObject.transform.localScale = theScale;
                facingRight = !facingRight;
                if (!collision.gameObject.CompareTag("Enemy") && !collision.gameObject.CompareTag("Bullet") && turtleStatus == 3)
                {
                    sounds[1].Play();
                }
            }
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            if (playerC.tempInvincible)
            {
                Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
            }
            else if (playerC.bodyStatus == 3 || playerC.bodyStatus == 4 || playerC.bodyStatus == 5)
            {
                KillEnemyInstant();
            }
            else if (turtleStatus == 2)
            {
                PushEnemy();
            }
            else
            {
                if (collision.gameObject.GetComponent<Rigidbody2D>().velocity.y < 0)
                {
                    if (turtleStatus == 0)
                    {
                        KillEnemy();
                    }
                    else if (turtleStatus == 1)
                    {
                        DisableEnemy();
                    }
                    else if (turtleStatus == 3)
                    {
                        DisableEnemy();
                    }
                    collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 100f));
                }
                else
                {
                    if (playerC.bodyStatus == 0)
                    {
                        playerC.KillPlayer();
                    }
                    else if (playerC.bodyStatus == 1 || playerC.bodyStatus == 2)
                    {
                        playerC.ShrinkPlayer();
                    }                    
                }
            }
        }
    }

    public void KillEnemy()
    {
        enemyAnimator.SetInteger("state", 1);
        dead = true;
        GetComponent<Collider2D>().enabled = false;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        rb.gravityScale = 0;
        pushed = false;
        sounds[0].Play();
        GameObject.FindWithTag("GameController").GetComponent<GameController>().Score("100", transform.position);
    }

    public void KillEnemyInstant()
    {
        dead = true;
        pushed = false;
        GetComponent<Collider2D>().enabled = false;
        Vector3 theScale = gameObject.transform.localScale;
        theScale.y *= -1;
        gameObject.transform.localScale = theScale;
        if (turtleStatus != 0)
        {
            enemyAnimator.SetInteger("state", 1);
        }
        if (playerC.gameObject.transform.position.x <= transform.position.x)
        {
            rb.velocity = new Vector2(1f, 3f);
            rb.gravityScale = 1;
        }
        else
        {
            rb.velocity = new Vector2(-1f, 3f);
            rb.gravityScale = 1;
        }
        sounds[0].Play();
        if (turtleStatus == 0)
        {
            GameObject.FindWithTag("GameController").GetComponent<GameController>().Score("100", transform.position);
        }
        else
        {
            GameObject.FindWithTag("GameController").GetComponent<GameController>().Score("200", transform.position);
        }
    }

    public void DisableEnemy()
    {
        enemyAnimator.SetInteger("state", 1);
        turtleStatus = 2;
        rb.velocity = Vector2.zero;
        sounds[0].Play();
        reviving = true;
        reviveTimer = 6f;
        pushed = false;
        GameObject.FindWithTag("GameController").GetComponent<GameController>().Score("100", transform.position);
    }

    public void PushEnemy()
    {
        enemyAnimator.SetInteger("state", 1);
        turtleStatus = 3;
        reviving = false;
        isReviving = false;
        reviveTimer = 6f;
        pushed = true;
        if (playerC.gameObject.transform.position.x < transform.position.x)
        {
            if (!facingRight)
            {
                Vector3 theScale = gameObject.transform.localScale;
                theScale.x *= -1;
                gameObject.transform.localScale = theScale;
                facingRight = !facingRight;
            }
        }
        else
        {
            if (facingRight)
            {
                Vector3 theScale = gameObject.transform.localScale;
                theScale.x *= -1;
                gameObject.transform.localScale = theScale;
                facingRight = !facingRight;
            }
        }
        GameObject.FindWithTag("GameController").GetComponent<GameController>().Score("500", transform.position);
    }

    public void ResetPhysics()
    {
        Physics2D.IgnoreCollision(playerC.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
    }
}
