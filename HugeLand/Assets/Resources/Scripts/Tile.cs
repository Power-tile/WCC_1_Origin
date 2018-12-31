using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : Init {
    public bool walkable; // true - can get on
    public bool current; // mark if the player is standing on this tile
    public int type; // terrain type
    public bool exist; // true - not fallen; false - fallen

    public int x, y; // row number and tile number
    public int eyedis; // dis of eye from standing tile to this tile
    public int movedis; // dis of move from standing tile to this tile

    public bool selectable; // if the tile is selectable
    public bool selected; // if the tile is selected
    public bool insight; // if the tile is in sight

    public Tile parent; // used when moving to targetted tile

    public int[,] itemList = new int[itemMaxCategory, itemMaxType];

    void Start() {
        Initialize();
    }

    void Update() {
        GameObject fog = this.transform.Find("Fog").gameObject; // find gameobject fog
        fog.SetActive(!insight); // if the tile is in sight, deactivate fog; else activate fog

        GameObject select = this.transform.Find("Select").gameObject; // find gameobject select
        if (current) { // player is standing on the tile
            select.SetActive(true);
            select.GetComponent<Renderer>().material = Resources.Load<Material>("SelectMaterial/GreenSelect");
        } else if (selected) { // player selected the tile
            select.SetActive(true);
            select.GetComponent<Renderer>().material = Resources.Load<Material>("SelectMaterial/RedSelect");
        } else if (selectable) { // player can select the tile
            select.SetActive(true);
            select.GetComponent<Renderer>().material = Resources.Load<Material>("SelectMaterial/BlueSelect");
        } else { // do not need select sign
            select.SetActive(false);
        }

        if (this.transform.position.y < 0) {
            exist = false;
            walkable = false;
        }
    }

    public void Initialize() {
        walkable = true;
        current = false;
        exist = true;
        insight = false;

        selectable = false;
        selected = false;
        current = false;

        eyedis = INF;
        movedis = INF;

        for (int i = 0; i < itemMaxCategory; i++) {
            for (int j = 0; j < itemMaxType; j++) {
                itemList[i, j] = 0;
            }
        }
    }

    public void AddToTile(Items item) {
        item.gameObject.name = item.itemName + (++itemList[item.itemCategory, item.itemType]).ToString();
    }

    public void DeleteFromTile(Items item) {
        itemList[item.itemCategory, item.itemType]--;
    }
}
