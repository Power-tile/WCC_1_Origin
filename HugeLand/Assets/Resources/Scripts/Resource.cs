using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : Init {
    public int type; // Stores the type of the resource
    public Tile currentTile; // The tile which this resource lies on
    public int rotateSpeed = 100; // The rotating speed of the resource
    
    void Update() {
        RotateSelf();
    }

    private void RotateSelf() {
        this.transform.RotateAround(this.transform.position, Vector3.up, rotateSpeed * Time.deltaTime);
    }
}
