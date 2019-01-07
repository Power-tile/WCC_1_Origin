using UnityEditor;
using UnityEngine;
using SelfDefine;
using System.IO;
using System;

public class Test : MonoBehaviour
{
    [MenuItem("Tools/Test Import Land Model Origin")]
    public static void ImportLandModelOrigin_Test()
    {
        StreamReader sd = new StreamReader("Asset/Resources/shity-0.txt");
        String line = sd.ReadLine();
        while ((line = sd.ReadLine()) != null)
        {
            Debug.Log(line);
        }
    }
}