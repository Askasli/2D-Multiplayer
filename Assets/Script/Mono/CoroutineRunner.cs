using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CoroutineRunner : MonoBehaviour
{ 
    private static CoroutineRunner instance;
    public static CoroutineRunner Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("CoroutineRunner");
                instance = go.AddComponent<CoroutineRunner>();
            }
            return instance;
        }
    }
}



