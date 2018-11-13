using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonControl : MonoBehaviour
{
    public void Reset()
    {
        GameObject P1 = GameObject.Find("P1");
        GameObject P2 = GameObject.Find("P2");
        P1.transform.position =  (float)2.5 * Vector3.left + (float)1.7 * Vector3.up;
        P1.transform.localEulerAngles = new Vector3(0, 0, 0);
        P1.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        P2.transform.position = -(float)2.5 * Vector3.left + (float)1.7 * Vector3.up;
        P2.transform.localEulerAngles = new Vector3(0, 0, 0);
        P2.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        foreach (GameObject t in tiles)
        {
            t.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            t.transform.position = t.transform.position.x * Vector3.right + t.transform.position.z * Vector3.forward;
        }

        P1.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        P2.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

        GroundControl.timer = 0;
        for (int i = 1; i <= 12; i++)
            for (int j = 1; j <= 12; j++)
                GroundControl.fallen[i, j] = false;

        GameObject Gameover = GameObject.Find("GameOver");
        Gameover.transform.position = -200 * Vector2.right - 25 * Vector2.up;
    }
}
