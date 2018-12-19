using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init : MonoBehaviour {
    // Marking the type of this terrain
    /*
    type:   1   2   3   4
    landscript:   mountain   forest   moor   plain
    color:   grey   green   brown   yellow
    movement cost:   25   20   40   15
    vision cost:   20   25   10   10
    total movement point: 60
    total vision point: 80
    */

    public static int MapLen = 20, MapWid = 20; // the size of the whole map: (MapLen, MapWid).
    public static int[,] MapType = new int[MapLen + 10, MapWid + 10]; // type of each block
    public static int[] movecost = new int[4] { 25, 20, 40, 15 }; // moving cost of different types of terrain
    public static int[] eyecost = new int[4] { 20, 25, 10, 10 }; // moving cost of different types of terrain
    public static int maxeye = 80; // max eye cost for player
    public static int maxmove = 60; // max move cost for player
    // public static int timecnt = 0; // time counter, used for periodical operation

    /// <summary>
    /// Infinite value.
    /// </summary>
    public static readonly int INF = 0x7fffffff - 0x7f;

    /*
    category: Timber 1
    type: 1; name: Oak; source: Oak; mass: 1.5; RotateSpeed: -2.5
    type: 2; name: Willow; source: Willow; mass: 1.2; RotateSpeed: -2.0
    
    category: Metal 2
    type: 1; name: Iron; source: Iron; mass: 3.0; RotateSpeed: 5.0
    type: 2; name: Silver; source: Silver; mass: 3.9; RotateSpeed: 3.0

    category: Stone 3
    type: 1; name: Stone; source: Stone; mass: 2.5; RotateSpeed: 10.0
    */

    public static int currentPlayerNumber = 1;

    public struct Item {
        public int category, type;
        public string name;
        public float mass, rotateSpeed;

        public Item(int category, int type, string name, float mass, float rotateSpeed) {
            this.category = category;
            this.type = type;
            this.name = name;
            this.mass = mass;
            this.rotateSpeed = rotateSpeed;
        }
    }

    public static int MaxCategory = 3;
    public static List<Item>[] ItemTemplate = {
        new List<Item> { new Item(1, 1, "Oak", 1.5f, -2.5f), new Item(1, 2, "Willow", 1.2f, -2.0f) },
        new List<Item> { new Item(2, 1, "Iron", 3.0f, 5.0f), new Item(2, 2, "Silver", 3.9f, 3.0f) },
        new List<Item> { new Item(3, 1, "Stone", 2.5f, 10.0f) }
    };

    /// <summary>
    /// This is for generating temporary test landscapes for the map.
    /// </summary>
    public static void GiveLandscape() {
        //decide the center of each kind of landscape
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

    void Update() {

    }
}