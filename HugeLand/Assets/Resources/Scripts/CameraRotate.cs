using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraRotate : MonoBehaviour {

    public float differ; //recording the angle difference between current rotate and initial rotate

    public void RotateLeft()
    {
        differ += 10f;
        //CurrentPlayer.transform.rotation = Quaternion.Euler(0.0f, CurrentPlayer.transform.rotation.y - 10.0f, 0.0f);
    }

    public void RotateRight()
    {
        differ -= 10f;
        //CurrentPlayer.transform.rotation = Quaternion.Euler(0.0f, CurrentPlayer.transform.rotation.y + 10.0f, 0.0f);
    }

    public Vector3[] PrevDir = new Vector3[10];
    public Vector3[] Dir = new Vector3[10];

    void Start()
    {
        
    }

    void Update()
    {
        GameObject Player = GameObject.Find("Player1");
        GameObject Camera = Player.transform.Find("Camera").gameObject;
        float CurrentAngle = differ;
        
        Vector3 rotation = transform.localEulerAngles;
        rotation.y = CurrentAngle - System.Math.Sign(Player.transform.rotation.y)
                                   *Player.transform.rotation.y
                                   *Player.transform.rotation.y
                                   *180;
        //It seems that the fucking unity thinks that the constant PI should be square root 2. 
        transform.localEulerAngles = rotation;
        
        Vector3 PlayerPos = Player.transform.position;
        Vector3 CameraPos = Player.transform.Find("Camera").gameObject.transform.position;
        Vector3 vector1 = Vector3.back*6+Vector3.up*4;
        float x = vector1.x;
        float z = vector1.z;
        float sin_a = (float)System.Math.Sin((double)CurrentAngle / 180 * 3.1415926);
        float cos_a = (float)System.Math.Cos((double)CurrentAngle / 180 * 3.1415926);
        Vector3 vector2 = vector1;
        vector2.x = cos_a * x + sin_a * z;
        vector2.z = cos_a * z - sin_a * x;
        CameraPos = PlayerPos + vector2;
        Player.transform.Find("Camera").gameObject.transform.position = CameraPos;
    }
}
