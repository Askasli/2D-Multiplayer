using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using TMPro;
using System.Collections;


public class LobbyNameManager : MonoBehaviourPunCallbacks
{
    private const string LobbyNameKey = "LobbyName";
    public TMP_Text lobbyNameText;


    void Start()
    {
        GenerateAndSyncLobbyName();
    }

    private void GenerateAndSyncLobbyName()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            int randomNumber = Random.Range(1000, 10000);
            string lobbyName = "Lobby" + randomNumber.ToString();
            ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable();
            customRoomProperties[LobbyNameKey] = lobbyName;
            PhotonNetwork.CurrentRoom.SetCustomProperties(customRoomProperties);
        }
    }

    
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey(LobbyNameKey))
        {
            string lobbyName = PhotonNetwork.CurrentRoom.CustomProperties[LobbyNameKey] as string;
            lobbyNameText.text = lobbyName;
        }
    }
   
}