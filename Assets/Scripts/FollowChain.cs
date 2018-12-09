using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowChain : MonoBehaviour {

    public float moveSpeed;
    public Transform[] points;

    private Transform currentPoint;
    private int pointIndex;

	// Use this for initialization
	void Start () {
        pointIndex = 0;
        currentPoint = points[pointIndex];
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.MoveTowards(transform.position, currentPoint.position, Time.deltaTime * moveSpeed);
        if (transform.position == currentPoint.position)
        {
            if (pointIndex == points.Length - 1)
            {
                pointIndex = 0;
            }
            else
            {
                pointIndex++;
            }
                currentPoint = points[pointIndex];
        }
	}
}
