using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GroundControl : MonoBehaviour {

    public static int timer;
    public static bool[,] fallen = new bool[20,20];//记录方块状态

	// Use this for initialization
	void Start () {
        timer=0;
        for (int i = 1; i <= 12; i++)
            for (int j = 1; j <= 12; j++)
                GroundControl.fallen[i, j] = false;
    }
	
	// Update is called once per frame
	void Update () {
        timer++;
        int tmptimer = timer / 10;
        if(tmptimer*10==timer)
        {
            //选择需要掉落的方块
            //计算方块到原点的距离，选取值最大方块
            int x = 0, y = 0;
            float maxans = (float)0.00;
            for (int i = 1; i <= 12; i++)
                for (int j = 1; j <= 12; j++)
                    if (fallen[i, j] == false)
                    {
                        float d = (float)System.Math.Sqrt((i - 6.5) * (i - 6.5) + (j - 6.5) * (j - 6.5));
                        //float theta = (float)System.Math.Atan(i / j);
                        //if (theta <= 0) theta += (float)System.Math.PI;
                        float ans = d;
                        if (ans > maxans)
                        {
                            maxans = ans;
                            x = i; y = j;
                        }
                    }
            //方块下落指令
            fallen[x, y] = true;
            string strx = x.ToString();
            string stry = y.ToString();
            GameObject rowx = GameObject.Find("Row" + strx);
            GameObject tmptile = rowx.transform.Find("Tile" + stry).gameObject;
            tmptile.GetComponent<Rigidbody>().constraints = ~RigidbodyConstraints.FreezePositionY;
        }
	}
}
