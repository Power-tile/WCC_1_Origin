using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticsMove : SwitchTurn {
    const int INF = 0x7fffffff; // Infinity value
    
    public struct Point { // stores a pair of coordinates of a tile
        public int x, y;
        public Point(int x, int y) { // used for assigning tiles
            this.x = x;
            this.y = y;
        }
    }

    // array of moving; up - down - left - right
    public int[] dirx = new int[4] { 0, 0, -1, 1 };
    public int[] diry = new int[4] { -1, 1, 0, 0 };

    public List<Tile> eyelist = new List<Tile>(); // List of the tiles that the player can see
    public List<Tile> movelist = new List<Tile>(); // List of the tiles that the player can move to

    public Point[,] prev = new Point[MapLen + 10, MapWid + 10];

    private Tile currentTile; // marking the tile that the player is standing
    private Point currentPoint; // storing the tile in a point form

    // tmpMaxEye, tmpMaxMove stores the maxeye and maxmove of the current player. (maxeye and maxmove may differ among players)
    private int tmpMaxEye;
    private int tmpMaxMove;

    private Tile targetTile; // marking the targetted tile
    private Point targetPoint; // stroing the targetted tile in a point form

    void Update() {
        // Get and stores the maxeye and maxmove of current player.
        GameObject currentPlayer = GameObject.Find("Player" + currentPlayerNumber.ToString());
        tmpMaxEye = currentPlayer.GetComponent<PlayerMove>().maxEyeOfPlayer;
        tmpMaxMove = currentPlayer.GetComponent<PlayerMove>().maxMoveOfPlayer;
    }

    // Find the tiles that the players can see
    public void SpfaEye(Tile t) {
        eyelist.Clear();
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        foreach (GameObject tile in tiles) { // Recover Fog
            tile.GetComponent<Tile>().insight = false;
        }

        int[,] dis = new int[MapLen + 1, MapWid + 1];
        bool[,] visited = new bool[MapLen + 10, MapWid + 10];
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
                if (Valid(v)) {
                    if (dis[u.x, u.y] + eyecost[MapType[v.x, v.y]] < dis[v.x, v.y]) {
                        dis[v.x, v.y] = dis[u.x, u.y] + eyecost[MapType[v.x, v.y]];
                        if (!visited[v.x, v.y] && dis[v.x, v.y] <= tmpMaxEye) {
                            Point ppush = new Point(v.x, v.y);
                            q.Enqueue(ppush);
                            visited[v.x, v.y] = true;
                        }
                    }
                }
            }
        }

        for (int i=1; i<=MapLen; i++) {
            for (int j=1; j<=MapWid; j++) {
                GameObject.Find("Row" + i.ToString()).transform.Find("Tile" + j.ToString()).gameObject.GetComponent<Tile>().eyedis = dis[i, j];
                if (dis[i, j] <= tmpMaxEye) {
                    GameObject row = GameObject.Find("Row" + i.ToString());
                    GameObject tile = row.transform.Find("Tile" + j.ToString()).gameObject;
                    eyelist.Add(tile.GetComponent<Tile>());
                    tile.GetComponent<Tile>().insight = true;
                    tile.GetComponent<Tile>().eyedis = dis[i, j];
                }
            }
        }
    }

    // Find the tiles that the players can move to.
    public void SpfaMove(Tile t) {
        movelist.Clear();
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        foreach (GameObject tile in tiles) {
            tile.GetComponent<Tile>().selectable = false;
            tile.GetComponent<Tile>().selected = false;
        }

        int[,] dis = new int[MapLen + 10, MapWid + 10];
        bool[,] visited = new bool[MapLen + 1, MapWid + 1];
        Queue<Point> q = new Queue<Point>();

        for (int i = 1; i <= MapLen; i++) {
            for (int j = 1; j <= MapWid; j++) {
                dis[i, j] = INF;
                visited[i, j] = false;
                prev[i, j] = new Point(0, 0);
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
                if (Valid(v)) {
                    if (dis[u.x, u.y] + movecost[MapType[v.x, v.y]] < dis[v.x, v.y]) {
                        dis[v.x, v.y] = dis[u.x, u.y] + movecost[MapType[v.x, v.y]];
                        prev[v.x, v.y] = new Point(u.x, u.y); // Record path
                        if (!visited[v.x, v.y] && dis[v.x, v.y] <= tmpMaxMove) {
                            Point ppush = new Point(v.x, v.y);
                            q.Enqueue(ppush);
                            visited[v.x, v.y] = true;
                        }
                    }
                }
            }
        }

        for (int i = 1; i <= MapLen; i++) {
            for (int j = 1; j <= MapWid; j++) {
                GameObject.Find("Row" + i.ToString()).transform.Find("Tile" + j.ToString()).gameObject.GetComponent<Tile>().movedis = dis[i, j];
                if (dis[i, j] <= tmpMaxMove) {
                    GameObject row = GameObject.Find("Row" + i.ToString());
                    GameObject tile = row.transform.Find("Tile" + j.ToString()).gameObject;
                    movelist.Add(tile.GetComponent<Tile>());
                    tile.GetComponent<Tile>().selectable = true;
                    tile.GetComponent<Tile>().movedis = dis[i, j];
                }
            }
        }
    }

    // Get the current tile under the player.
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

    // Find the tiles that the players can see and the tiles that the players can move to.
    public void FindPath(int eye, int move) {
        tmpMaxEye = eye;
        tmpMaxMove = move;

        GetCurrentTile();
        currentPoint = new Point(currentTile.x, currentTile.y);

        SpfaEye(currentTile);
        SpfaMove(currentTile);
    }

    // Check if point p exists in map.
    public bool Valid(Point p) {
        return p.x > 0 && p.x <= MapLen && p.y > 0 && p.y <= MapWid;
    }

    public void MoveToTile(PlayerMove p, Tile t) {
        p.moving = true;
        t.selected = true;

        targetPoint = new Point(t.x, t.y);
    }

    public void Move(PlayerMove p) {
        Stack<Point> routine = new Stack<Point>();

        int i = targetPoint.x, j = targetPoint.y;
        routine.Push(new Point(i, j));
        while (i != currentPoint.x && j != currentPoint.y) {
            int tmpi = i, tmpj = j;
            i = prev[tmpi, tmpj].x;
            j = prev[tmpi, tmpj].y;
            routine.Push(new Point(i, j));
        }

        i = currentPoint.x;
        j = currentPoint.y;
        while (routine.Count > 0) {
            int tmpi = routine.Peek().x;
            int tmpj = routine.Peek().y;
            if (i != tmpi) {
                MoveX(i, tmpi, p);
            } else {
                MoveY(j, tmpj, p);
            }
            i = tmpi;
            j = tmpj;
            routine.Pop();
        }

        p.moving = false;
    }

    private void MoveX(int m, int n, PlayerMove p) {
        GameObject player = p.gameObject;

        float d = (m > n) ? 0.02f : -0.02f;
        float tmp = m;
        while (tmp != n) {
            tmp += d;
            player.transform.position += Vector3.forward * d;
        }
    }

    private void MoveY(int m, int n, PlayerMove p) {
        GameObject player = p.gameObject;

        float d = (m > n) ? 0.02f : -0.02f;
        float tmp = m;
        while (tmp != n) {
            tmp += d;
            player.transform.position += Vector3.left * d;
        }
    }
}
