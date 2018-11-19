using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MenuScript {
    //This is only for assigning the names of the first tiles; DO NOT REUSE.
    /*
    [MenuItem("Tools/Assign First Row Tile Name")]
    public static void AssignFirstRowTileName() {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");

        int i = 0;
        foreach (GameObject t in tiles) {
            i++;
            t.name = "T" + i.ToString();
        }
    }
    */

    //This is only for assigning the names of rows; NO NEED TO REUSE.
    /*
    [MenuItem("Tools/Assign Row Name")]
    public static void AssignRowName() {
        GameObject[] rows = GameObject.FindGameObjectsWithTag("Row");

        int i = 0;
        foreach (GameObject r in rows) {
            i++;
            r.name = "R" + i.ToString();
        }
    }
    */

    //This is for assigning the first positions of rows and tiles(flat, no y change); DO NOT REUSE.
    [MenuItem("Tools/Assign Tile Position")]
    public static void AssignTilePosition() {
        for (int i=1; i<=126; i++) {
            GameObject row = GameObject.Find("R" + i.ToString());
            Vector3 rStandard = new Vector3(0, 0, i - 1);
            row.transform.position = rStandard;
            for (int j=1; j<=126; j++) {
                GameObject tile = row.transform.FindChild("T" + j.ToString()).gameObject;
                Vector3 tStandard = new Vector3(0, 0, j - 1);
                tile.transform.position = tStandard;
            }
        }
    }
}
