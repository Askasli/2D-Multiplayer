using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class FollowMouse : MonoBehaviour
{
    PhotonView pv;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (!pv.IsMine)
        {
            gameObject.SetActive(false);
        }
    }
}
