using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinHit : MonoBehaviour {

    private float moveDuration = 0.3f;
    private Vector3 originalPosition;
    private bool stop = false;
    private float destroyTimer = 1.0f;

    void Start()
    {
        originalPosition = new Vector3 (transform.position.x, transform.position.y - 0.05f, transform.position.z);
        GameObject.FindWithTag("GameController").GetComponent<GameController>().Score("200", transform.position);
        GameObject.FindWithTag("GameController").GetComponent<GameController>().Coin();
    }

    // Update is called once per frame
    void Update()
    {
        if (!stop)
        {
            moveDuration -= Time.deltaTime;
            if (moveDuration >= 0)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime * 2, transform.position.z);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, originalPosition, Time.deltaTime * 2);
                if (transform.position.y <= originalPosition.y)
                {
                    stop = true;
                }
            }
        }
        destroyTimer -= Time.deltaTime;
        if (destroyTimer <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
