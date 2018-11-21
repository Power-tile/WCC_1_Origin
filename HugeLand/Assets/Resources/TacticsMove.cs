using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticsMove : MenuScript {
    const int INF = 0x7fffffff;
    
    struct Point { // stores a pair of coordinates of a tile
        public int x, y;
        public Point(int x, int y) { // used for assigning tiles
            this.x = x;
            this.y = y;
        }
    }

    // up, down, left, right
    public int[] dirx = new int[4] { 0, 0, -1, 1 };
    public int[] diry = new int[4] { -1, 1, 0, 0 };

    public List<Tile> eyelist = new List<Tile>();
    public List<Tile> movelist = new List<Tile>();

    Tile currentTile;

    public void SpfaEye(Tile t) {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        foreach (GameObject tile in tiles) { // Recover Fog
            GameObject fog = tile.transform.Find("Fog").gameObject;
            fog.SetActive(true);
        }

        int[,] dis = new int[MapLen + 1, MapWid + 1];
        bool[,] visited = new bool[MapLen + 1, MapWid + 1];
        Queue<Point> q = new Queue<Point>();

        for (int i = 1; i <= MapLen; i++) {
            for (int j = 1; j <= MapWid; j++) {
                dis[i, j] = INF;
                visited[i, j] = false;
            }
        }
        q.Clear();

        dis[t.x, t.y] = 0;
        Point push = new Point(t.x, t.y);
        q.Enqueue(push);
        visited[t.x, t.y] = true;

        while (q.Count > 0) {
            Point u = q.Dequeue();
            visited[u.x, u.y] = false;

            for (int i=0; i<4; i++) {
                Point v = new Point(u.x + dirx[i], u.y + diry[i]);
                if (dis[u.x, u.y] + eyecost[MapType[v.x, v.y]] < dis[v.x, v.y]) {
                    dis[v.x, v.y] = dis[u.x, u.y] + eyecost[MapType[v.x, v.y]];
                    if (!visited[v.x, v.y] && dis[v.x, v.y] <= maxeye) {
                        Point ppush = new Point(v.x, v.y);
                        q.Enqueue(ppush);
                        visited[v.x, v.y] = true;
                    }
                }
            }
        }

        for (int i=1; i<=MapLen; i++) {
            for (int j=1; j<=MapWid; j++) {
                if (dis[i, j] <= maxeye) {
                    GameObject row = GameObject.Find("Row" + i.ToString());
                    GameObject tile = row.transform.Find("Tile" + j.ToString()).gameObject;
                    eyelist.Add(tile.GetComponent<Tile>());
                    GameObject fog = tile.transform.Find("Fog").gameObject;
                    fog.SetActive(false);
                }
            }
        }
    }

    public void SpfaMove(Tile t) {
        int[,] dis = new int[MapLen + 1, MapWid + 1];
        bool[,] visited = new bool[MapLen + 1, MapWid + 1];
        Queue<Point> q = new Queue<Point>();

        for (int i = 1; i <= MapLen; i++) {
            for (int j = 1; j <= MapWid; j++) {
                dis[i, j] = INF;
                visited[i, j] = false;
            }
        }
        q.Clear();

        dis[t.x, t.y] = 0;
        Point push = new Point(t.x, t.y);
        q.Enqueue(push);
        visited[t.x, t.y] = true;

        while (q.Count > 0) {
            Point u = q.Dequeue();
            visited[u.x, u.y] = false;

            for (int i = 0; i < 4; i++) {
                Point v = new Point(u.x + dirx[i], u.y + diry[i]);
                if (dis[u.x, u.y] + movecost[MapType[v.x, v.y]] < dis[v.x, v.y]) {
                    dis[v.x, v.y] = dis[u.x, u.y] + movecost[MapType[v.x, v.y]];
                    if (!visited[v.x, v.y] && dis[v.x, v.y] <= maxmove) {
                        Point ppush = new Point(v.x, v.y);
                        q.Enqueue(ppush);
                        visited[v.x, v.y] = true;
                    }
                }
            }
        }

        for (int i = 1; i <= MapLen; i++) {
            for (int j = 1; j <= MapWid; j++) {
                if (dis[i, j] <= maxmove) {
                    GameObject row = GameObject.Find("Row" + i.ToString());
                    GameObject tile = row.transform.Find("Tile" + j.ToString()).gameObject;
                    movelist.Add(tile.GetComponent<Tile>());
                    tile.GetComponent<Tile>().selectable = true;
                }
            }
        }
    }

    public void GetCurrentTile() {
        currentTile = GetTargetTile(gameObject);
        currentTile.current = true;
    }

    public Tile GetTargetTile(GameObject target) {
        RaycastHit hit;
        Tile tile = null;


        Physics.Raycast(target.transform.position, -Vector3.up, out hit, 1);
        tile = hit.collider.GetComponentInParent<Tile>();

        return tile;
    }

    public void FindPath() {
        GetCurrentTile();
        SpfaEye(currentTile);
    }
}
