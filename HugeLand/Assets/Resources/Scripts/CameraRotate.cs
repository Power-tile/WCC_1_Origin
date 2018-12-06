using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour {

    public int differ; //recording the angle difference between current rotate and initial rotate

    public void RotateLeft()
    {
        int n = 0;
        if (GameObject.Find("Player1").transform.Find("Camera").gameObject.GetComponent<Camera>().enabled) { n = 1; }
        if (GameObject.Find("Player2").transform.Find("Camera").gameObject.GetComponent<Camera>().enabled) { n = 2; }
        if (GameObject.Find("Player3").transform.Find("Camera").gameObject.GetComponent<Camera>().enabled) { n = 3; }
        if (GameObject.Find("Player4").transform.Find("Camera").gameObject.GetComponent<Camera>().enabled) { n = 4; }
        GameObject CurrentPlayer = GameObject.Find("Player" + n.ToString());
        GameObject CurrentCamera = GameObject.Find("Player" + n.ToString()).transform.Find("Camera").gameObject;
        CurrentCamera.transform.RotateAround(CurrentPlayer.transform.position,Vector3.up,5);
        differ += 5;
        //CurrentPlayer.transform.rotation = Quaternion.Euler(0.0f, CurrentPlayer.transform.rotation.y - 10.0f, 0.0f);
    }

    public void RotateRight()
    {
        int n = 0;
        if (GameObject.Find("Player1").transform.Find("Camera").gameObject.GetComponent<Camera>().enabled) { n = 1; }
        if (GameObject.Find("Player2").transform.Find("Camera").gameObject.GetComponent<Camera>().enabled) { n = 2; }
        if (GameObject.Find("Player3").transform.Find("Camera").gameObject.GetComponent<Camera>().enabled) { n = 3; }
        if (GameObject.Find("Player4").transform.Find("Camera").gameObject.GetComponent<Camera>().enabled) { n = 4; }
        GameObject CurrentPlayer = GameObject.Find("Player" + n.ToString());
        GameObject CurrentCamera = GameObject.Find("Player" + n.ToString()).transform.Find("Camera").gameObject;
        CurrentCamera.transform.RotateAround(CurrentPlayer.transform.position, Vector3.up,-5);
        differ -= 5;
        //CurrentPlayer.transform.rotation = Quaternion.Euler(0.0f, CurrentPlayer.transform.rotation.y + 10.0f, 0.0f);
    }

    public Vector3[] PrevDir = new Vector3[10];
    public Vector3[] Dir = new Vector3[10];

    void Start()
    {
        //check player facing direction changing
        PrevDir[1] = GameObject.Find("Player1").transform.forward;
        PrevDir[2] = GameObject.Find("Player2").transform.forward;
        PrevDir[3] = GameObject.Find("Player3").transform.forward;
        PrevDir[4] = GameObject.Find("Player4").transform.forward;
    }

    void Update()
    {
        /*
        Debug.Log(GameObject.Find("Player1").transform.forward);
        if(GameObject.Find("Player1").transform.forward.y == 0)
            Dir[1] = GameObject.Find("Player1").transform.forward;
        if (GameObject.Find("Player2").transform.forward.y == 0)
            Dir[2] = GameObject.Find("Player2").transform.forward;
        if (GameObject.Find("Player3").transform.forward.y == 0)
            Dir[3] = GameObject.Find("Player3").transform.forward;
        if (GameObject.Find("Player4").transform.forward.y == 0)
            Dir[4] = GameObject.Find("Player4").transform.forward;
        */
        for (int i=1;i<=4;i++)
        {
            GameObject player = GameObject.Find("Player" + i.ToString());
            GameObject camera = GameObject.Find("Player"+i.ToString()).transform.Find("Camera").gameObject;
            if (PrevDir[i] != Dir[i])
            {
                Dir[i].x = (int)Dir[i].x;
                Dir[i].z = (int)Dir[i].z;
                PrevDir[i].x = (int)PrevDir[i].x;
                PrevDir[i].z = (int)PrevDir[i].z;
                if ((-PrevDir[i].z == Dir[i].x) && (PrevDir[i].x == Dir[i].z))
                {
                    camera.transform.RotateAround(player.transform.position, Vector3.up, 45);
                }
                else if ((-PrevDir[i].x == Dir[i].x) && (-PrevDir[i].z == Dir[i].z))
                {
                    camera.transform.RotateAround(player.transform.position, Vector3.up, 90);
                }
                else if ((PrevDir[i].z == Dir[i].x) && (-PrevDir[i].x == Dir[i].z))
                {
                    camera.transform.RotateAround(player.transform.position, Vector3.up, -  45);
                }
            }
        }
        PrevDir[1] = Dir[1];
        PrevDir[2] = Dir[2];
        PrevDir[3] = Dir[3];
        PrevDir[4] = Dir[4];
    }
}
