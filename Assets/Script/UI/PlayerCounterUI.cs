using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using System.Collections;

public class PlayerCounterUI : MonoBehaviourPunCallbacks, IPunObservable
{
    private PhotonView PV;
    [SerializeField] private TMP_Text playerCounter;
    [SerializeField] private TMP_Text startDelay;
    [SerializeField] private TMP_Text startDelayText;
    [SerializeField] private float alphaDelayTime;
    [SerializeField] private int requiredPlayers = 1;
    [SerializeField] private float delayStartTime = 10f;
   
     Player[] allPlayers;
    
    private bool launchBool;
    
  
    private int playerCount;
    private bool readyToStart;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
       
    }

    private void Update()
    {
        playerCounter.text = PhotonNetwork.PlayerList.Length.ToString("0/4");
        startDelay.color = new Color(startDelay.color.r, startDelay.color.g, startDelay.color.b, alphaDelayTime);
        startDelayText.color = new Color(startDelayText.color.r, startDelayText.color.g, startDelayText.color.b, alphaDelayTime);
        startDelay.SetText(delayStartTime.ToString("0"));
    }


    private void FixedUpdate()
    {
        if (launchBool)
            PlayerCountUpdate();
    }

    void PlayerCountUpdate()
    {
        if (PhotonNetwork.PlayerList.Length >= requiredPlayers)
        {
            if (alphaDelayTime < 1)
            {
                alphaDelayTime += Time.deltaTime;
            }

            PV.RPC("TimerFullLobby", RpcTarget.All);

        }
        else
        {
            PV.RPC("TimeBack", RpcTarget.All);
        }

    }


    [PunRPC]
    IEnumerator TimerFullLobby()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (delayStartTime > 0)
            {
                delayStartTime -= Time.deltaTime;
            }
            else
            {
                if (!launchBool)
                {
                    Launcher.instance.StartGame();
                    launchBool = true;
                }
            }
        }

        yield return null;
    }

    [PunRPC]
    private void TimeBack()
    {
        if (alphaDelayTime > 0)
        {
            alphaDelayTime += Time.deltaTime;
        }

        delayStartTime = 10;
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(delayStartTime);
        }
        else
        {
            delayStartTime = (float)stream.ReceiveNext();
        }
    }
}
