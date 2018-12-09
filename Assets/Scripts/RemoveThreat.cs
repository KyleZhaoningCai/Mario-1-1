using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveThreat : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            this.gameObject.tag = "Ground";
            this.transform.parent.gameObject.tag = "Ground";
            GetComponent<AudioSource>().Play();
        }
    }
}
