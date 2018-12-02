using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init : MonoBehaviour {
    //Marking the type of this terrain
    /*
    type:   1   2   3   4
    landscript:   mountain   forest   moor   plain
    color:   grey   green   brown   yellow
    movement cost:   25   20   40   10
    vision cost:   20   25   10   10
    total movement point:150
    total vision point:75
    */

    public static int MapLen = 20, MapWid = 20; // the size of the whole map: (MapLen, MapWid).
    public static int[,] MapType = new int[MapLen + 10, MapWid + 10]; // type of each block
    public static int[] movecost = new int[4] { 25, 20, 40, 10 }; // moving cost of different types of terrain
    public static int[] eyecost = new int[4] { 20, 25, 10, 10 }; // moving cost of different types of terrain
    public static int maxeye = 150; // max eye cost for player
    public static int maxmove = 75; // max move cost for player
    //public static int timecnt = 0;//time counter, used for periodical operation

    // This is for generating temporary test landscapes for the map.
    public static void GiveLandscape() {
        //decide the center of each kinf of landscape
        //order:mountain,forest,moor,plain
        int x1 = UnityEngine.Random.Range(1, MapLen / 2);
        int x2 = UnityEngine.Random.Range(1, MapLen / 2);
        int x3 = UnityEngine.Random.Range(MapLen / 2 + 1, MapLen);
        int x4 = UnityEngine.Random.Range(MapLen / 2 + 1, MapLen);
        int y1 = UnityEngine.Random.Range(1, MapWid / 2);
        int y2 = UnityEngine.Random.Range(MapWid / 2 + 1, MapWid);
        int y3 = UnityEngine.Random.Range(1, MapWid / 2);
        int y4 = UnityEngine.Random.Range(MapWid / 2 + 1, MapWid);
        for (int i = 1; i <= MapLen; i++) {
            for (int j = 1; j <= MapWid; j++) {
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
                MapType[i, j] = tmp;
            }
        }
        for (int i = 1; i <= MapLen; i++) {
            for (int j = 1; j <= MapWid; j++) {
                string stri = i.ToString();
                string strj = j.ToString();
                GameObject map = GameObject.Find("Map");
                GameObject row = map.transform.Find("Row" + stri).gameObject;
                GameObject tile = row.transform.Find("Tile" + strj).gameObject;
                if (MapType[i, j] == 0) {
                    Material material = Resources.Load<Material>("LandMaterial/Grey");
                    tile.GetComponent<Renderer>().material = material;
                }
                else if (MapType[i, j] == 1) {
                    Material material = Resources.Load<Material>("LandMaterial/Green");
                    tile.GetComponent<Renderer>().material = material;
                }
                else if (MapType[i, j] == 2) {
                    Material material = Resources.Load<Material>("LandMaterial/Brown");
                    tile.GetComponent<Renderer>().material = material;
                }
                else if (MapType[i, j] == 3) {
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

    void Start() {
        GiveLandscape();
    }

    void Update()
    {
        
    }
}