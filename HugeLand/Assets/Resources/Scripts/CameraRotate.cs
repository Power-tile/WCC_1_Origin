using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraRotate : Init
{

    public static float[] differ_x = new float[7] { 30, 30, 30, 30, 30, 30, 30 };
    public static float[] differ_y = new float[7] { 0, 0, 0, 0, 0, 0, 0 };
    //Vector3 prevpos;
    //Quaternion prevrot;
    //recording the angle difference between current rotate and initial rotate for each player
    //y for rotation around y axis
    /*
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
        differ_y[n] += 5f;
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
        differ_y[n] -= 5f;
    }
    */

    //int currentPlayerNumber = Init.currentPlayerNumber;

    void Start()
    {
        //prevpos = GameObject.Find("Player" + currentPlayerNumber.ToString()).transform.Find("Camera").gameObject.transform.position;
        //prevrot = GameObject.Find("Player" + currentPlayerNumber.ToString()).transform.Find("Camera").gameObject.transform.rotation;
    }

    void Update()
    {
        int n = currentPlayerNumber;

        //int flag_x = 0; // -1: differ_x[n]-=3f; 1: differ_x[n]+=3f
        //int flag_y = 0; // -1: differ_y[n]-=3f; 1: differ_y[n]+=3f
        if((Input.mousePosition.x>0)&&(Input.mousePosition.x<Screen.width)&&(Input.mousePosition.y>0)&&(Input.mousePosition.y<Screen.height))
        {
            if ((Input.mousePosition.x < Screen.width - 160) || (Input.mousePosition.y < Screen.height - 30))
            {
                if (Input.mousePosition.x < Screen.width / 20)
                {
                    differ_y[n] += 3f;
                    //flag_y = 1;
                }
                if (Input.mousePosition.x > Screen.width / 20 * 19)
                {
                    differ_y[n] -= 3f;
                    //flag_y = -1;
                }
                if (Input.mousePosition.y < Screen.height / 20)
                {
                    differ_x[n] -= 3f;
                    //flag_x = -1;
                }
                if (Input.mousePosition.y > Screen.height / 20 * 19)
                {
                    differ_x[n] += 3f;
                    //flag_x = 1;
                }
                if (differ_x[n] >= 90) differ_x[n] = 90;
                if (differ_x[n] <= -90) differ_x[n] = -90;
                //differ_y[n] %= 360;
            }
        }

        GameObject player = GameObject.Find("Player" + n.ToString());
        GameObject camera = player.transform.Find("Camera").gameObject;

        //rotation change
        float theta = differ_y[n] - (float)player.transform.localEulerAngles.y;
        camera.transform.localEulerAngles = Vector3.right * differ_x[n] + Vector3.up * theta;

        //position change
        Vector3 PlayerPos = player.transform.position;
        Vector3 CameraPos = player.transform.Find("Camera").gameObject.transform.position;
        Vector3 vector1 = Vector3.back * 3;
        float y = vector1.y;
        float z = vector1.z;
        float sin = (float)System.Math.Sin((double)differ_x[n] / 180 * 3.1415926);
        float cos = (float)System.Math.Cos((double)differ_x[n] / 180 * 3.1415926);
        Vector3 vector2 = vector1;
        vector2.y = cos * y - sin * z;
        vector2.z = cos * z + sin * y; //vector rotate differ_y[n] around y axis
        float x = vector2.x;
        z = vector2.z;
        sin = (float)System.Math.Sin((double)differ_y[n] / 180 * 3.1415926);
        cos = (float)System.Math.Cos((double)differ_y[n] / 180 * 3.1415926);
        Vector3 vector3 = vector2;
        vector3.x = cos * x + sin * z;
        vector3.z = cos * z - sin * x; //vector rotate differ_x[n] around y axis
        CameraPos = PlayerPos + vector3;
        player.transform.Find("Camera").gameObject.transform.position = CameraPos;

        //distance calculation
        for(int i=1;i<=10000;i++)
        {
            Vector2 centre;
            centre.x = Screen.width / 2;
            centre.y = Screen.height / 2;
            Ray ray = camera.GetComponent<Camera>().ScreenPointToRay(centre);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject now = hit.collider.gameObject;
                if((now.name!="Player1")&&(now.name!= "Player2")&&(now.name!= "Player3")&&(now.name!= "Player4"))
                {
                    //camera.transform.position = prevpos;
                    //camera.transform.rotation = prevrot;
                    //differ_x[n] -= flag_x * 3f;
                    //differ_y[n] -= flag_y * 3f;
                    Vector3 pos = camera.transform.position-player.transform.position;
                    pos.x = pos.x * 0.8f;
                    pos.y = (pos.y - 0.1f) * 0.8f + 0.1f;
                    pos.z = pos.z * 0.8f;
                    camera.transform.position = pos+player.transform.position;
                }
                else
                {
                    break;
                }
            }
        }
        //prevpos = camera.transform.position;
        //prevrot = camera.transform.rotation;
    }
}