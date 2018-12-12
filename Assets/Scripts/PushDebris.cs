using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushDebris : MonoBehaviour {

    public float upForce;
    public float sideForce;


	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(sideForce, upForce));
	}
}
