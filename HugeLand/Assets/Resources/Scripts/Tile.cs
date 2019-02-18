using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
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

    public int itemCount = 0;
    public int[,] itemList = new int[Data.itemMaxCategory, Data.itemMaxType]; // recording the list of the items on this Tile
    public GameObject[] itemListInGameObject = new GameObject[500]; // storing the items on this Tile in GameObject form
    public Vector3[] itemPosition = new Vector3[500]; // storing the items' desired init position when shifting
    public bool[] shiftComplete = new bool[500]; // if an item has completed the process of shifting
    public bool onShift = false; // if the items on this Tile is in the shifting process

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

        /// ?? needed to be reconsidered: how to judge if a tile is in the water (in the imported map) ??
        if (this.transform.position.y < 0) { // temporary judge: if the tile is still under the water
            exist = false; // tile no longer exists
            walkable = false; // tile cannot be walked
        }

        if (onShift) CheckShiftComplete();
    }

    /// <summary>
    /// The initialization of the tile.
    /// </summary>
    public void Initialize() {
        walkable = true;
        current = false;
        exist = true;
        insight = false;

        selectable = false;
        selected = false;
        current = false;

        eyedis = Data.INF;
        movedis = Data.INF;

        for (int i = 0; i < Data.itemMaxCategory; i++) {
            for (int j = 0; j < Data.itemMaxType; j++) {
                itemList[i, j] = 0;
            }
        }
    }

    /// <summary>
    /// Control the items on the current Tile.
    /// </summary>
    public void ConsoleItem() {
        itemCount = -1; // resetting the itemCount for a new round of counting
        for (int i = 0; i < this.transform.childCount; i++) {
            GameObject child = this.transform.GetChild(i).gameObject; // getting the i-th child of the Tile
            if (child.tag == "Item") { // this child is an item
                itemListInGameObject[++itemCount] = child; // record the child in GameObject form
                shiftComplete[itemCount] = false; // reset shiftComplete to false
                //child.GetComponent<Items>().DropToGround(this); // shouldn't be called here; DropToGround() should be called before ConsoleItem
            }
        }
        itemCount++; // 0~itemCount-1 storing items; itemCount++ to get exact number of items

        Data.Complex complex = new Data.Complex(0, 0); // storing the complex number for position calculating
        for (int i = 0; i < itemCount; i++) {
            GameObject currentGameObject = itemListInGameObject[i]; // i-th item on this Tile
            if (i == 0) { // the first item
                if (itemCount != 1) complex = new Data.Complex(System.Math.Cos(2 * System.Math.PI / itemCount),
                                                          System.Math.Sin(2 * System.Math.PI / itemCount)); // first position, more than 1 item
                //else complex = new Complex(0, 0); // only one item
            } else { // not first item
                complex *= new Data.Complex(System.Math.Cos(2 * System.Math.PI / itemCount),
                                       System.Math.Sin(2 * System.Math.PI / itemCount)); // shiftPosition for i-th item
            }

            itemPosition[i] = new Vector3(this.transform.position.x + (float)complex.real * 0.3f, // real part -> x, *0.7f considering size of the item
                                          this.transform.position.y + this.gameObject.GetComponent<Collider>().bounds.extents.y
                                                                    + currentGameObject.GetComponent<Collider>().bounds.extents.y,
                                          this.transform.position.z + (float)complex.image * 0.3f); // image part -> z, *0.7f considering size of the item
        }

        for (int i = 0; i < itemCount; i++) {
            GameObject currentGameObject = itemListInGameObject[i]; // get the i-th item on this Tile
            currentGameObject.GetComponent<Items>().StartShifting(itemPosition[i]); // start the shifting process of this item
        }

        onShift = true; // start shifting process on this Tile
    }

    public void CheckShiftComplete() {
        bool complete = true; // true - all item shifting completed; false - !true
        for (int i = 0; i < itemCount; i++) {
            if (!shiftComplete[i]) { // item i shifting sequence incomplete
                complete = false; // still not completed
                break; // no need to check further, break checking sequence
            }
        }
        if (complete) { // all item shifting completed
            onShift = false; // no longer in shift sequence
            for (int i = 0; i < itemCount; i++) {
                itemListInGameObject[i].GetComponent<Items>().shift = false; // close the shifting command of all items, item start center-rotation
            }
        }
    }

    public void RecvShiftComplete(GameObject item) {
        for (int i = 0; i < itemCount; i++) {
            if (itemListInGameObject[i] == item) {
                shiftComplete[i] = true;
                break;
            }
        }
    }
}
