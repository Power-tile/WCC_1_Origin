﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TacticsMove : SwitchTurn {
    public struct Point { // stores a pair of coordinates of a tile
        public int x, y;
        public Point(int x, int y) { // used for assigning tiles
            this.x = x;
            this.y = y;
        }

        public static bool operator ==(Point p, Point q) {
            return p.x == q.x && p.y == q.y;
        }

        public static bool operator !=(Point p, Point q) {
            return !(p.x == q.x && p.y == q.y);
        }

        public override bool Equals(object obj) {
            if (obj is Point) {
                Point p = (Point)obj;
                return x == p.x && y == p.y;
            } else {
                return false;
            }
        }

        public override int GetHashCode() {
            return x.GetHashCode() ^ y.GetHashCode();
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

    private Tile targetTile; // marking the targetted tile
    private Point targetPoint; // stroing the targetted tile in a point form

    private Stack<Tile> path = new Stack<Tile>(); // storing the path to the targetted tile
    private Stack<Point> pathPoint = new Stack<Point>(); // storing the path to the targetted tile in a point form

    private float halfHeight = 0; // storing the half height of the current player
    private Vector3 velocity = new Vector3(); // storing the velocity of the current player
    private Vector3 heading = new Vector3(); // storing the heading (head direction) of the current player

    private bool fallingDown = false; // if the player is falling down
    private bool jumpingUp = false; // if the player is jumping up
    private bool movingEdge = false; // if the player is moving to the edge of the tile
    private Vector3 jumpTarget; // the target of the player's jump

    void Update() {
        // Get and stores the maxeye and maxmove of current player.
    }

    /// <summary>
    /// Find the tiles that the players can see
    /// </summary>
    public void SpfaEye(Tile t, PlayerMove p) {
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

            for (int i = 0; i < 4; i++) {
                Point v = new Point(u.x + dirx[i], u.y + diry[i]);
                if (Valid(u, v)) {
                    if (dis[u.x, u.y] + eyecost[MapType[v.x, v.y]] < dis[v.x, v.y]) {
                        dis[v.x, v.y] = dis[u.x, u.y] + eyecost[MapType[v.x, v.y]];
                        if (!visited[v.x, v.y] && dis[v.x, v.y] <= p.maxEyeOfPlayer) {
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
                GameObject.Find("Row" + i.ToString()).transform.Find("Tile" + j.ToString()).gameObject.GetComponent<Tile>().eyedis = dis[i, j];
                if (dis[i, j] <= p.maxEyeOfPlayer) {
                    GameObject row = GameObject.Find("Row" + i.ToString());
                    GameObject tile = row.transform.Find("Tile" + j.ToString()).gameObject;
                    eyelist.Add(tile.GetComponent<Tile>());
                    tile.GetComponent<Tile>().insight = true;
                    tile.GetComponent<Tile>().eyedis = dis[i, j];
                }
            }
        }
    }

    /// <summary>
    /// Find the tiles that the players can move to.
    /// </summary>
    public void SpfaMove(Tile t, PlayerMove p) {
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
                if (Valid(u, v)) {
                    if (dis[u.x, u.y] + movecost[MapType[v.x, v.y]] < dis[v.x, v.y]) {
                        dis[v.x, v.y] = dis[u.x, u.y] + movecost[MapType[v.x, v.y]];
                        prev[v.x, v.y] = new Point(u.x, u.y); // Record path
                        if (!visited[v.x, v.y] && dis[v.x, v.y] <= p.currentMoveOfPlayer) { /// !!! Change to p.currentMoveOfPlayer after turned
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
                if (dis[i, j] <= p.currentMoveOfPlayer && PointToTile(new Point(i, j)).insight) {
                    GameObject row = GameObject.Find("Row" + i.ToString());
                    GameObject tile = row.transform.Find("Tile" + j.ToString()).gameObject;
                    movelist.Add(tile.GetComponent<Tile>());
                    tile.GetComponent<Tile>().selectable = true;
                    tile.GetComponent<Tile>().movedis = dis[i, j];
                }
            }
        }
    }

    /// <summary>
    /// Get the current tile under the player.
    /// </summary>
    /// <param name="player"> The required player. </param>
    public void GetCurrentTile(GameObject player) {
        currentTile = GetTargetTile(player); // get the tile under the currentPlayer
        currentTile.current = true; // player is standing on this tile, show green select sign
    }

    /// <summary>
    /// Get the tile directly under the GameObject target.
    /// </summary>
    /// <param name="target"> The required GameObject. </param>
    /// <returns> Returns the tile directly under GameObject target. </returns>
    public Tile GetTargetTile(GameObject target) {
        return PointToTile(new Point((int)System.Math.Truncate(target.transform.position.x) + 1, // row of the tile
                                     (int)System.Math.Truncate(target.transform.position.z) + 1)); // column of the tile
    }

    /// <summary>
    /// Find the tiles that the players can see and the tiles that the players can move to.
    /// </summary>
    public void FindPath(PlayerMove p) {
        GetCurrentTile(p.gameObject);
        currentPoint = new Point(currentTile.x, currentTile.y);

        SpfaEye(currentTile, p);
        SpfaMove(currentTile, p);
    }

    /// <summary>
    /// Check if point q exists in map and the height difference of tile p and tile q.
    /// </summary>
    public bool Valid(Point p, Point q) {
        return q.x > 0 && q.x <= MapLen && q.y > 0 && q.y <= MapWid
            && Mathf.Abs(PointToTile(p).gameObject.transform.position.y - PointToTile(q).gameObject.transform.position.y) <= 2;
    }

    /// <summary>
    /// Some initiating sequences when a player selects the targetted tile
    /// </summary>
    /// <param name="p"></param>
    /// <param name="t"></param>
    public void MoveToTile(PlayerMove p, Tile t) { // current player; target
        p.moving = true; // player is moving
        t.selected = true; // mark the selected tile
        halfHeight = p.gameObject.transform.Find("Character").gameObject.GetComponent<Collider>().bounds.extents.y; // obtain halfheight
        p.currentMoveOfPlayer -= t.movedis; // reduce the moving point of the player
        Text text = GameObject.Find("Canvas").transform.Find("RequiredMovePointNumber").gameObject.GetComponent<Text>();
        text.text = "0  ";

        // Finding the path from player's current position to the target position
        targetPoint = new Point(t.x, t.y);
        pathPoint.Push(targetPoint);
        path.Push(PointToTile(targetPoint));
        Point now = targetPoint;
        while (now != currentPoint) {
            Point tmp = now;
            now = prev[tmp.x, tmp.y];
            PointToTile(tmp).parent = PointToTile(now);
            pathPoint.Push(now);
            path.Push(PointToTile(now));
        }
    }

    public void Move(PlayerMove p) {
        if (path.Count > 0) {
            Tile t = path.Peek();
            Vector3 target = t.transform.position;

            target.y += halfHeight + t.gameObject.GetComponent<Collider>().bounds.extents.y; // Calculate the unit position on top of target tiles

            if (Vector3.Distance(p.gameObject.transform.position, target) >= 0.05f) {
                bool jump = p.gameObject.transform.position.y != target.y;

                if (jump) {
                    Jump(target, p);
                }
                else {
                    CalculateHeading(target, p);
                    SetHorizontalVelocity(p);
                }

                // Locomotion
                p.gameObject.transform.forward = heading;
                p.gameObject.transform.position += velocity * Time.deltaTime;
            }
            else { // Reached the tile center
                p.gameObject.transform.position = target;
                p.gameObject.GetComponent<PlayerInventory>().currentTile = t;
                path.Pop();
            }
        }
        else {
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

    private void CalculateHeading(Vector3 target, PlayerMove p) {
        heading = target - p.gameObject.transform.position;
        heading.Normalize();
    }

    private void SetHorizontalVelocity(PlayerMove p) {
        velocity = heading * p.moveSpeed;
    }

    private void Jump(Vector3 target, PlayerMove p) {
        if (fallingDown) {
            FallDownward(target, p);
        }
        else if (jumpingUp) {
            JumpUpward(target, p);
        }
        else if (movingEdge) {
            MoveToEdge(p);
        }
        else {
            PrepareJump(target, p);
        }
    }

    private void PrepareJump(Vector3 target, PlayerMove p) {
        float targetY = target.y;

        target.y = p.gameObject.transform.position.y;

        CalculateHeading(target, p);

        if (p.gameObject.transform.position.y > targetY) {
            fallingDown = false;
            jumpingUp = false;
            movingEdge = true;

            jumpTarget = p.gameObject.transform.position + (target - p.gameObject.transform.position) / 2.0f;
        }
        else {
            fallingDown = false;
            jumpingUp = true;
            movingEdge = false;

            velocity = heading * p.moveSpeed / 3.0f;

            float difference = targetY - p.gameObject.transform.position.y;

            velocity.y = p.jumpVelocity * (0.5f + difference / 2.0f);
        }
    }

    private void FallDownward(Vector3 target, PlayerMove p) {
        velocity += Physics.gravity * Time.deltaTime;

        if (p.gameObject.transform.position.y <= target.y) {
            fallingDown = false;

            Vector3 pos = p.gameObject.transform.position;
            pos.y = target.y;
            p.gameObject.transform.position = pos;

            velocity = new Vector3();
        }
    }

    private void JumpUpward(Vector3 target, PlayerMove p) {
        velocity += Physics.gravity * Time.deltaTime;

        if (p.gameObject.transform.position.y > target.y) {
            jumpingUp = false;
            fallingDown = true;
        }
    }

    private void MoveToEdge(PlayerMove p) {
        if (Vector3.Distance(p.gameObject.transform.position, jumpTarget) >= 0.133f) {
            SetHorizontalVelocity(p);
        }
        else {
            movingEdge = false;
            fallingDown = true;

            velocity /= 2.0f;
            velocity.y = 1.5f;
        }
    }

    /// <summary>
    /// Transforms a Point-formed tile to a Tile-formed tile.
    /// </summary>
    public Tile PointToTile(Point p) {
        return GameObject.Find("Row" + p.x.ToString()).transform.Find("Tile" + p.y.ToString()).gameObject.GetComponent<Tile>();
    }
}
