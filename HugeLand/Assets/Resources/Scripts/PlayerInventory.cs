using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : Init {
    public Tile currentTile; // the tile the player is now standing on

    public int[,] inventory = new int[itemMaxCategory, itemMaxType]; // recording the items in the player's inventory

	void Start () {
        InitInventory();
	}
	
	void Update () {
        CheckItem(currentTile);
	}

    private void CheckItem(Tile tile) {
        List<Items> itemList = new List<Items>();
        for (int i = 0; i < tile.transform.childCount; i++) {
            if (tile.transform.GetChild(i).gameObject.tag == "Item") {
                itemList.Add(tile.transform.GetChild(i).gameObject.GetComponent<Items>());
            }
        }

        foreach (Items item in itemList) {
            item.AddToPlayerInventory(this);
        }
    }

    private void InitInventory() {
        for (int i = 0; i < itemMaxCategory; i++) {
            for (int j = 0; j < itemMaxType; j++) {
                inventory[i, j] = 0;
            }
        }
    }
}
