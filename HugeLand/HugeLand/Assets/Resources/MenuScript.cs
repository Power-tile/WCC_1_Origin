using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class MenuScript : MonoBehaviour{
    //This is only for assigning the names and positions of the first tiles; DO NOT REUSE.
    /*
    [MenuItem("Tools/Assign First Row Tile")]
    public static void AssignFirstRowTileName() {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");

        int i = 0;
        foreach (GameObject t in tiles) {
            i++;
            t.name = "T" + i.ToString();
            Vector3 v = new Vector3(0, 0, i - 1);
            t.transform.position = v;
        }
    }
    */
    

    //This is only for assigning the names and positions of rows; NO NEED TO REUSE.
    /*
    [MenuItem("Tools/Assign Row")]
    public static void AssignRowName() {
        GameObject[] rows = GameObject.FindGameObjectsWithTag("Row");

        int i = 0;
        foreach (GameObject r in rows) {
            i++;
            r.name = "R" + i.ToString();
            Vector3 v = new Vector3(i - 1, 0, 0);
            r.transform.position = v;
        }
    }
    */

    //This is for assigning the Tile.cs file to every tile; DO NOT REUSE.
    /*
    [MenuItem("Tools/Give Tile Script")]
    public static void GiveTileScript() {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");

        foreach (GameObject t in tiles) {
            t.AddComponent<Tile>();
        }
    }
    */

    //This is for assigning the fog GameObject to every tile; DO NOT REUSE.
    /*
    [MenuItem("Tools/Give Tile Fog")]
    public static void GiveTileFog() {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");

        foreach (GameObject t in tiles) {
            GameObject FogTemplate = Resources.Load<GameObject>("Fog");
            GameObject fog = Instantiate(FogTemplate);
            fog.name = "fog";

            fog.transform.parent = t.transform;

            Vector3 position = new Vector3(0.0f, 1.0f, 0.0f);
            fog.transform.position = position;
        }
    }
    */

    //This is for creating an initiative map, sized MapLen, MapWid; DO NOT USE WHEN MAP EXISTS.
    public static int MapLen = 20, MapWid = 20;
    [MenuItem("Tools/Generate Map")]
    public static void MapGenerating() {
        GameObject map = new GameObject();
        map.name = "Map";
        GameObject FogTemplate = Resources.Load<GameObject>("Fog");

        for (int i = 1; i <= MapLen; i++) {
            GameObject row = new GameObject();
            row.name = "Row" + i.ToString();
            row.transform.parent = map.transform;
            row.transform.position = Vector3.left * (i - 1);
            for (int j = 1; j <= MapWid; j++) {
                GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Cube);
                tile.name = "Tile" + j.ToString();
                tile.transform.parent = row.transform;
                tile.tag = "Tile";
                tile.transform.position = row.transform.position + Vector3.forward * (j - 1);
                tile.AddComponent<Tile>();

                GameObject fog = Instantiate(FogTemplate);
                fog.name = "Fog";
                fog.transform.parent = tile.transform;
                fog.transform.position = tile.transform.position + Vector3.up * 1;
            }
        }
    }
}
