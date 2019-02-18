using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour {
    public int itemCategory = 0; // Stores the category of the item
    public int itemType = 0; // Stores the type of the item
    public string itemName; // Stores the name of the item type
    public float itemMass; // mass of the item
    public float itemRotateSpeed; // The rotating speed of the resource
    public float RotateCenterSpeed = 40;
    public float shiftSpeed = 0.2f;

    public bool status = false; // the status of the item: false - laying on the ground; true - on player
    public bool has_tile_parent = false; // the status of the item: false - no parent OR has player parent; true - has Tile parent
    public bool init_state = true; // if the item is on initialized state
    public bool gravity = true; // if the item can be affected by gravity
    public bool shift = false; // if the item need to go to the new position
    public bool rotate_around = true; // if the item is rotating around
    public Tile currentTile; // the tile which this resource lies on
    public Vector3 shiftPosition;
    
    void Start() {
        GetBasicInformation(); // extract the basic information of the item from itemTemplate

        currentTile = transform.parent.gameObject.GetComponent<Tile>(); // initialize the parent of the item to Tile
        init_state = true; // initialize statement
        DropToGround(currentTile); // drop the item to ground
    }

    void Update() {
        ApplyGravity(); // apply the gravity to the item
        if (!status) {
            if (rotate_around) RotateSelf();
            if (shift) ShiftToPosition();
            else RotateAroundCenter();
        }
    }

    /// <summary>
    /// Apply gravity on the item, not falling through the tile.
    /// </summary>
    private void ApplyGravity() {
        if (gravity) {
            this.transform.position += Physics.gravity * Time.deltaTime;
            //Debug.Log(currentTile.gameObject.GetComponent<Collider>().bounds.extents.y);
            //Debug.Log(currentTile.transform.position.y);
            if (this.transform.position.y < currentTile.transform.position.y                                 // center of the tile 
                                          + currentTile.gameObject.GetComponent<Collider>().bounds.extents.y // halfHeight of the tile
                                          + this.gameObject.GetComponent<Collider>().bounds.extents.y) {     // halfHeight of the item
                this.transform.position = currentTile.transform.position                                                // center of the tile
                                        + Vector3.up * currentTile.gameObject.GetComponent<Collider>().bounds.extents.y // halfHeight of the tile
                                        + Vector3.up * this.gameObject.GetComponent<Collider>().bounds.extents.y;       // halfHeight of the item
                gravity = false; // no longer requires gravity effect
            }
        }
    }

    /// <summary>
    /// Get the basic data of the item.
    /// </summary>
    private void GetBasicInformation() {
        Data.Item item = Data.ItemTemplate[itemCategory][itemType]; // Find the data of the item from array itemTemplate (stored in class Init).
        itemName = item.name;
        itemMass = item.mass;
        itemRotateSpeed = item.rotateSpeed;
    }

    /// <summary>
    /// Rotate itself with a speed of itemRotateSpeed.
    /// </summary>
    private void RotateSelf() {
        this.transform.RotateAround(this.transform.position, Vector3.up, 1.0f * itemRotateSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Drop the item onto Tile t.
    /// </summary>
    /// <param name="t"></param>
    /// <param name="droppedFromPlayer"> If the tile is dropped from player. </param>
    public void DropToGround(Tile t) {
        currentTile = t; // set currentTile to t
        status = false;
        has_tile_parent = true;

        this.gameObject.name = itemName + (++t.itemList[itemCategory, itemType]).ToString(); // add the item to tile's itemList and change name
        this.transform.parent = t.gameObject.transform;

        gravity = true;
        this.gameObject.GetComponent<MeshRenderer>().enabled = true; // show the item
    }

    /// <summary>
    /// Add this item to PlayerInventory playerInventory.
    /// </summary>
    /// <param name="playerInventory"> The inventory that this item is going to be added to. </param>
    public void AddToPlayerInventory(PlayerInventory playerInventory) {
        status = true; // picked up by player
        has_tile_parent = false; // no longer has a Tile parent
        currentTile.itemList[itemCategory, itemType]--; // delete self from tile list
        currentTile = null; // no currentTile needed
        this.transform.parent = playerInventory.transform; // set the item's parent to the player
        this.gameObject.name = itemName + (++playerInventory.inventory[itemCategory, itemType]).ToString(); // rename the item

        // Item no longer affected by gravity
        gravity = false;

        // Hide the picked item
        this.GetComponent<MeshRenderer>().enabled = false;
    }

    /// <summary>
    /// Drop this item from PlayerInventory playerInventory.
    /// </summary>
    /// <param name="playerInventory"></param>
    public void DropFromPlayerInventory(PlayerInventory playerInventory) {
        playerInventory.inventory[itemCategory, itemType]--; // delete the item from player's inventory
        DropToGround(playerInventory.currentTile); // drop the item on the Tile t
        currentTile.ConsoleItem(); // because of new item adding, a sequence of shifting is needed
    }

    /// <summary>
    /// Start the shifting process of the current item and set the shiftPosition to target.
    /// </summary>
    /// <param name="target">The position that the item is shifting to.</param>
    public void StartShifting(Vector3 target) {
        shift = true; // shifting sequence starts
        shiftPosition = target; // set the target of the shifting
    }

    /// <summary>
    /// Shift the item to the position shiftPosition.
    /// </summary>
    private void ShiftToPosition() {
        Vector3 deltaPosition = shiftPosition - this.transform.position; // get the vector Δx by subtracting the two position vectors
        deltaPosition.Normalize(); // normalize the vector to get the direction of the shifting
        deltaPosition *= shiftSpeed * Time.deltaTime; // multiply by speed and time to get the movement in this frame
        this.transform.position += deltaPosition; // apply the movement this frame
        if (Vector3.Distance(this.transform.position, shiftPosition) <= 0.01f) { // nearly reaches the targetPosition
            this.transform.position = shiftPosition; // set the item's position to shiftPosition, shifting completed
            currentTile.RecvShiftComplete(this.gameObject); // tell the Tile that this item had completed shifting
        } 
    }

    /// <summary>
    /// Rotate the item around the center of the tile under it.
    /// </summary>
    private void RotateAroundCenter() {
        this.transform.RotateAround(currentTile.gameObject.transform.position, Vector3.up, RotateCenterSpeed * Time.deltaTime);
    }
}
