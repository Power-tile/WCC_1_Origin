using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : TacticsMove {
    public bool moving = false;
    public int selfNumber;

	// Use this for initialization
	void Start () {
        moving = false;
        selfNumber = (int)(this.name.Split('r')[1][0] - '0');
	}
	
	// Update is called once per frame
	void Update () {
        if (currentPlayer == selfNumber) {
            Debug.DrawRay(transform.position, transform.forward);

            if (!moving) {
                //FindSelectableTiles();
                //CheckMouse();
                FindPath();
            }
            else {
                //Move();
            }
        }
    }

    void CheckMouse() {
        if (Input.GetMouseButtonUp(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.tag == "Tile") {
                    Tile t = hit.collider.GetComponent<Tile>();
                    if (t.selectable) {
                        //MoveToTile(t);
                    }
                }
            }
        }
    }
}
