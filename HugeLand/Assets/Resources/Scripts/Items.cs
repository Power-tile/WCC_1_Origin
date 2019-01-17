using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : Init {
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
        ApplyGravity();
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
            if (this.transform.position.y < currentTile.transform.position.y + currentTile.gameObject.GetComponent<Collider>().bounds.extents.y
                                                                             + this.gameObject.GetComponent<Collider>().bounds.extents.y) {
                this.transform.position = currentTile.transform.position
                                        + Vector3.up * currentTile.gameObject.GetComponent<Collider>().bounds.extents.y
                                        + Vector3.up * this.gameObject.GetComponent<Collider>().bounds.extents.y;
                gravity = false;
            }
        }
    }

    /// <summary>
    /// Get the basic data of the item.
    /// </summary>
    private void GetBasicInformation() {
        Item item = ItemTemplate[itemCategory][itemType]; // Find the data of the item from array itemTemplate.
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

        has_tile_parent = true; // has a valid Tile parent
        status = false; // on the ground

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
        DropToGround(playerInventory.currentTile);
        currentTile.ConsoleItem();
    }

    public void StartShifting(Vector3 target) {
        shift = true;
        shiftPosition = target;
    }

    /// <summary>
    /// Shift the item to the position target.
    /// </summary>
    /// <param name="target"></param>
    private void ShiftToPosition() {
        Vector3 deltaPosition = shiftPosition - this.transform.position;
        deltaPosition.Normalize();
        deltaPosition *= shiftSpeed * Time.deltaTime;
        this.transform.position += deltaPosition;
        if (Vector3.Distance(this.transform.position, shiftPosition) <= 0.005f) {
            currentTile.RecvShiftComplete(this.gameObject);
        } 
    }

    private void RotateAroundCenter() {
        this.transform.RotateAround(currentTile.gameObject.transform.position, Vector3.up, RotateCenterSpeed * Time.deltaTime);
    }
}
