using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour {

    public GameObject[] enemies;
    public Transform[] spawnPoints;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                Instantiate(enemies[i], spawnPoints[i].position, enemies[i].transform.rotation);
            }
            Destroy(this.gameObject);
        }        
    }
}
