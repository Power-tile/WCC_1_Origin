using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2move : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public float speed = (float)0.1;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.J))
        {
            Vector3 position = this.transform.position;
            position.x-= speed;
            this.transform.position = position;
        }
        if (Input.GetKey(KeyCode.L))
        {
            Vector3 position = this.transform.position;
            position.x+=speed;
            this.transform.position = position;
        }
        if (Input.GetKey(KeyCode.I))
        {
            Vector3 position = this.transform.position;
            position.z+=speed;
            this.transform.position = position;
        }
        if (Input.GetKey(KeyCode.K))
        {
            Vector3 position = this.transform.position;
            position.z-=speed;
            this.transform.position = position;
        }
    }
}
