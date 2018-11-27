using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour {
    public void RotateLeft()
    {
        int n = 0;
        if (GameObject.Find("Player1").transform.Find("Camera").gameObject.GetComponent<Camera>().enabled) { n = 1; }
//        if (GameObject.Find("Player2").transform.Find("Camera").gameObject.GetComponent<Camera>().enabled) { n = 2; }
//        if (GameObject.Find("Player3").transform.Find("Camera").gameObject.GetComponent<Camera>().enabled) { n = 3; }
//        if (GameObject.Find("Player4").transform.Find("Camera").gameObject.GetComponent<Camera>().enabled) { n = 4; }
        GameObject CurrentPlayer = GameObject.Find("Player" + n.ToString());
        CurrentPlayer.transform.Rotate(Vector3.up, 10, Space.Self);
        //CurrentPlayer.transform.rotation = Quaternion.Euler(0.0f, CurrentPlayer.transform.rotation.y - 10.0f, 0.0f);
    }

    public void RotateRight()
    {
        int n = 0;
        if (GameObject.Find("Player1").transform.Find("Camera").gameObject.GetComponent<Camera>().enabled) { n = 1; }
//        if (GameObject.Find("Player2").transform.Find("Camera").gameObject.GetComponent<Camera>().enabled) { n = 2; }
//        if (GameObject.Find("Player3").transform.Find("Camera").gameObject.GetComponent<Camera>().enabled) { n = 3; }
//        if (GameObject.Find("Player4").transform.Find("Camera").gameObject.GetComponent<Camera>().enabled) { n = 4; }
        GameObject CurrentPlayer = GameObject.Find("Player" + n.ToString());
        CurrentPlayer.transform.Rotate(Vector3.up, -10, Space.Self);
        //CurrentPlayer.transform.rotation = Quaternion.Euler(0.0f, CurrentPlayer.transform.rotation.y + 10.0f, 0.0f);
    }
}
