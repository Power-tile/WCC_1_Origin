using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1move : MonoBehaviour {

    Vector3 position;

    // Use this for initialization
    void Start () {
        //position = this.transform.position;
         
    }

    public float speed = (float)0.1;

	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.A))
        {
            Vector3 position = this.transform.position;
            position.x-= speed;
            this.transform.position = position;
        }
        if (Input.GetKey(KeyCode.D))
        {
            Vector3 position = this.transform.position;
            position.x+=speed;
            this.transform.position = position;
        }
        if (Input.GetKey(KeyCode.W))
        {
            Vector3 position = this.transform.position;
            position.z+=speed;
            this.transform.position = position;
        }
        if (Input.GetKey(KeyCode.S))
        {
            Vector3 position = this.transform.position;
            position.z-=speed;
            this.transform.position = position;
        }
    }
}
