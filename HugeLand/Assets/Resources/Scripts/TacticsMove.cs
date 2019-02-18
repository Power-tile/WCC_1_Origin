using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TacticsMove : SwitchTurn {
    // array of moving; up - down - left - right
    public int[] dirx = new int[4] { 0, 0, -1, 1 };
    public int[] diry = new int[4] { -1, 1, 0, 0 };

    public List<Tile> eyelist = new List<Tile>(); // List of the tiles that the player can see
    public List<Tile> movelist = new List<Tile>(); // List of the tiles that the player can move to

    public Data.Point[,] prev = new Data.Point[Data.MapLen + 10, Data.MapWid + 10]; // storing the previous Tile on the moving routine of Tile (i, j)

    private Tile currentTile; // marking the tile that the player is standing
    private Data.Point currentPoint; // storing the tile in a point form

    private Tile targetTile; // marking the targetted tile
    private Data.Point targetPoint; // stroing the targetted tile in a point form

    private Stack<Tile> path = new Stack<Tile>(); // storing the path to the targetted tile
    private Stack<Data.Point> pathPoint = new Stack<Data.Point>(); // storing the path to the targetted tile in a point form

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
        int[,] dis = new int[Data.MapLen + 1, Data.MapWid + 1]; // the minimum moving distance between Tile t and Tile (i, j)
        bool[,] visited = new bool[Data.MapLen + 10, Data.MapWid + 10]; // if Tile (i, j) is in Queue q
        Queue<Data.Point> q = new Queue<Data.Point>(); // the queue used for Spfa algorithm

        for (int i = 1; i <= Data.MapLen; i++) {
            for (int j = 1; j <= Data.MapWid; j++) {
                dis[i, j] = Data.INF; // initialize distance to INFINITY at first
                visited[i, j] = false; // not in queue
            }
        }
        q.Clear(); // initialize queue

        dis[t.x, t.y] = 0; // t is the starting tile, distance = 0
        q.Enqueue(new Data.Point(t.x, t.y)); // push the tile t into the queue for checking the tiles beside it
        visited[t.x, t.y] = true; // tile t is in the queue

        while (q.Count > 0) { // queue not empty, updating required
            Data.Point u = q.Dequeue(); // get top of the queue
            visited[u.x, u.y] = false; // point u no longer in queue

            for (int i = 0; i < 4; i++) { // going thru the four tiles around point u
                Data.Point v = new Data.Point(u.x + dirx[i], u.y + diry[i]); // calculate the row and column of the point
                if (ValidTileForMoving(u, v)) { // point is valid
                    if (dis[u.x, u.y] + Data.eyecost[Data.MapType[v.x, v.y]] < dis[v.x, v.y]) { // relaxation operation
                        dis[v.x, v.y] = dis[u.x, u.y] + Data.eyecost[Data.MapType[v.x, v.y]]; // refresh minimum distance
                        if (!visited[v.x, v.y] && dis[v.x, v.y] <= p.maxEyeOfPlayer) { // point v not in queue
                            Data.Point ppush = new Data.Point(v.x, v.y); // v in point form
                            q.Enqueue(ppush); // add v into queue
                            visited[v.x, v.y] = true; // record enqueue operation
                        }
                    }
                }
            }
        }

        for (int i = 1; i <= Data.MapLen; i++) {
            for (int j = 1; j <= Data.MapWid; j++) {
                Init.PointToTile(new Data.Point(i, j)).eyedis = dis[i, j];
                if (dis[i, j] <= p.maxEyeOfPlayer) {
                    GameObject tile = Init.PointToTile(new Data.Point(i, j)).gameObject;
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
        int[,] dis = new int[Data.MapLen + 10, Data.MapWid + 10];
        bool[,] visited = new bool[Data.MapLen + 1, Data.MapWid + 1];
        Queue<Data.Point> q = new Queue<Data.Point>();

        for (int i = 1; i <= Data.MapLen; i++) {
            for (int j = 1; j <= Data.MapWid; j++) {
                dis[i, j] = Data.INF;
                visited[i, j] = false;
                prev[i, j] = new Data.Point(0, 0);
            }
        }
        q.Clear();

        dis[t.x, t.y] = 0;
        Data.Point push = new Data.Point(t.x, t.y);
        q.Enqueue(push);
        visited[t.x, t.y] = true;

        while (q.Count > 0) {
            Data.Point u = q.Dequeue();
            visited[u.x, u.y] = false;

            for (int i = 0; i < 4; i++) {
                Data.Point v = new Data.Point(u.x + dirx[i], u.y + diry[i]);
                if (ValidTileForMoving(u, v)) {
                    if (dis[u.x, u.y] + Data.movecost[Data.MapType[v.x, v.y]] < dis[v.x, v.y]) {
                        dis[v.x, v.y] = dis[u.x, u.y] + Data.movecost[Data.MapType[v.x, v.y]];
                        prev[v.x, v.y] = new Data.Point(u.x, u.y); // Record path
                        if (!visited[v.x, v.y] && dis[v.x, v.y] <= p.currentMoveOfPlayer) {
                            Data.Point ppush = new Data.Point(v.x, v.y);
                            q.Enqueue(ppush);
                            visited[v.x, v.y] = true;
                        }
                    }
                }
            }
        }

        for (int i = 1; i <= Data.MapLen; i++) {
            for (int j = 1; j <= Data.MapWid; j++) {
                GameObject.Find("Row" + i.ToString()).transform.Find("Tile" + j.ToString()).gameObject.GetComponent<Tile>().movedis = dis[i, j];
                if (dis[i, j] <= p.currentMoveOfPlayer && Init.PointToTile(new Data.Point(i, j)).insight) {
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
        currentTile = Init.GetTileUnderObject(player); // get the tile under the currentPlayer
        currentTile.current = true; // player is standing on this tile, show green select sign
    }

    /// <summary>
    /// Find the tiles that the players can see and the tiles that the players can move to.
    /// </summary>
    public void FindPath(PlayerMove p) {
        GetCurrentTile(p.gameObject);
        currentPoint = new Data.Point(currentTile.x, currentTile.y);

        SpfaEye(currentTile, p);
        SpfaMove(currentTile, p);

        p.pathChecked = true;
    }

    /// <summary>
    /// Check if point q exists in map and the height difference of tile p and tile q is smaller than 2.
    /// </summary>
    public bool ValidTileForMoving(Data.Point p, Data.Point q) {
        return q.x > 0 && q.x <= Data.MapLen && q.y > 0 && q.y <= Data.MapWid
            && Mathf.Abs(Init.PointToTile(p).gameObject.transform.position.y - Init.PointToTile(q).gameObject.transform.position.y) <= 2;
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
        targetPoint = new Data.Point(t.x, t.y);
        pathPoint.Push(targetPoint);
        path.Push(Init.PointToTile(targetPoint));
        Data.Point now = targetPoint;
        while (now != currentPoint) {
            Data.Point tmp = now;
            now = prev[tmp.x, tmp.y];
            Init.PointToTile(tmp).parent = Init.PointToTile(now);
            pathPoint.Push(now);
            path.Push(Init.PointToTile(now));
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
            p.pathChecked = false;

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
}
