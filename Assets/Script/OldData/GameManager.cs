using System;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using TMPro;
using Zenject;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text playersText;
    [SerializeField] private int currentPlayersInGame;
    
    [Inject] 
    private PlayerSpawner playerSpawner;
    

    private void Awake()
    {
        playerSpawner.SpawnPlayer();

        if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene("Menu");
            return;
        }
        
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        
        if (PhotonNetwork.IsMasterClient)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }

    void Update()
    {
        currentPlayersInGame = PhotonNetwork.PlayerList.Length;
        playersText.SetText(currentPlayersInGame.ToString("0"));

        if (photonView.IsMine)
        {
            if (currentPlayersInGame <= 1)
            {
                Debug.Log("You won");
                // wonMenu.SetActive(true);
                //  playersText.gameObject.SetActive(false);
            }
        }
    }


    public void Disconnect()
    {
        PhotonNetwork.LeaveRoom();
        Cursor.visible = true;
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        if (otherPlayer.IsLocal)
        {
            Disconnect();
        }

        base.OnPlayerLeftRoom(otherPlayer);
    }


    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        PlayerPrefs.SetString("Disconnect", "Server close connection");
        base.OnMasterClientSwitched(newMasterClient);

    }



}
