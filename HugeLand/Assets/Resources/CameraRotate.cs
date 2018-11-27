using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour {
    public void RotateLeft()
    {
        int n = 0;
        if (GameObject.Find("Player1").GetComponent<Camera>().enabled) n = 1;
        if (GameObject.Find("Player2").GetComponent<Camera>().enabled) n = 2;
        if (GameObject.Find("Player3").GetComponent<Camera>().enabled) n = 3;
        if (GameObject.Find("Player4").GetComponent<Camera>().enabled) n = 4;
        GameObject.Find("Player"+n.ToString()).transform.GetComponent<Camera>().transform.Rotate(Vector3.up, -10, Space.Self);
    }

    public void RotateRight()
    {
        int n = 0;
        if (GameObject.Find("Player1").GetComponent<Camera>().enabled) n = 1;
        if (GameObject.Find("Player2").GetComponent<Camera>().enabled) n = 2;
        if (GameObject.Find("Player3").GetComponent<Camera>().enabled) n = 3;
        if (GameObject.Find("Player4").GetComponent<Camera>().enabled) n = 4;
        GameObject.Find("Player" + n.ToString()).transform.GetComponent<Camera>().transform.Rotate(Vector3.up, 10, Space.Self);
    }
}
