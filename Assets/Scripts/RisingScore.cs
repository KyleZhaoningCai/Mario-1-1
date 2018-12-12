using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingScore : MonoBehaviour {

    private float duration = 1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime, -4);
        duration -= Time.deltaTime;
        if (duration <= 0)
        {
            Destroy(this.gameObject);
        }
	}
}
