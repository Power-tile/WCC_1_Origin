using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : Init {
    public int category; // Stores the category of the item
    public int type; // Stores the type of the item
    public string name; // Stores the name of the item type
    public float mass; // mass of the item

    public Tile currentTile; // The tile which this resource lies on
    public float rotateSpeed = 0.01f; // The rotating speed of the resource

    private Vector3 velocity;
    
    void Start() {
        GetBasicInformation();

        CheckCurrentTile();
    }

    void Update() {
        RotateSelf();
        CheckCurrentTile();
        PhysicsDownFall();
    }

    private void RotateSelf() {
        this.transform.RotateAround(this.transform.position, Vector3.up, 1.0f * rotateSpeed * Time.deltaTime);
    }

    private void CheckCurrentTile() {
        RaycastHit hit;

        Physics.Raycast(this.transform.position, -Vector3.up, out hit, 1);
        if (hit.collider != null) {
            GameObject x = hit.collider.gameObject;
            while (x.tag != "Tile") {
                x = x.transform.parent.gameObject;
            }

            currentTile = x.GetComponent<Tile>();
        }
    }

    private void GetBasicInformation() {
        foreach (Item item in ItemTemplate[category]) {
            if (item.type == type) {
                name = item.name;
                mass = item.mass;

                return;
            }
        }
    }

    private void PhysicsDownFall() {
        if (Vector3.Distance(transform.position, currentTile.gameObject.transform.position) > 0) {

        }
    }
}
