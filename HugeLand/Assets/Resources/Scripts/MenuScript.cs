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

    /*
    [MenuItem("Tools/Test Mesh Filter Combine")]
    public static void MeshFilterCombine()
    {
        //unable to combine , because they are to big
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
    }
    */

    [MenuItem("Tools/Generate Imported Map")]
    public static void GenerateImportedMap() {
        GameObject ColliderMap = new GameObject();
        ColliderMap.name = "Map";
        ColliderMap.transform.position = Vector3.zero;
        GameObject MeshMap = new GameObject();
        MeshMap.name = "MeshMap";
        MeshMap.transform.position = Vector3.zero;
        GameObject Tmp = Instantiate(Resources.Load<GameObject>("ImportedMap1"));
        Tmp.transform.position = Vector3.zero;
        GameObject FogTemplate = Resources.Load<GameObject>("Fog");
        GameObject SelectTemplate = Resources.Load<GameObject>("Select");

        for (int i = 0; i < 7; i++) {
            GameObject MeshRow = new GameObject();
            MeshRow.name = "MeshRow" + i.ToString();
            MeshRow.transform.position = (i * 25 + 12.5f) * Vector3.right;
            MeshRow.transform.parent = MeshMap.transform;
            for (int j = 0; j < 7; j++) {
                //Debug.Log(i.ToString() + "," + j.ToString());
                // Get one mesh piece
                GameObject Land = Tmp.transform.Find("Map" + (i + 1).ToString() + (j + 1).ToString()).gameObject;

                // Create new copy and set basic values
                GameObject Cover = Instantiate(Land);
                Cover.name = "Part" + i.ToString() + j.ToString();
                Cover.transform.parent = MeshRow.transform;
                Cover.transform.localScale = 0.2f * Vector3.one;
                Cover.transform.position = (i * 25 + 12.5f) * Vector3.right + (j * 25 + 12.5f) * Vector3.forward;
                //+ 10 * Vector3.up;

                // Create mesh collider for getting ground height
                Cover.AddComponent<MeshCollider>();
                for (int ii = 0; ii < 25; ii++)// Split one piece of mesh into 25*25 pieces
                    for (int jj = 0; jj < 25; jj++) {
                        float[,] CntHeight = new float[300, 5];// 25 points' ground height count
                        int len = 1;
                        for (int k = 0; k <= 150; k++) { CntHeight[k, 1] = 0; }// number
                        for (int k = 0; k <= 150; k++) { CntHeight[k, 2] = 0; }// occured times
                        for (float iii = 0.1f; iii < 1; iii += 0.2f)
                            for (float jjj = 0.1f; jjj < 1; jjj += 0.2f) {
                                //float gndheight = 128;// Binary search lowest height
                                float gndheight = 0;
                                bool flagg = false;
                                for (gndheight = 0; (gndheight <= 25) && (!flagg); gndheight += 0.01f) {
                                    //Debug.Log(flagg);
                                    Ray ray = new Ray(new Vector3(i * 25 + ii + iii, gndheight, j * 25 + jj + jjj), new Vector3(0, -1, 0));
                                    RaycastHit hit;
                                    if (Physics.Raycast(ray, out hit)) {
                                        //Debug.Log(hit.point);
                                        GameObject now = hit.collider.gameObject;
                                        if (hit.point.y == 0) Debug.Log(now);
                                        //Debug.Log(now);
                                        if (now == Cover) {
                                            //gndheight -= k;
                                            flagg = true;
                                        } else {
                                            //gndheight += k;
                                            flagg = false;
                                        }
                                    } else {
                                        //gndheight += k;
                                        flagg = false;
                                    }
                                    //Debug.Log(iii.ToString() + "," + jjj.ToString() + ":" + gndheight.ToString());
                                }
                                gndheight -= 0.01f;
                                //Debug.Log(iii.ToString()+","+jjj.ToString()+":"+gndheight);
                                bool flag = false;
                                for (int k = 1; k <= len; k++) {
                                    if (CntHeight[len, 1] == gndheight) {
                                        flag = true;
                                        CntHeight[len, 2]++;
                                        break;
                                    }
                                }
                                if (!flag) {
                                    len++;
                                    CntHeight[len, 1] = gndheight;
                                    CntHeight[len, 2] = 1;
                                }
                            }
                        int cnt = 0;// Count most occured ground height and use it as box collider height
                        float height = 0;
                        for (int k = 0; k <= len; k++)
                            if (cnt <= CntHeight[k, 2]) {
                                cnt = (int)CntHeight[k, 2];
                                height = CntHeight[k, 1];
                            }
                        /*
                        string str = "";
                        for (int k = 0; k <= len; k++)
                            str += CntHeight[k, 1].ToString() + " " + CntHeight[k, 2].ToString() + ";";
                        Debug.Log(str);
                        */
                        GameObject collider = Instantiate(Resources.Load<GameObject>("Empty"));// Create collider
                        collider.name = "Tile" + (j * 25 + jj + 1).ToString();
                        collider.transform.position = (i * 25 + ii + 0.5f) * Vector3.right + (j * 25 + jj + 0.5f) * Vector3.forward + height / 2 * Vector3.up;

                        GameObject tile = collider;
                        tile.tag = "Tile";
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

                        //Debug.Log(i * 25 + ii);
                        //Debug.Log(j * 25 + jj);

                        collider.AddComponent<BoxCollider>();
                        collider.GetComponent<BoxCollider>().size = height * Vector3.up + Vector3.right + Vector3.forward;
                        if (GameObject.Find("Row" + (i * 25 + ii + 1).ToString()) == null) {
                            GameObject Row = new GameObject();
                            Row.name = "Row" + (i * 25 + ii + 1).ToString();
                            Row.transform.position = (i * 25 + ii) * Vector3.right;
                            Row.transform.parent = ColliderMap.transform;
                        }
                        collider.transform.parent = GameObject.Find("Row" + (i * 25 + ii + 1).ToString()).transform;
                    }
                DestroyImmediate(Cover.GetComponent<MeshCollider>());
            }
        }
        DestroyImmediate(Tmp);
    }

    [MenuItem("Tools/Toggle Fog and Select")]
    public static void ToggleFogAndSelect() {
        for (int i = 1; i <= MapLen; i++) {
            for (int j = 1; j <= MapWid; j++) {
                GameObject tile = GameObject.Find("Map").transform.Find("Row" + i.ToString()).transform.Find("Tile" + j.ToString()).gameObject;
                tile.transform.Find("Fog").localPosition += (tile.GetComponent<Collider>().bounds.extents.y + 1.0f) * Vector3.up;
                tile.transform.Find("Select").localPosition += (tile.GetComponent<Collider>().bounds.extents.y) * Vector3.up;
            }
        }
    }
}