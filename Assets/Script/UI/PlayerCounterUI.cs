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
    PhotonView PV;
    [SerializeField] private TMP_Text playerCounter;
    Player[] allPlayers;
    public int playersAmount;
    [SerializeField] private TMP_Text startDelay;
    [SerializeField] private TMP_Text startDelayText;
    [SerializeField] private float alphaDelayTime;
    public int requiredPlayers = 1;

    [SerializeField] private float delayStartTime = 10f;
    [SerializeField] private float notFullTime;
    [SerializeField] private float fullTimer = 10;
    private int once;

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
        if (once != 1)
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
                if (once != 1)
                {
                    Launcher.instance.StartGame();
                    once = 1;
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


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
      //  if (PhotonNetwork.IsMasterClient)
         //   PV.RPC("TimeBack", RpcTarget.All, delayStartTime);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
       // if (PhotonNetwork.IsMasterClient)
          //  PV.RPC("TimeBack", RpcTarget.All, delayStartTime);
    }




    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Send the timer value over the network
            stream.SendNext(delayStartTime);
        }
        else
        {
            // Receive the timer value from the network
            delayStartTime = (float)stream.ReceiveNext();
        }
    }


    [PunRPC]
    private void RPC_SendTimer(float timeIn)
    {
        delayStartTime = timeIn;
        notFullTime = timeIn;

        if (timeIn < fullTimer)
        {
            fullTimer = timeIn;
        }
    }

    void WaitingForOthers()
    {
        if (playerCount <= 1)
        {
            //   res
        }
    }
}
