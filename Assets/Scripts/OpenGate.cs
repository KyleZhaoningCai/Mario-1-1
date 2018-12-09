using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGate : MonoBehaviour {

    public GameObject openedGate;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<PlayerController>().haveKey)
            {
                collision.gameObject.GetComponent<PlayerController>().LoseKey();
                Instantiate(openedGate, transform.parent.transform.position, transform.rotation);
                Destroy(this.transform.parent.gameObject);
            }
        }
    }
}
