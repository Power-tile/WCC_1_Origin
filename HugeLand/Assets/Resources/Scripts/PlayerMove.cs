using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : TacticsMove {
    public bool moving = false; // marking if the player is moving or not
    public float moveSpeed; // storing the movingspeed of the player
    public float jumpVelocity; // storing the jumpVelocity of the player

    public int selfNumber; // storing the number of the player

    public int maxEyeOfPlayer;
    public int maxMoveOfPlayer;

    public int currentEyeOfPlayer;
    public int currentMoveOfPlayer;

    // Use this for initialization
    void Start() {
        maxEyeOfPlayer = maxeye;
        maxMoveOfPlayer = maxmove;

        moving = false;
        selfNumber = int.Parse(this.name.Split('r')[1]);

        moveSpeed = 2.0f;
        jumpVelocity = 4.5f;

        currentMoveOfPlayer = maxMoveOfPlayer;
    }

    // Update is called once per frame
    void Update() {
        if (currentPlayerNumber == selfNumber) {
            Debug.DrawRay(transform.position, transform.forward);

            if (!moving) {
                FindPath(this);
                CheckMouse();
            }
            else {
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
}
