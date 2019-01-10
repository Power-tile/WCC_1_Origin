using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class MenuScript : Init {
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

    /// <summary> This is for creating an initiative map, sized MapLen, MapWid; DO NOT USE WHEN MAP EXISTS. </summary>
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
            row.transform.position = Vector3.right * (i - 1);
            for (int j = 1; j <= MapWid; j++) {
                //GameObject tile = Instantiate(Resources.Load<GameObject>("shity-0.obj"));
                GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Cube);
                tile.name = "Tile" + j.ToString();
                tile.transform.parent = row.transform;
                tile.tag = "Tile";
                tile.transform.position = row.transform.position + Vector3.forward * (j - 1);
                ///*
                if (!(i == 1 && j == 1 || i == 1 && j == MapWid || i == MapLen && j == 1 || i == MapLen && j == MapWid)) {
                    tile.transform.position += Vector3.up * (float)(
                                                System.Math.Sqrt(MapLen * MapLen / 4 + MapWid * MapWid / 4)
                                                - System.Math.Sqrt((i - MapLen / 2) * (i - MapLen / 2)
                                                                 + (j - MapWid / 2) * (j - MapWid / 2))
                                                                                                     ) / 3;
                    tile.transform.position += Vector3.up * UnityEngine.Random.Range(-0.5f, 0.5f);
                }
                //*/
                tile.AddComponent<Tile>();
                tile.GetComponent<Tile>().x = i;
                tile.GetComponent<Tile>().y = j;

                GameObject fog = Instantiate(FogTemplate);
                fog.name = "Fog";
                fog.transform.parent = tile.transform;
                fog.transform.position = tile.transform.position + Vector3.up * 1;
                DestroyImmediate(fog.GetComponent<MeshCollider>());
                fog.tag = "Fog";
                //fog.AddComponent<BoxCollider>();

                GameObject select = Instantiate(SelectTemplate);
                select.name = "Select";
                select.transform.parent = tile.transform;
                select.transform.position = tile.transform.position + Vector3.up * 0.51f;
                select.AddComponent<BoxCollider>();
                select.tag = "Select";
            }
        }
    }

    [MenuItem("Tools/Import Land Model Origin")]
    public static void ImportLandModelOrigin() {
        GameObject map = new GameObject();
        map.name = "MapWithModel";
        map.transform.position.Set(0, 10, 0);
        for (int i = 1; i <= 35; i++) {
            GameObject tmp = Resources.Load<GameObject>("Landscape/Map (" + i.ToString() + ")");

            GameObject landTemplate = null;
            Material landMaterial = Resources.Load<Material>("Landscape/MapMaterial");
            //Debug.Log(landMaterial);
            for (int j = 0; j < tmp.transform.childCount; j++) {
                object child = tmp.transform.GetChild(j);
                if (child is Transform) {
                    landTemplate = ((Transform)child).gameObject;
                }
            }

            if (landTemplate == null) Debug.Log("Having a whistle landTemplate not working!!!");
            if (landMaterial == null) Debug.Log("Having a whistle landMaterial not working!!!");

            GameObject land = Instantiate(landTemplate);
            land.transform.parent = map.transform;
            land.name = "Map (" + i.ToString() + ")";
            land.transform.localScale = 0.008f * Vector3.one;
            land.transform.position = 10 * Vector3.up + i * Vector3.forward;
            land.GetComponent<MeshRenderer>().material = landMaterial;
        }
    }

    [MenuItem("Tools/Test Mesh Filter Combine")]
    public static void MeshFilterCombine()
    {
        //GameObject father = Instantiate(Resources.Load<GameObject>("TestResourses/smallpart"));
        GameObject father = GameObject.Find("Tmp");
        GameObject tmp = father.transform.Find("Map22").gameObject;
        GameObject total = Instantiate(Resources.Load<GameObject>("Empty"));
        GameObject correct = Instantiate(Resources.Load<GameObject>("Empty"));
        correct.AddComponent<MeshRenderer>();
        correct.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Landscape/MapMaterial");
        correct.AddComponent<MeshFilter>();
        correct.GetComponent<MeshFilter>().sharedMesh = tmp.GetComponent<MeshFilter>().sharedMesh;
        total.AddComponent<MeshRenderer>();
        total.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Landscape/MapMaterial");
        Mesh mesh = new Mesh();
        total.AddComponent<MeshFilter>();
        mesh.vertices = tmp.GetComponent<MeshFilter>().sharedMesh.vertices;
        mesh.normals = tmp.GetComponent<MeshFilter>().sharedMesh.normals;
        //mesh.triangles = tmp.GetComponent<MeshFilter>().sharedMesh.triangles;
        Array.Copy(tmp.GetComponent<MeshFilter>().sharedMesh.triangles, mesh.triangles, tmp.GetComponent<MeshFilter>().sharedMesh.triangles.Length);
        mesh.uv = tmp.GetComponent<MeshFilter>().sharedMesh.uv;
        total.GetComponent<MeshFilter>().sharedMesh = mesh;

        /*
        for(int i=120000;i< correct.GetComponent<MeshFilter>().sharedMesh.triangles.Length;i++)
        {
            if(correct.GetComponent<MeshFilter>().sharedMesh.triangles[i]-total.GetComponent<MeshFilter>().sharedMesh.triangles[i]!=0)
            {
                Debug.Log(i);
                Debug.Log(correct.GetComponent<MeshFilter>().sharedMesh.triangles[i] - total.GetComponent<MeshFilter>().sharedMesh.triangles[i]);
            }
        }
        */
    }
}