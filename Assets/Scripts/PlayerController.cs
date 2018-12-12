using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    
    public bool haveKey = false;

    public float maxSpeed = 10f;
    public bool facingRight = true;
    public float jumpForce = 700f;
    public float jumpCooldownFactor = 0.5f;
    public float shootCooldownFactor = 0.2f;
    public GameObject bullet;
    public GameObject gameControllerObject;
    public float dyingSpeed = 5.0f;
    public int bodyStatus = 0;
    public bool tempInvincible = false;
    public bool uncontrollable = false;

    public bool grounded = false;
    private bool killed = false;
    private bool won = false;

    private float tempInvincibleTimer = 6f;
    private float shootCooldown = 0;
    private float jumpCooldown = 0;
    private Rigidbody2D playerRigitbody;
    private Animator playerAnimator;
    private float dyingForce;
    private GameController gameController;
    private AudioSource[] sounds;
    private bool motionPaused = false;
    private float pauseDuration = 1f;
    private Rigidbody2D[] allRigidbodies;
    private Vector2[] velocities;
    private float[] angularVelocities;
    private BoxCollider2D c2d;
    private bool isInvincible = false;
    private float invincibleTime = 12f;
    public bool flagBottomed = false;
    private bool winMove = false;
    private float winWait = 0.5f;
    private bool winMoving = false;
    private bool waitFlag = false;
    private bool playerBottomed = false;

    private float smallColliderSizeX = 0.1192513f;
    private float smallColliderSizeY = 0.1192513f;
    private float smallColliderOffsetX = 0;
    private float smallColliderOffsetY = 0.08173168f;
    private float bigColliderSizeX = 0.1385598f;
    private float bigColliderSizeY = 0.3169111f;
    private float bigColliderOffsetX = 0;
    private float bigColliderOffsetY = 0.1589669f;


    // Use this for initialization
    void Start () {
        gameController = gameControllerObject.GetComponent<GameController>();
        playerRigitbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        dyingForce = jumpForce;
        sounds = GetComponents<AudioSource>();
        c2d = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (transform.position.x >= 3.955843f)
        {
            gameController.Checkpoint(new Vector3(3.955843f, 0.3f, -5f));
        }
        if (transform.position.x >= 23.42f)
        {
            Destroy(this.gameObject);
        }
        if (waitFlag)
        {
            if (playerRigitbody.velocity.y == 0)
            {
                playerBottomed = true;
                if (bodyStatus == 0 || bodyStatus == 3)
                {
                    playerAnimator.SetInteger("state", 33);
                }
                else if (bodyStatus == 1 || bodyStatus == 4)
                {
                    playerAnimator.SetInteger("state", 34);
                }
                else if (bodyStatus == 2 || bodyStatus == 5)
                {
                    playerAnimator.SetInteger("state", 35);
                }
                waitFlag = false;
            }
        }
        else if (flagBottomed && playerBottomed)
        {
            ContinueWin();
            flagBottomed = false;
            playerBottomed = false;
        }
        else if (winMove)
        {
            winWait -= Time.deltaTime;
            if (winWait <= 0)
            {
                winMoving = true;
                winMove = false;
                gameController.GetComponent<GameController>().Clear();

            }
        }
        else if (!motionPaused && !killed && !uncontrollable)
        {
            if (grounded && (Input.GetKey("up") || Input.GetKey("w")) && jumpCooldown <= 0)
            {
                sounds[0].Play();
                if (Input.GetKey(KeyCode.Space))
                {
                    if (facingRight)
                    {
                        playerRigitbody.AddForce(new Vector2(200f, jumpForce));
                    }
                    else
                    {
                        playerRigitbody.AddForce(new Vector2(-200f, jumpForce));
                    }
                    
                }
                else
                {
                    playerRigitbody.AddForce(new Vector2(0, jumpForce));
                }
                    
                jumpCooldown = jumpCooldownFactor;
            }
            if (Input.GetKey(KeyCode.Space) && shootCooldown <= 0 && (bodyStatus == 2 || bodyStatus == 5))
            {
                Fire();
                shootCooldown = shootCooldownFactor;
            }
            if (jumpCooldown > 0)
            {
                jumpCooldown -= Time.deltaTime;
            }
            if (shootCooldown > 0)
            {
                shootCooldown -= Time.deltaTime;
            }
        }
        else if (motionPaused)
        {
            pauseDuration -= Time.deltaTime;
            if (pauseDuration <= 0)
            {
                ResumeMotion();
            }
        }

        if (isInvincible)
        {
            invincibleTime -= Time.deltaTime;
            if (invincibleTime <= 0)
            {
                isInvincible = false;
                if (bodyStatus == 3)
                {
                    bodyStatus = 0;
                }
                else if (bodyStatus == 4)
                {
                    bodyStatus = 1;
                }
                else if (bodyStatus == 5)
                {
                    bodyStatus = 2;
                }
            }
        }

        if (tempInvincible)
        {
            tempInvincibleTimer -= Time.deltaTime;
            if (tempInvincibleTimer <= 0)
            {
                tempInvincible = false;
                GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
                SpriteRenderer spRend = transform.GetComponent<SpriteRenderer>();
                Color col = spRend.color;
                col.a = 1f;
                spRend.color = col;
                for (int i = 0; i < allEnemies.Length; i++)
                {
                    allEnemies[i].GetComponent<EnemyController>().ResetPhysics();
                }
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (!motionPaused && !killed)
        {
            float move = 0;
            if (!uncontrollable)
            {
                move = Input.GetAxis("Horizontal");
                
            }
            else if (winMoving)
            {
                move = 1;
            }
            if (Input.GetKey(KeyCode.Space))
            {
                playerRigitbody.velocity = new Vector2(move * maxSpeed * 1.5f, playerRigitbody.velocity.y);
            }
            else
            {
                playerRigitbody.velocity = new Vector2(move * maxSpeed, playerRigitbody.velocity.y);
            }
               
            if (move > 0 && !facingRight)
            {
                Flip(this.gameObject);
            }
            else if (move < 0 && facingRight)
            {
                Flip(this.gameObject);
            }
            if (grounded)
            {
                if (move > 0.1f || move < -0.1f)
                {
                    if (bodyStatus == 0)
                    {
                        playerAnimator.SetInteger("state", 1);
                    }
                    else if (bodyStatus == 1)
                    {
                        playerAnimator.SetInteger("state", 6);
                    }
                    else if (bodyStatus == 2)
                    {
                        playerAnimator.SetInteger("state", 10);
                    }
                    else if (bodyStatus == 3)
                    {
                        playerAnimator.SetInteger("state", 14);
                    }
                    else if (bodyStatus == 4)
                    {
                        playerAnimator.SetInteger("state", 17);
                    }
                    else if (bodyStatus == 5)
                    {
                        playerAnimator.SetInteger("state", 20);
                    }
                }
                else
                {
                    if (bodyStatus == 0)
                    {
                        playerAnimator.SetInteger("state", 0);
                    }
                    else if (bodyStatus == 1)
                    {
                        playerAnimator.SetInteger("state", 5);
                    }
                    else if (bodyStatus == 2)
                    {
                        playerAnimator.SetInteger("state", 9);
                    }
                    else if (bodyStatus == 3)
                    {
                        playerAnimator.SetInteger("state", 13);
                    }
                    else if (bodyStatus == 4)
                    {
                        playerAnimator.SetInteger("state", 16);
                    }
                    else if (bodyStatus == 5)
                    {
                        playerAnimator.SetInteger("state", 19);
                    }
                }

            }
            else
            {
                if (bodyStatus == 0)
                {
                    playerAnimator.SetInteger("state", 2);
                }
                else if (bodyStatus == 1)
                {
                    playerAnimator.SetInteger("state", 7);
                }
                else if (bodyStatus == 2)
                {
                    playerAnimator.SetInteger("state", 11);
                }
                else if (bodyStatus == 3)
                {
                    playerAnimator.SetInteger("state", 15);
                }
                else if (bodyStatus == 4)
                {
                    playerAnimator.SetInteger("state", 18);
                }
                else if (bodyStatus == 5)
                {
                    playerAnimator.SetInteger("state", 21);
                }
            }
        }    
    }

    private void Flip(GameObject gameObject)
    {
        if (gameObject.CompareTag("Player"))
        {
            facingRight = !facingRight;
        }        
        Vector3 theScale = gameObject.transform.localScale;
        theScale.x *= -1;
        gameObject.transform.localScale = theScale;
    }

    private void Fire()
    {
        sounds[1].Play();
        if (bodyStatus == 2)
        {
            playerAnimator.SetInteger("state", 3);
        }
        else if (bodyStatus == 5)
        {
            playerAnimator.SetInteger("state", 22);
        }
        if (facingRight)
        {
            GameObject bulletClone = Instantiate(bullet, transform.GetChild(0).position, transform.rotation);
            bulletClone.GetComponent<Rigidbody2D>().velocity = (new Vector2(3f, 0));
        }
        else
        {
            GameObject bulletClone = Instantiate(bullet, transform.GetChild(0).position, transform.rotation);
            bulletClone.GetComponent<Rigidbody2D>().velocity = (new Vector2(-3f, 0));
            Flip(bulletClone);
        }
        
    }
    void OnCollisionStay2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = false;
        }
    }

    private void PauseMotion()
    {
        motionPaused = true;
        allRigidbodies = FindObjectsOfType<Rigidbody2D>();
        velocities = new Vector2[allRigidbodies.Length];
        angularVelocities = new float[allRigidbodies.Length];
        for (int i = 0; i < allRigidbodies.Length; i++)
        {
            if (allRigidbodies[i].gameObject.CompareTag("Player") || allRigidbodies[i].gameObject.CompareTag("Enemy"))
            {
                if (allRigidbodies[i].gameObject.CompareTag("Enemy"))
                {
                    allRigidbodies[i].gameObject.GetComponent<EnemyController>().motionPaused = true;
                }
                velocities[i] = allRigidbodies[i].velocity;
                angularVelocities[i] = allRigidbodies[i].angularVelocity;
                allRigidbodies[i].isKinematic = true;
                allRigidbodies[i].velocity = new Vector2(0, 0);
                allRigidbodies[i].angularVelocity = 0;
            }            
        }
    }

    private void ResumeMotion()
    {
        motionPaused = false;
        for (int i = 0; i < allRigidbodies.Length; i++)
        {
            if (allRigidbodies[i] != null && (allRigidbodies[i].gameObject.CompareTag("Player") || allRigidbodies[i].gameObject.CompareTag("Enemy")))
            {
                if (allRigidbodies[i].gameObject.CompareTag("Enemy"))
                {
                    allRigidbodies[i].gameObject.GetComponent<EnemyController>().motionPaused = false;
                }
                allRigidbodies[i].isKinematic = false;
                allRigidbodies[i].velocity = velocities[i];
                allRigidbodies[i].angularVelocity = angularVelocities[i];
            }    
        }
        pauseDuration = 1f;
    }

    public void EatGrowMushroom()
    {
        PauseMotion();
        if (bodyStatus == 0)
        {
            bodyStatus = 1;
            playerAnimator.SetInteger("state", 4);
        }
        else if (bodyStatus == 3)
        {
            bodyStatus = 4;
            playerAnimator.SetInteger("state", 23);
        }        
        sounds[4].Play();
        c2d.size = new Vector2(bigColliderSizeX, bigColliderSizeY);
        c2d.offset = new Vector2(bigColliderOffsetX, bigColliderOffsetY);
        gameController.GetComponent<GameController>().Score("1000", transform.position);
    }

    public void EatLifeMushroom()
    {
        sounds[4].Play();
        gameController.GetComponent<GameController>().Score("1up", transform.position);
    }

    public void EatFlower()
    {
        if (bodyStatus == 1)
        {
            PauseMotion();
            bodyStatus = 2;
            playerAnimator.SetInteger("state", 8);
        }
        else if (bodyStatus == 4)
        {
            PauseMotion();
            bodyStatus = 5;
            playerAnimator.SetInteger("state", 8);
        }        
        sounds[4].Play();
        gameController.GetComponent<GameController>().Score("1000", transform.position);
    }

    public void EatStar()
    {
        if (bodyStatus == 0)
        {
            bodyStatus = 3;
        }
        else if (bodyStatus == 1)
        {
            bodyStatus = 4;
        }
        else if (bodyStatus == 2)
        {
            bodyStatus = 5;
        }
        sounds[4].Play();
        isInvincible = true;
        invincibleTime = 15f;
        gameController.GetComponent<GameController>().Score("1000", transform.position);
        gameController.GetComponent<GameController>().Invincible();
    }

    public void KillPlayer()
    {
        killed = true;
        playerAnimator.SetInteger("state", 50);
        GetComponent<Collider2D>().enabled = false;
        playerRigitbody.velocity = Vector3.zero;
        playerRigitbody.angularVelocity = 0;
        playerRigitbody.AddForce(new Vector2(0, dyingForce));
        gameController.Die();
    }

    public void ShrinkPlayer()
    {
        PauseMotion();
        playerAnimator.SetInteger("state", 12);
        bodyStatus = 0;
        sounds[6].Play();
        tempInvincible = true;
        tempInvincibleTimer = 4f;
        SpriteRenderer spRend = transform.GetComponent<SpriteRenderer>();
        Color col = spRend.color;
        col.a = 0.5f;
        spRend.color = col;
        c2d.size = new Vector2(smallColliderSizeX, smallColliderSizeY);
        c2d.offset = new Vector2(smallColliderOffsetX, smallColliderOffsetY);
    }

    public void KillPlayerAnimationLess()
    {
        killed = true;
        playerRigitbody.velocity = Vector3.zero;
        playerRigitbody.angularVelocity = 0;
        gameController.Die();
    }

    public void Win()
    {
        uncontrollable = true;
        playerRigitbody.velocity = new Vector2(0, -1.2f);
        playerRigitbody.gravityScale = 0;
        if (bodyStatus == 0 || bodyStatus == 3)
        {
            playerAnimator.SetInteger("state", 30);
        }
        else if (bodyStatus == 1 || bodyStatus == 4)
        {
            playerAnimator.SetInteger("state", 31);
        }
        else if(bodyStatus == 2 || bodyStatus == 5)
        {
            playerAnimator.SetInteger("state", 32);
        }
        waitFlag = true;
    }

    public void ContinueWin()
    {
        playerRigitbody.gravityScale = 1;
        transform.position = new Vector3(transform.position.x + GetComponent<Renderer>().bounds.size.x, transform.position.y, transform.position.z);
        Flip(this.gameObject);
        winMove = true;
    }
}
