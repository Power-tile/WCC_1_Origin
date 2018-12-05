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

        public static bool operator == (Point p, Point q) {
            return p.x == q.x && p.y == q.y;
        }

        public static bool operator != (Point p, Point q) {
            return !(p.x == q.x && p.y == q.y);
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

    private Stack<Tile> path = new Stack<Tile>(); // storing the path to the targetted tile
    private Stack<Point> pathPoint = new Stack<Point>(); // storing the path to the targetted tile in a point form

    private float halfHeight = 0; // storing the half height of the current player
    private Vector3 velocity = new Vector3(); // storing the velocity of the current player
    private Vector3 heading = new Vector3(); // storing the heading (head direction) of the current player

    void Update() {
        // Get and stores the maxeye and maxmove of current player.
        GameObject currentPlayer = GameObject.Find("Player" + currentPlayerNumber.ToString());
        tmpMaxEye = currentPlayer.GetComponent<PlayerMove>().maxEyeOfPlayer;
        tmpMaxMove = currentPlayer.GetComponent<PlayerMove>().maxMoveOfPlayer;
    }

    // Find the tiles that the players can see
    public void SpfaEye(Tile t) {
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

    // Some initiating sequences when a player selects the targetted tile
    public void MoveToTile(PlayerMove p, Tile t) {
        p.moving = true;
        t.selected = true;
        halfHeight = p.gameObject.GetComponent<Collider>().bounds.extents.y;

        targetPoint = new Point(t.x, t.y);
        pathPoint.Push(targetPoint);
        path.Push(PointToTile(targetPoint));
        Point now = targetPoint;
        while (now != currentPoint) {
            Point tmp = now;
            now = prev[tmp.x, tmp.y];
            pathPoint.Push(now);
            path.Push(PointToTile(now));
        }
    }

    public void Move(PlayerMove p) {
        if (path.Count > 0) {
            Tile t = path.Peek();
            Vector3 target = t.transform.position;

            target.y += halfHeight + t.GetComponent<Collider>().bounds.extents.y; // Calculate the unit position on top of target tiles

            if (Vector3.Distance(p.gameObject.transform.position, target) >= 0.05f) {
                CalculateHeading(target, p);
                SetHorizontalVelocity(p);

                p.gameObject.transform.forward = heading;
                p.gameObject.transform.position += velocity * Time.deltaTime;
            } else { // Reached the tile center
                p.gameObject.transform.position = target;
                path.Pop();
            }
        } else {
            p.moving = false;

            RemoveInSightTiles();
            RemoveSelectableTiles();
        }
    }

    protected void RemoveInSightTiles() {
        foreach (Tile t in eyelist) {
            t.insight = false;
        }
        eyelist.Clear();
    }

    protected void RemoveSelectableTiles() {
        foreach (Tile t in movelist) {
            t.current = false;
            t.selectable = false;
            t.selected = false;
        }
        movelist.Clear();
    }

    public void CalculateHeading(Vector3 target, PlayerMove p) {
        heading = target - p.gameObject.transform.position;
        heading.Normalize();
    }

    public void SetHorizontalVelocity(PlayerMove p) {
        velocity = heading * p.moveSpeed;
    }

    public Tile PointToTile(Point p) {
        return GameObject.Find("Row" + p.x.ToString()).transform.Find("Tile" + p.y.ToString()).gameObject.GetComponent<Tile>();
    }
}
