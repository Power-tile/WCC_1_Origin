using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : TacticsMove {
    public bool moving = false; // marking if the player is moving or not
    public float moveSpeed; // storing the movingspeed of the player
    public float jumpVelocity; // storing the jumpVelocity of the player

    public int selfNumber; // storing the number of the player

    public int maxEyeOfPlayer; // the max eye point of the player
    public int maxMoveOfPlayer; // the max move point of the player

    public int currentEyeOfPlayer; // the current eye point of the player
    public int currentMoveOfPlayer; // the current move point of the player

    // Use this for initialization
    void Start() {
        maxEyeOfPlayer = maxeye; // set player's eye point data to default
        maxMoveOfPlayer = maxmove; // set player's move point data to default

        moving = false; // player is not moving
        selfNumber = int.Parse(this.name.Split('r')[1]); // get the player's number from player's name

        moveSpeed = 2.0f; // set the moveSpeed of the player
        jumpVelocity = 4.5f; // set the jumpVelocity of the player

        currentMoveOfPlayer = maxMoveOfPlayer; // reset move point of player
    }

    // Update is called once per frame
    void Update() {
        if (currentPlayerNumber == selfNumber) {
            Debug.DrawRay(transform.position, transform.forward); // debug ray, showing the player's facing direction
            ShowCurrentMovePoint(); // show the current move point of the player

            if (!moving) {
                FindPath(this); // find the possible tiles which the player can go to
                CheckMouse(); // check the position of the mouse (showing move point cost and go to tile when clicked)
            } else {
                Move(this); // complete the action of moving
            }
        }
    }

    void CheckMouse() {
        bool flag = false; // if the mouse is resting on a tile
        bool has_value = false; // if the tile has a valid move point cost value
        Camera cam = this.transform.Find("Camera").gameObject.GetComponent<Camera>(); // get the player's camera
        Ray ray = cam.ScreenPointToRay(Input.mousePosition); // emit a ray from the mouse
        Text text = GameObject.Find("Canvas").transform.Find("RequiredMovePointNumber").gameObject.GetComponent<Text>();

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            GameObject now = hit.collider.gameObject;
            if (now.tag == "Select" || now.tag == "Fog") { // mouse resting on select/fog
                now = now.transform.parent.gameObject; // find the tile
                flag = true;
            } else if (now.tag == "Tile") { // mouse resting on tile directly
                flag = true;
            } else { // mouse not resting on tile/select/fog

            }

            if (flag) { // now is a Tile
                Tile t = now.GetComponent<Tile>(); // convert GameObject now to Tile t
                if (t.selectable) { // the player can go to Tile t
                    has_value = true; // tile has a vaild move point cost value, player can move to this tile
                    text.text = t.movedis.ToString() + "  "; // show the cost of moving to this tile

                    if (Input.GetMouseButtonUp(0)) MoveToTile(this, t); // if mouse clicked, move to this tile
                }
            }
        }

        if (!has_value || !flag) { // mouse not resting on a tile OR player cannot go to the tile
            text.text = "0  "; // no "cost" displayed
        }
    }

    public void Initialize() {
        this.currentMoveOfPlayer = this.maxMoveOfPlayer;
        this.currentEyeOfPlayer = this.maxEyeOfPlayer;
    }

    public void Clear() {
        GameObject[] list = GameObject.FindGameObjectsWithTag("Tile");

        foreach (GameObject tile in list) {
            tile.GetComponent<Tile>().Initialize();
        }
    }

    private void ShowCurrentMovePoint() {
        Text text = GameObject.Find("Canvas").transform.Find("CurrentMovePointNumber").gameObject.GetComponent<Text>();

        //Debug.Log(currentMovePointNumberText);
        text.text = currentMoveOfPlayer.ToString() + "  ";
    }
}
