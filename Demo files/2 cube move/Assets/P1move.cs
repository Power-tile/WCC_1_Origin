using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1move : MonoBehaviour {

    Vector3 position;

    // Use this for initialization
    void Start () {
        position = this.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            position.x--;
            if((position.x<0)&& (position.x>-10) && (position.z < 5) && (position.x > -5))
            {
                GetComponent<Renderer>().material.color = new Color(0, 0, 0);
            }
            else
            {
                GetComponent<Renderer>().material.color = new Color(85, 85, 85);
            }
            this.transform.position = position;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Vector3 position = this.transform.position;
            position.x++;
            this.transform.position = position;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Vector3 position = this.transform.position;
            position.z++;
            this.transform.position = position;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Vector3 position = this.transform.position;
            position.z--;
            this.transform.position = position;
        }
    }
}
