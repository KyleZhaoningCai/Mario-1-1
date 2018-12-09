using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour {

    public float trapSpeed = 0.5f;
    private bool upward;

	// Use this for initialization
	void Start () {
        upward = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (upward)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + trapSpeed * Time.deltaTime, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - trapSpeed * Time.deltaTime, transform.position.z);
        }
        if (transform.position.y >= 0.87)
        {
            upward = false;
        }
        if (transform.position.y <= -0.43)
        {
            upward = true;
        }
	}
}
