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

    private bool grounded = false;
    private bool killed = false;
    private bool won = false;

    private float shootCooldown = 0;
    private float jumpCooldown = 0;
    private Rigidbody2D playerRigitbody;
    private Animator playerAnimator;
    private float dyingForce;
    private GameController gameController;
    private AudioSource[] sounds;


    // Use this for initialization
    void Start () {
        gameController = gameControllerObject.GetComponent<GameController>();
        playerRigitbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        dyingForce = jumpForce;
        sounds = GetComponents<AudioSource>();
    }

    void Update()
    {
        if (!killed && !won)
        {
            if (grounded && (Input.GetKey("up") || Input.GetKey("w")) && jumpCooldown <= 0)
            {
                sounds[0].Play();
                playerRigitbody.AddForce(new Vector2(0, jumpForce));
                jumpCooldown = jumpCooldownFactor;
            }
            if (Input.GetKey(KeyCode.Space) && shootCooldown <= 0)
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
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (!killed && !won)
        {
            float move = Input.GetAxis("Horizontal");
            playerRigitbody.velocity = new Vector2(move * maxSpeed, playerRigitbody.velocity.y);
            if (move > 0 && !facingRight)
            {
                Flip(this.gameObject);
            }
            else if (move < 0 && facingRight)
            {
                if (grounded)
                {
                    playerAnimator.SetInteger("state", 1);
                }
                Flip(this.gameObject);
            }
            if (grounded)
            {
                if (move != 0)
                {
                    playerAnimator.SetInteger("state", 1);
                }
                else
                {
                    playerAnimator.SetInteger("state", 0);
                }

            }
            else
            {
                playerAnimator.SetInteger("state", 2);
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
        playerAnimator.SetInteger("state", 3);
        if (facingRight)
        {
            GameObject bulletClone = Instantiate(bullet, transform.GetChild(0).position, transform.rotation);
            bulletClone.GetComponent<Rigidbody2D>().AddForce(new Vector2(1000f, 0));
        }
        else
        {
            GameObject bulletClone = Instantiate(bullet, transform.GetChild(0).position, transform.rotation);
            Flip(bulletClone);
            bulletClone.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1000f, 0));
        }
        
    }
    void OnCollisionStay2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Ground" || collision.gameObject.CompareTag("Chest"))
        {
            grounded = true;
            transform.parent = collision.transform;
        }
    }

    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("WinCondition"))
        {
            won = true;
            playerAnimator.SetInteger("state", 0);
            gameController.Win();
        }
        if (collision.collider.gameObject.CompareTag("Threat") || collision.collider.gameObject.CompareTag("Enemy"))
        {
            killed = true;
            transform.parent = null;
            playerAnimator.SetInteger("state", 4);
            GetComponent<Collider2D>().enabled = false;
            playerRigitbody.velocity = Vector3.zero;
            playerRigitbody.angularVelocity = 0;
            playerRigitbody.AddForce(new Vector2(0, dyingForce));
            sounds[3].Play();
            gameController.Die();
        }

        if (collision.gameObject.CompareTag("Key"))
        {
            sounds[2].Play();
            Destroy(collision.gameObject);
            haveKey = true;
            gameController.GainLoseKey(haveKey);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.CompareTag("Chest"))
        {
            grounded = false;
            transform.parent = null;
        }
    }

    public void LoseKey()
    {
        haveKey = false;
        gameController.GainLoseKey(haveKey);
        gameController.Checkpoint(transform.position);
    }
}
