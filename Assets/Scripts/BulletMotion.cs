using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMotion : MonoBehaviour
{

    public GameObject explosion;
    public GameObject explostionSoundless;

    private Rigidbody2D rb;
    private float destroyTimer = 0.8f;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.x < 3.0f && rb.velocity.x > -3.0f)
        {
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
        destroyTimer -= Time.deltaTime;
        if (destroyTimer <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Ground") && !collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                collision.gameObject.GetComponent<EnemyController>().KillEnemyInstant();
                Instantiate(explostionSoundless, transform.position, transform.rotation);
            }
            else
            {
                Instantiate(explosion, transform.position, transform.rotation);
            }            
            Destroy(this.gameObject);
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (rb.velocity.x > 0)
            {
                rb.velocity = (new Vector2(3f, 2f));
            }
            else
            {
                rb.velocity = (new Vector2(-3f, 2f));
            }           
        }
    }
}
