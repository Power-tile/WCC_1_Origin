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
        GameObject SelectTemplate = Resources.Load<GameObject>("Select");

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

                GameObject select = Instantiate(SelectTemplate);
                select.name = "Select";
                select.transform.parent = tile.transform;
                select.transform.position = tile.transform.position + Vector3.up * 0.51f;
            }
        }
    }

    //Marking the type of this terrain
    /*
    type:   1   2   3   4
    landscript:   mountain   forest   moor   plain
    color:   grey   green   brown   yellow
    movement cost:   25   20   40   10
    vision cost:   20   25   10   10
    total movement point:100
    total vision point:300
    */

    //This is for generating temporary test landscapes for the map.
    public static int[,] MapType = new int[MapLen+1, MapWid+1];
    public static int[] movecost = new int[4] { 25, 20, 40, 10 };
    public static int[] eyecost = new int[4] { 20, 25, 10, 10 };
    public static int maxeye = 300;
    public static int maxmove = 100;
    [MenuItem("Tools/Generate Landscape")]
    public static void GiveLandscape()
    {
        //decide the center of each kinf of landscape
        //order:mountain,forest,moor,plain
        int x1 = UnityEngine.Random.Range(1,MapLen/2);
        int x2 = UnityEngine.Random.Range(1,MapLen/2);
        int x3 = UnityEngine.Random.Range(MapLen/2+1,MapLen);
        int x4 = UnityEngine.Random.Range(MapLen/2+1,MapLen);
        int y1 = UnityEngine.Random.Range(1, MapWid / 2);
        int y2 = UnityEngine.Random.Range(MapWid / 2 + 1, MapWid);
        int y3 = UnityEngine.Random.Range(1, MapWid / 2);
        int y4 = UnityEngine.Random.Range(MapWid / 2 + 1, MapWid);
        for(int i=1;i<=MapLen;i++)
        {
            for(int j=1;j<=MapWid;j++)
            {
                float[] d = new float[5];
                d[1] = (float)System.Math.Sqrt((x1 - i) * (x1 - i) + (y1 - j) * (y1 - j));
                d[2] = (float)System.Math.Sqrt((x2 - i) * (x2 - i) + (y2 - j) * (y2 - j));
                d[3] = (float)System.Math.Sqrt((x3 - i) * (x3 - i) + (y3 - j) * (y3 - j));
                d[4] = (float)System.Math.Sqrt((x4 - i) * (x4 - i) + (y4 - j) * (y4 - j));
                int tmp=0;
                float mind = 100000000000.00f;
                for(int k=1;k<=4;k++)
                {
                    if(mind>d[k])
                    {
                        tmp = k;
                        mind = d[k];
                    }
                }
                MapType[i, j] = tmp;
            }
        }
        for(int i=1;i<=MapLen;i++)
        {
            for(int j=1;j<=MapWid;j++)
            {
                string stri = i.ToString();
                string strj = j.ToString();
                GameObject map = GameObject.Find("Map");
                GameObject row = map.transform.Find("Row" + stri).gameObject;
                GameObject tile = row.transform.Find("Tile" + strj).gameObject;
                if (MapType[i, j] == 1)
                {
                    Material material = Resources.Load<Material>("LandMaterial/Grey");
                    tile.GetComponent<Renderer>().material = material;
                }
                else if(MapType[i, j] == 2)
                {
                    Material material = Resources.Load<Material>("LandMaterial/Green");
                    tile.GetComponent<Renderer>().material = material;
                }
                else if (MapType[i, j] == 3)
                {
                    Material material = Resources.Load<Material>("LandMaterial/Brown");
                    tile.GetComponent<Renderer>().material = material;
                }
                else if (MapType[i, j] == 4)
                {
                    Material material = Resources.Load<Material>("LandMaterial/Yellow");
                    tile.GetComponent<Renderer>().material = material;
                }
                Tile t = tile.GetComponent<Tile>();
                t.type = MapType[i, j];
                t.x = i;
                t.y = j;
            }
        }
    }
}
