using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : Init {
    public Tile currentTile; // the tile the player is now standing on

    public int[,] inventory = new int[itemMaxCategory, itemMaxType]; // recording the items in the player's inventory
    public int[,] itemListAround = new int[itemMaxCategory, itemMaxType]; // recording the items around currentTile

    public int pickupRangeOfPlayer; // the pickup range of the player

    void Start() {
        InitInventory(); // initialize the inventory of the player
        pickupRangeOfPlayer = pickupRange;
    }

    void Update() {
        currentTile = GetTileUnderObject(this.gameObject); // get the tile under the player
        CheckItem(currentTile); // check the items on the tile
    }

    /// <summary>
    /// Check the item on the Tile t and the tiles around Tile t restrained by pickupRangeOfPlayer.
    /// </summary>
    /// <param name="t"></param>
    public void CheckItem(Tile t) {
        for (int i = 0; i < itemMaxCategory; i++) {
            for (int j = 0; j < itemMaxType; j++) {
                itemListAround[i, j] = 0;
            }
        }

        for (int x = t.x - pickupRangeOfPlayer; x <= t.x + pickupRangeOfPlayer; x++) {
            int delta = pickupRangeOfPlayer - System.Math.Abs(t.x - x);

            for (int y = t.y - delta; y <= t.y + delta; y++) {
                if (ValidPoint(new Point(x, y))) {
                    Tile tile = PointToTile(new Point(x, y));

                    if (tile != null && tile.gameObject.transform.childCount > 2) { // currentTile already obtained && currentTile has items
                        for (int i = 0; i < tile.gameObject.transform.childCount; i++) {
                            if (tile.transform.GetChild(i).gameObject.tag == "Item") { // child i is an item
                                Items item = tile.transform.GetChild(i).gameObject.GetComponent<Items>();
                                itemListAround[item.itemCategory, item.itemType]++;
                            }
                        }
                    }
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
