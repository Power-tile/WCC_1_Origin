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
    public bool init_state = true;
    public Tile currentTile; // The tile which this resource lies on
    
    void Start() {
        GetBasicInformation();

        currentTile = transform.parent.gameObject.GetComponent<Tile>();
        init_state = true;
        DropToGround(currentTile);
    }

    void Update() {
       if (!status) RotateSelf();
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

        this.gameObject.AddComponent<Rigidbody>();
        this.gameObject.AddComponent<BoxCollider>();
        this.gameObject.GetComponent<MeshRenderer>().enabled = true; // show the item
        transform.localPosition = Vector3.up * 1.0f;
        transform.localRotation.Set(0, 0, 0, 0);
        transform.localScale.Set(0.2f, 0.2f, 0.2f);

        if (currentTile != null) {
            /*
            if (!currentTile.insight) { // ignore fog collider
                Physics.IgnoreCollision(this.GetComponent<Collider>(), currentTile.gameObject.transform.Find("Fog").GetComponent<Collider>(), true);
            }
            */ // no collider on fog, statement not necessary
            if (currentTile.selectable || currentTile.selected || currentTile.current) { // ignore select collider
                Physics.IgnoreCollision(this.GetComponent<Collider>(), currentTile.gameObject.transform.Find("Select").GetComponent<Collider>(), true);
            }
        }

        // ignore player collider
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players) {
            Physics.IgnoreCollision(this.GetComponent<Collider>(), player.transform.Find("Character").gameObject.GetComponent<MeshCollider>());
        }
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

        // Delete the Rigidbody and BoxCollider component of the item
        Destroy(this.gameObject.GetComponent<Rigidbody>());
        Destroy(this.gameObject.GetComponent<BoxCollider>());

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
    }
}
