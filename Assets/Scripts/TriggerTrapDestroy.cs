using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTrapDestroy : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject.transform.parent.gameObject);
        }
    }
}
