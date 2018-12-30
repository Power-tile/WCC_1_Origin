using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : Init {
    public Tile currentTile; // the tile the player is now standing on

    public int[,] inventory = new int[itemMaxCategory, itemMaxType]; // recording the items in the player's inventory

    void Start() {
        InitInventory();
    }

    void Update() {
        CheckItem(currentTile);
    }

    /// <summary>
    /// Check the item on the Tile tile.
    /// </summary>
    /// <param name="tile"></param>
    private void CheckItem(Tile tile) {
        //Debug.Log(tile.gameObject.transform.childCount);
        if (tile != null && tile.gameObject.transform.childCount > 2) { // currentTile already obtained && currentTile has items
            //Debug.Log(tile.gameObject.transform.childCount);
            for (int i = 0; i < tile.gameObject.transform.childCount; i++) {
                if (tile.transform.GetChild(i).gameObject.tag == "Item") { // child i is an item
                    // add the item to the player's inventory
                    tile.transform.GetChild(i).gameObject.GetComponent<Items>().AddToPlayerInventory(this);
                }
            }
        }
    }

    /// <summary>
    /// Initialize the inventory of the player.
    /// </summary>
    private void InitInventory() {
        for (int i = 0; i < itemMaxCategory; i++) {
            for (int j = 0; j < itemMaxType; j++) {
                inventory[i, j] = 0;
            }
        }
    }
}
