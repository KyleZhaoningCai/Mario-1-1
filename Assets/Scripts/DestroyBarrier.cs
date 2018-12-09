using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBarrier : MonoBehaviour {

    public GameObject explosion;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Threat"))
        {
            Instantiate(explosion, new Vector3(52.17f, 6.0f, 0), transform.rotation);
            Instantiate(explosion, new Vector3(52.17f, 3.5f, 0), transform.rotation);
            Instantiate(explosion, new Vector3(52.17f, 1f, 0), transform.rotation);
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }
}
