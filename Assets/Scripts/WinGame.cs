using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinGame : MonoBehaviour {

    private GameObject playerObject;
    private bool flagMovingDown = false;
    public GameObject flag;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (flagMovingDown)
        {
            if (flag.transform.position.y <= 0.34)
            {
                playerObject.GetComponent<PlayerController>().flagBottomed = true;
                flagMovingDown = false;
            }
            else
            {
                flag.transform.position = new Vector3(flag.transform.position.x, flag.transform.position.y - Time.deltaTime, flag.transform.position.z);
            }
        }        
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject.FindWithTag("GameController").GetComponent<GameController>().Win();
            float heightPercentage = (collision.transform.position.y - GameObject.FindWithTag("FinalTile").GetComponent<Renderer>().bounds.size.y) / this.gameObject.GetComponent<Renderer>().bounds.size.y;
            float score = 5000 * heightPercentage;
            string finalScore = (Math.Round(score / 100, 0) * 100) + "";
            GameObject.FindWithTag("GameController").GetComponent<GameController>().Score(finalScore, collision.transform.position);
            playerObject = collision.gameObject;
            playerObject.GetComponent<PlayerController>().Win();
            flagMovingDown = true;
        }
    }
}
