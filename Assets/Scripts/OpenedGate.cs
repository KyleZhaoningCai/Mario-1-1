using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenedGate : MonoBehaviour {

    public GameObject leftGate;
    public GameObject rightGate;

    private float gateOpeningTime = 1.5f;

    void Update()
    {

        gateOpeningTime -= Time.deltaTime;
        if (gateOpeningTime >= 0)
        {
            leftGate.transform.position = new Vector3(leftGate.transform.position.x - Time.deltaTime, leftGate.transform.position.y, leftGate.transform.position.z);
            rightGate.transform.position = new Vector3(rightGate.transform.position.x + Time.deltaTime, rightGate.transform.position.y, -3.0f);
        }
    }
}
