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
    [SerializeField] private Transform pauseMenu;
    private bool pause;


    PhotonView pv;
    [Inject] private PlayerSpawner playerSpawner;



    private void Awake()
    {
        playerSpawner.SpawnPlayer();

        if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene("Menu");
            return;
        }

        pv = GetComponent<PhotonView>();

    }
   

    // Update is called once per frame
    void Update()
    {
        currentPlayersInGame = PhotonNetwork.PlayerList.Length;

        playersText.SetText(currentPlayersInGame.ToString("0"));

        if (pv.IsMine)
        {
            if (currentPlayersInGame <= 1)
            {
                Debug.Log("You won");
                // wonMenu.SetActive(true);
                //  playersText.gameObject.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause = !pause;
            pauseMenu.gameObject.SetActive(pause);
        }

        Application.targetFrameRate = 60;


        if (PhotonNetwork.IsMasterClient)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }


        /*
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("K", out object kills);
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("D", out object death);

        if (kills != null && death != null)
        {
            scoresHeader.text = "K: " + ((int)kills).ToString("000") + " D:" + ((int)death).ToString("000");
        }
        */
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
