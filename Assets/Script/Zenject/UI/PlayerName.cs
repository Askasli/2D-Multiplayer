using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerName : MonoBehaviourPun
{
    private TMP_Text playerName;

    private void Awake()
    {
        playerName = GetComponent<TMP_Text>();
    }

    void Start()
    {
        playerName.text = photonView.Owner.NickName;
        
        if(photonView.IsMine)
            playerName.gameObject.transform.localPosition = new Vector3(0.032f, 0.753f, 0f);
    }
}
