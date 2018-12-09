using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroy : MonoBehaviour {

    public GameObject key;
    public GameObject explosion;

    private float destroyTimer = 0.5f;

    void Update()
    {
        destroyTimer -= Time.deltaTime;
        if (destroyTimer <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Trigger"))
        {
            Destroy(this.gameObject);
        }
        if (collision.gameObject.CompareTag("Chest"))
        {
            GameObject keyClone = Instantiate(key, collision.gameObject.transform.GetChild(0).position, transform.rotation);
            keyClone.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 600f));
            Instantiate(explosion, collision.gameObject.transform.GetChild(0).position, transform.rotation);
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.CompareTag("Destroyable")){
            Destroy(collision.gameObject.transform.parent.gameObject);
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyController>().dead = true;
        }
    }
}
