using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetractingPlatform : MonoBehaviour {

    public float platformSpeed = 0.5f;

    private bool movingLeft;

    // Use this for initialization
    void Start()
    {
        movingLeft = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (movingLeft)
        {
            transform.position = new Vector3(transform.position.x - platformSpeed * Time.deltaTime, transform.position.y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x + platformSpeed * Time.deltaTime, transform.position.y, transform.position.z);
        }
        if (transform.position.x >= 137.37)
        {
            movingLeft = true;
        }
        if (transform.position.x <= 134.3)
        {
            movingLeft = false;
        }
    }
}
