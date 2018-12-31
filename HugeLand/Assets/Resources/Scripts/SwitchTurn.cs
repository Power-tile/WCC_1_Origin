using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTurn : Init {
    public void switchTurn() {
        /*
        int n = 0;
        if (GameObject.Find("Player1").transform.Find("Camera").gameObject.GetComponent<Camera>().enabled) { n = 1; }
        if (GameObject.Find("Player2").transform.Find("Camera").gameObject.GetComponent<Camera>().enabled) { n = 2; }
        if (GameObject.Find("Player3").transform.Find("Camera").gameObject.GetComponent<Camera>().enabled) { n = 3; }
        if (GameObject.Find("Player4").transform.Find("Camera").gameObject.GetComponent<Camera>().enabled) { n = 4; }
        Debug.Log(n);
        */
        //GUI.Label(new Rect(Screen.width * 0.5f, Screen.height * 0.5f, 100, 30), "This is Player" + n.ToString() + "'s turn\n Please pass the device and press Space");
        /*
        GameObject.Find("Player" + n.ToString()).transform.Find("Camera").gameObject.GetComponent<Camera>().enabled = false;
        n %= 4; n++;
        GameObject.Find("Player" + n.ToString()).transform.Find("Camera").gameObject.GetComponent<Camera>().enabled = true;
        currentPlayerNumber = n;
        */

        GameObject.Find("Player" + currentPlayerNumber.ToString()).transform.Find("Camera").gameObject.GetComponent<Camera>().enabled = false;
        GameObject.Find("Player" + currentPlayerNumber.ToString()).transform.Find("Camera").gameObject.SetActive(false);
        GameObject.Find("Player" + currentPlayerNumber.ToString()).GetComponent<PlayerMove>().Clear();
        currentPlayerNumber = ((currentPlayerNumber + 1) % 4 == 0) ? 4 : (currentPlayerNumber + 1) % 4;
        //Debug.Log(currentPlayerNumber);
        GameObject.Find("Player" + currentPlayerNumber.ToString()).GetComponent<PlayerMove>().Initialize();
        GameObject.Find("Player" + currentPlayerNumber.ToString()).transform.Find("Camera").gameObject.SetActive(true);
        GameObject.Find("Player" + currentPlayerNumber.ToString()).transform.Find("Camera").gameObject.GetComponent<Camera>().enabled = true;
    }

    void Start() {
        currentPlayerNumber = 1;

        GameObject.Find("Main Camera").GetComponent<Camera>().enabled = false;
        GameObject.Find("Player1").transform.Find("Camera").gameObject.GetComponent<Camera>().enabled = true;
        GameObject.Find("Player2").transform.Find("Camera").gameObject.GetComponent<Camera>().enabled = false;
        GameObject.Find("Player3").transform.Find("Camera").gameObject.GetComponent<Camera>().enabled = false;
        GameObject.Find("Player4").transform.Find("Camera").gameObject.GetComponent<Camera>().enabled = false;

        GameObject.Find("Player1").transform.Find("Camera").gameObject.SetActive(true);
        GameObject.Find("Player2").transform.Find("Camera").gameObject.SetActive(false);
        GameObject.Find("Player3").transform.Find("Camera").gameObject.SetActive(false);
        GameObject.Find("Player4").transform.Find("Camera").gameObject.SetActive(false);
    }

    void Update() {

    }
}
