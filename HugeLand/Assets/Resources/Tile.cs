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
    public bool selectable;
    
    void Start() {

    }

    void Update() {
        
    }
}
