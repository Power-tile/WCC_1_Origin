using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraRotate : Init {

    public static float[] differ = new float[7] {0,0,0,0,0,0,0}; //recording the angle difference between current rotate and initial rotate for each player

    public void RotateLeft()
    {
        int n = 0;
        for (int i = 1; i <= 4; i++)
        {
            if (GameObject.Find("Player" + i.ToString()).transform.Find("Camera").gameObject.GetComponent<Camera>().enabled)
            {
                n = i;
            }
        }
        differ[n] += 5f;
    }

    public void RotateRight()
    {
        int n = 0;
        for (int i = 1; i <= 4; i++)
        {
            if (GameObject.Find("Player" + i.ToString()).transform.Find("Camera").gameObject.GetComponent<Camera>().enabled)
            {
                n = i;
            }
        }
        differ[n] -= 5f;
    }

    void Start()
    {
        differ[1] = 0;
        differ[2] = 0;
        differ[3] = 0;
        differ[4] = 0;
    }

    void Update()
    {
        int n = 0;
        for(int i=1;i<=4;i++)
        {
            if(GameObject.Find("Player"+i.ToString()).transform.Find("Camera").gameObject.GetComponent<Camera>().enabled)
            {
                n = i;
                break;
            }
        }

        if (Input.mousePosition.x < Screen.width / 10) differ[n] += (Screen.width / 5 - Input.mousePosition.x) / (Screen.width / 5) * 2f;
        if (Input.mousePosition.x > Screen.width / 10 * 9) differ[n] -= (Input.mousePosition.x - Screen.width /  5* 4) / (Screen.width / 5) * 2f;

        GameObject player = GameObject.Find("Player"+n.ToString());
        GameObject camera = player.transform.Find("Camera").gameObject;
        Debug.Log(differ);
        float CurrentAngle = differ[n];
        
        //rotation change
        float theta = CurrentAngle - (float)player.transform.localEulerAngles.y ;
        camera.transform.localEulerAngles=Vector3.right*30+Vector3.up*theta;
        Quaternion p=camera.transform.rotation;
        
        //position change
        Vector3 PlayerPos = player.transform.position;
        Vector3 CameraPos = player.transform.Find("Camera").gameObject.transform.position;
        Vector3 vector1 = Vector3.back*6+Vector3.up*4;
        float x = vector1.x;
        float z = vector1.z;
        float sin_a = (float)System.Math.Sin((double)CurrentAngle / 180 * 3.1415926);
        float cos_a = (float)System.Math.Cos((double)CurrentAngle / 180 * 3.1415926);
        Vector3 vector2 = vector1;
        vector2.x = cos_a * x + sin_a * z;
        vector2.z = cos_a * z - sin_a * x;
        CameraPos = PlayerPos + vector2;
        player.transform.Find("Camera").gameObject.transform.position = CameraPos;
    }
}