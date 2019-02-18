using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Data {
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
    public static int maxmove = 200; // max move cost for player
    // public static int timecnt = 0; // time counter, used for periodical operation

    /// <summary>
    /// Infinite value.
    /// </summary>
    public static readonly int INF = 0x7fffffff - 0x7f;

    /*
    category: Timber 1
    type: 0; name: Oak; source: Oak; mass: 1.5; RotateSpeed: -25
    type: 1; name: Willow; source: Willow; mass: 1.2; RotateSpeed: -20
    
    category: Metal 2
    type: 0; name: Iron; source: Iron; mass: 3.0; RotateSpeed: 50
    type: 1; name: Silver; source: Silver; mass: 3.9; RotateSpeed: 30

    category: Stone 3
    type: 0; name: Stone; source: Stone; mass: 2.5; RotateSpeed: 100
    */

    public static int currentPlayerNumber = 1;

    public class Item {
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

    public static int itemMaxCategory = 3;
    public static int itemMaxType = 2;
    public static List<Item>[] ItemTemplate = {
        new List<Item> { new Item(0, 0, "Oak", 1.5f, 25f),
                         new Item(0, 1, "Willow", 1.2f, 20f) },
        new List<Item> { new Item(1, 0, "Iron", 3.0f, -50f),
                         new Item(1, 1, "Silver", 3.9f, -30f) },
        new List<Item> { new Item(2, 0, "Stone", 2.5f, -100f) }
    };
    public static int pickupRange = 3;
    
    public class Point { // stores a pair of coordinates of a tile
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

    /// <summary>
    /// A complex number represented as (real, image), indicating the complex number is valued real + image*i.
    /// </summary>
    public class Complex {
        public double real, image;
        public Complex(double real, double image) {
            this.real = real;
            this.image = image;
        }

        public static Complex operator *(Complex p, Complex q) {
            return new Complex(p.real * q.real - p.image * q.image, p.real * q.image + p.image * q.real);
        }
    }
}
