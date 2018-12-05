using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : TacticsMove {
    public bool moving = false; // marking if the player is moving or not
    public float moveSpeed; // storing the movingspeed of the player
    public float jumpVelocity; // storing the jumpVelocity of the player

    public int selfNumber; // storing the number of the player

    public int maxEyeOfPlayer = maxeye;
    public int maxMoveOfPlayer = maxmove;

	// Use this for initialization
	void Start () {
        moving = false;
        selfNumber = int.Parse(this.name.Split('r')[1]);
        moveSpeed = 2.0f;
        jumpVelocity = 4.5f;
	}
	
	// Update is called once per frame
	void Update () {
        if (currentPlayerNumber == selfNumber) {
            Debug.DrawRay(transform.position, transform.forward);

            if (!moving) {
                FindPath(maxEyeOfPlayer, maxMoveOfPlayer);
                CheckMouse();
            } else {
                Move(this);
            }
        }
    }

    void CheckMouse() {
        if (Input.GetMouseButtonUp(0)) {
            Camera cam = this.transform.Find("Camera").gameObject.GetComponent<Camera>();
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                GameObject now = hit.collider.gameObject;
                while (now.tag != "Tile") {
                    now = now.transform.parent.gameObject;
                }

                Tile t = now.GetComponent<Tile>();
                if (t.selectable) {
                    MoveToTile(this, t);
                }
            }
        }
    }
}
