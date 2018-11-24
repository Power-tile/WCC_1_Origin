using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    public bool walkable = true; // true - can get on
    public bool current = false; // mark if the player is standing on this tile
    public int type; // terrain type
    public bool exist = true; // true - not fallen; false - fallen
    public int x, y; // row number and tile number
    public int eyedis; // dis of eye from standing tile to this tile
    public int movedis; // dis of move from standing tile to this tile
    public bool selectable; // if the tile is selectable
    public bool selected; // if the tile is selected
    public bool insight; // if the tile is in sight
    
    void Start() {

    }

    void Update() {
        GameObject fog = this.transform.Find("Fog").gameObject; // find gameobject fog
        fog.SetActive(!insight); // if the tile is in sight, deactivate fog; else activate fog

        GameObject select = this.transform.Find("Select").gameObject; // find gameobject select
        if (current) {
            select.SetActive(true);
            select.GetComponent<Renderer>().material.color = Color.green;
        }
        else if (selectable) {
            select.SetActive(true);
            select.GetComponent<Renderer>().material.color = Color.blue;
        }
        else if (selected) {
            select.SetActive(true);
            select.GetComponent<Renderer>().material.color = Color.magenta;
        } else {
            select.SetActive(false);
        }
    }
}
