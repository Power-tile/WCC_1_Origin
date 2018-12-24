using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : Init {
    public int category; // Stores the category of the item
    public int type; // Stores the type of the item
    public string name; // Stores the name of the item type
    public float mass; // mass of the item
    public float rotateSpeed; // The rotating speed of the resource

    public Tile currentTile; // The tile which this resource lies on

    private Vector3 velocity;
    
    void Start() {
        GetBasicInformation();

        CheckCurrentTile();

        IgnoreCollider();
    }

    void Update() {
        RotateSelf();
    }

    private void RotateSelf() {
        this.transform.RotateAround(this.transform.position, Vector3.up, 1.0f * rotateSpeed * Time.deltaTime);
    }

    private void CheckCurrentTile() {
        RaycastHit hit;

        Physics.Raycast(this.transform.position, -Vector3.up, out hit, 1);
        if (hit.collider != null) {
            GameObject x = hit.collider.gameObject;

            currentTile = x.GetComponent<Tile>();
        }
    }

    private void GetBasicInformation() {
        foreach (Item item in ItemTemplate[category]) {
            if (item.type == type) {
                name = item.name;
                mass = item.mass;
                rotateSpeed = item.rotateSpeed;

                return;
            }
        }
    }

    private void IgnoreCollider() {
        if (currentTile != null) {
            if (!currentTile.insight) {
                Physics.IgnoreCollision(this.GetComponent<Collider>(), currentTile.gameObject.transform.Find("Fog").GetComponent<Collider>(), true);
            }
            if (currentTile.selectable || currentTile.selected || currentTile.current) {
                Physics.IgnoreCollision(this.GetComponent<Collider>(), currentTile.gameObject.transform.Find("Select").GetComponent<Collider>(), true);
            }
        }
    }
}
