using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Init : MonoBehaviour {
    /// <summary>
    /// Transforms a Point-formed tile to a Tile-formed tile.
    /// </summary>
    /// <param name="p"></param>
    public static Tile PointToTile(Data.Point p) {
        return GameObject.Find("Map").transform.Find("Row" + p.x.ToString()).transform.Find("Tile" + p.y.ToString()).gameObject.GetComponent<Tile>();
    }

    /// <summary>
    /// Get the tile directly under the GameObject target.
    /// </summary>
    /// <param name="target"> The required GameObject. </param>
    /// <returns> Returns the tile directly under GameObject target. </returns>
    public static Tile GetTileUnderObject(GameObject target) {
        return PointToTile(new Data.Point((int)System.Math.Truncate(target.transform.position.x) + 1, // row of the tile
                                     (int)System.Math.Truncate(target.transform.position.z) + 1)); // column of the tile
    }

    /// <summary>
    /// Check if a point exist in the map.
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    public static bool ValidPoint(Data.Point p) {
        return p.x > 0 && p.x <= Data.MapLen && p.y > 0 && p.y <= Data.MapWid;
    }

    /// <summary>
    /// This is for generating temporary test landscapes for the map.
    /// </summary>
    public void GiveLandscape() {
        //decide the center of each kind of landscape
        //order:mountain,forest,moor,plain
        int x1 = UnityEngine.Random.Range(1, Data.MapLen / 2);
        int x2 = UnityEngine.Random.Range(1, Data.MapLen / 2);
        int x3 = UnityEngine.Random.Range(Data.MapLen / 2 + 1, Data.MapLen);
        int x4 = UnityEngine.Random.Range(Data.MapLen / 2 + 1, Data.MapLen);
        int y1 = UnityEngine.Random.Range(1, Data.MapWid / 2);
        int y2 = UnityEngine.Random.Range(Data.MapWid / 2 + 1, Data.MapWid);
        int y3 = UnityEngine.Random.Range(1, Data.MapWid / 2);
        int y4 = UnityEngine.Random.Range(Data.MapWid / 2 + 1, Data.MapWid);
        for (int i = 1; i <= Data.MapLen; i++) {
            for (int j = 1; j <= Data.MapWid; j++) {
                float[] d = new float[5];
                d[1] = (float)System.Math.Sqrt((x1 - i) * (x1 - i) + (y1 - j) * (y1 - j));
                d[2] = (float)System.Math.Sqrt((x2 - i) * (x2 - i) + (y2 - j) * (y2 - j));
                d[3] = (float)System.Math.Sqrt((x3 - i) * (x3 - i) + (y3 - j) * (y3 - j));
                d[4] = (float)System.Math.Sqrt((x4 - i) * (x4 - i) + (y4 - j) * (y4 - j));
                int tmp = 0;
                float mind = 100000000000.00f;
                for (int k = 1; k <= 4; k++) {
                    if (mind > d[k]) {
                        tmp = k - 1;
                        mind = d[k];
                    }
                }
                Data.MapType[i, j] = tmp;
            }
        }
        for (int i = 1; i <= Data.MapLen; i++) {
            for (int j = 1; j <= Data.MapWid; j++) {
                string stri = i.ToString();
                string strj = j.ToString();
                GameObject map = GameObject.Find("Map");
                GameObject row = map.transform.Find("Row" + stri).gameObject;
                GameObject tile = row.transform.Find("Tile" + strj).gameObject;
                if (Data.MapType[i, j] == 0) {
                    Material material = Resources.Load<Material>("LandMaterial/Grey");
                    tile.GetComponent<Renderer>().material = material;
                }
                else if (Data.MapType[i, j] == 1) {
                    Material material = Resources.Load<Material>("LandMaterial/Green");
                    tile.GetComponent<Renderer>().material = material;
                }
                else if (Data.MapType[i, j] == 2) {
                    Material material = Resources.Load<Material>("LandMaterial/Brown");
                    tile.GetComponent<Renderer>().material = material;
                }
                else if (Data.MapType[i, j] == 3) {
                    Material material = Resources.Load<Material>("LandMaterial/Yellow");
                    tile.GetComponent<Renderer>().material = material;
                }
                Tile t = tile.GetComponent<Tile>();
                t.type = Data.MapType[i, j];
                t.x = i;
                t.y = j;
            }
        }
    }

    public void GenerateItem() {
        for (int i = 1; i <= Data.MapLen; i++) {
            for (int j = 1; j <= Data.MapWid; j++) {
                float possibility = UnityEngine.Random.Range(0.0f, 1.0f);
                if (possibility <= 0.3f) {
                    int cnt = UnityEngine.Random.Range(2, 6);

                    GameObject OakTemplate = Resources.Load<GameObject>("OakTemplate");
                    for (int k = 1; k <= cnt; k++) {
                        GameObject oak = Instantiate(OakTemplate);
                        oak.GetComponent<Items>().DropToGround(PointToTile(new Data.Point(i, j)));
                    }
                }
                PointToTile(new Data.Point(i, j)).ConsoleItem();
            }
        }
    }

    void Start() {
        Resolution[] res = Screen.resolutions;
        Screen.SetResolution(res[0].width, res[0].height, false);

        GiveLandscape();
        GenerateItem();
    }

    void Update() {

    }
}