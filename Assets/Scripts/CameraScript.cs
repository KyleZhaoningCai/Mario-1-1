using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;

    private GameObject _player;


    // Use this for initialization
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (_player != null)
        {
            float x = Mathf.Clamp(_player.transform.position.x, xMin, xMax);
            float y = Mathf.Clamp(_player.transform.position.y, yMin, yMax);
            gameObject.transform.position = new Vector3(x, y, gameObject.transform.position.z);
        }
        
    }
}
