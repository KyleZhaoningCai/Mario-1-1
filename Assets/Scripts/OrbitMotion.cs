using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitMotion : MonoBehaviour {

    public GameObject centerObject;
    public float radius = 10f;
    public float angle = 2f;
    public float rotateSpeed = 0.8f;
    public float rightAngle = 2f;
    public float leftAngle = 4f;
    public float speedRotate = 1.0f;
    public bool clockwise = true;

    private Vector2 centre;
    

    private void Start()
    {
        centre = centerObject.transform.position;
    }

    private void Update()
    {
        if (clockwise)
        {
            angle += rotateSpeed * Time.deltaTime;
            transform.Rotate(new Vector3(0, 0, -1) * speedRotate * Time.deltaTime);

        }
        else
        {
            angle -= rotateSpeed * Time.deltaTime;
            transform.Rotate(Vector3.forward * speedRotate * Time.deltaTime);
        }
        if (angle >= leftAngle && leftAngle != 0)
        {
            clockwise = false;
        }
        if (angle <= rightAngle && rightAngle != 0)
        {
            clockwise = true;
        }

        var offset = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * radius;
        transform.position = centre + offset;
    }
}
