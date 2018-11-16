using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonControl : GameEnding
{
    public void Reset() {
        GameObject P1 = GameObject.Find("P1");
        GameObject P2 = GameObject.Find("P2");
        Vector3 pos1 = new Vector3(-2.5f, 1.7f, 0.0f);
        Vector3 pos2 = new Vector3(2.5f, 1.7f, 0.0f);
        //P1.transform.position = -2.5f * Vector3.left + 1.7f * Vector3.up;
        P1.transform.position = pos1;
        P1.transform.localEulerAngles = new Vector3(0, 0, 0);
        P1.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        //P2.transform.position = 2.5f * Vector3.left + 1.7f * Vector3.up;
        P2.transform.position = pos2;
        P2.transform.localEulerAngles = new Vector3(0, 0, 0);
        P2.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        foreach (GameObject t in tiles) {
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
        end = false;
    }
}
