using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : Init {
    public int itemCategory = 0; // Stores the category of the item
    public int itemType = 0; // Stores the type of the item
    public string itemName; // Stores the name of the item type
    public float itemMass; // mass of the item
    public float itemRotateSpeed; // The rotating speed of the resource

    public bool status = false; // the status of the item: false - laying on the ground; true - on player
    public bool has_tile_parent = false; // the status of the item: false - no parent OR has player parent; true - has Tile parent
    public Tile currentTile; // The tile which this resource lies on
    
    void Start() {
        GetBasicInformation();

        currentTile = transform.parent.gameObject.GetComponent<Tile>();
        SetTransform(currentTile);
        IgnoreCollider();
    }

    void Update() {
       if (!status) RotateSelf();
    }

    /// <summary>
    /// Get the basic information of the item.
    /// </summary>
    private void GetBasicInformation() {
        Item item = ItemTemplate[itemCategory][itemType];
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
    /// Set the item on top of Tile t, while adding a Rigidbody component to it.
    /// </summary>
    /// <param name="t"></param>
    private void SetTransform(Tile t) {
        this.gameObject.AddComponent<Rigidbody>();
        transform.localPosition = Vector3.up * 1.0f;
        transform.localRotation.Set(0, 0, 0, 0);
        transform.localScale.Set(0.2f, 0.2f, 0.2f);
    }

    /// <summary>
    /// Ignore the colliders of fog and select.
    /// </summary>
    private void IgnoreCollider() {
        if (currentTile != null) {
            /*
            if (!currentTile.insight) {
                Physics.IgnoreCollision(this.GetComponent<Collider>(), currentTile.gameObject.transform.Find("Fog").GetComponent<Collider>(), true);
            }
            */ // Currently there is no collider on fog
            if (currentTile.selectable || currentTile.selected || currentTile.current) {
                Physics.IgnoreCollision(this.GetComponent<Collider>(), currentTile.gameObject.transform.Find("Select").GetComponent<Collider>(), true);
            }
        }
    }

    /// <summary>
    /// Add this item to PlayerInventory playerInventory.
    /// </summary>
    /// <param name="playerInventory"></param>
    public void AddToPlayerInventory(PlayerInventory playerInventory) {
        status = true; // picked up by player
        has_tile_parent = false; // no longer has a Tile parent
        currentTile = null; // no currentTile needed
        transform.parent = playerInventory.transform; // link the item to the player
        playerInventory.inventory[itemCategory, itemType]++; // record the item in player's inventory

        // Delete the Rigidbody component of the item
        Destroy(this.gameObject.GetComponent<Rigidbody>());

        // Reset the transform of this Item
        this.transform.localPosition.Set(0, 0, 0);
        this.transform.localRotation.Set(0, 0, 0, 0);
        this.transform.localScale.Set(0.0001f, 0.0001f, 0.0001f);
    }

    /// <summary>
    /// Drop this item from PlayerInventory playerInventory.
    /// </summary>
    /// <param name="playerInventory"></param>
    public void DropFromPlayerInventory(PlayerInventory playerInventory) {
        status = false; // on the ground
        playerInventory.inventory[itemCategory, itemType]--; // delete the item from player's inventory

        currentTile = playerInventory.currentTile; // set the currentTile of the player
        transform.parent = currentTile.gameObject.transform; // link the item to currentTile
        SetTransform(currentTile); // set the transform of the item
        IgnoreCollider(); // ignore the collider of the select and fog
        has_tile_parent = true; // has a valid Tile parent
    }
}
