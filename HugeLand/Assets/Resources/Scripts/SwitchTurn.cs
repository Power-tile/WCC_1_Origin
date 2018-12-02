using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTurn : Init {
    public int currentPlayerNumber = 1;

	public void switchTurn()
    {
        int n = 0;
        if (GameObject.Find("Player1").transform.Find("Camera").gameObject.GetComponent<Camera>().enabled) { n = 1; }
        if (GameObject.Find("Player2").transform.Find("Camera").gameObject.GetComponent<Camera>().enabled) { n = 2; }
        if (GameObject.Find("Player3").transform.Find("Camera").gameObject.GetComponent<Camera>().enabled) { n = 3; }
        if (GameObject.Find("Player4").transform.Find("Camera").gameObject.GetComponent<Camera>().enabled) { n = 4; }
        Debug.Log(n);
        GameObject.Find("Player" + n.ToString()).transform.Find("Camera").gameObject.GetComponent<Camera>().enabled = false;
        n %= 4; n++;
        //GUI.Label(new Rect(Screen.width * 0.5f, Screen.height * 0.5f, 100, 30),"This is Player"+n.ToString()+"'s turn\n Please pass the device and press Space");
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
            GameObject.Find("Player" + n.ToString()).transform.Find("Camera").gameObject.GetComponent<Camera>().enabled = true;
        //}
    }

    void Start() {
        GameObject.Find("Main Camera").GetComponent<Camera>().enabled = false;
        GameObject.Find("Player1").transform.Find("Camera").gameObject.GetComponent<Camera>().enabled = true;
        GameObject.Find("Player2").transform.Find("Camera").gameObject.GetComponent<Camera>().enabled = false;
        GameObject.Find("Player3").transform.Find("Camera").gameObject.GetComponent<Camera>().enabled = false;
        GameObject.Find("Player4").transform.Find("Camera").gameObject.GetComponent<Camera>().enabled = false;
    }

    void Update() {
        // Switch to the next player
    }
}
