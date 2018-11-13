using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEnding : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        GameObject P1 = GameObject.Find("P1");
        GameObject P2 = GameObject.Find("P2");
        if(P1.transform.position.y<=0)
        {
            this.transform.position = 336*Vector2.right+160*Vector2.up;
            GameObject Button = GameObject.Find("GameOver");
            Text text = Button.transform.Find("Text").GetComponent<Text>();
            text.text = "The White Player Wins!";
            P1.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            P2.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
            foreach (GameObject t in tiles)
            {
                t.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
        }
        if (P2.transform.position.y <= 0)
        {
            this.transform.position = 336 * Vector2.right + 160 * Vector2.up;
            GameObject Button = GameObject.Find("GameOver");
            Text text = Button.transform.Find("Text").GetComponent<Text>();
            text.text = "The Black Player Wins!";
            P1.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            P2.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
            foreach (GameObject t in tiles)
            {
                t.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
        }
    }
}
