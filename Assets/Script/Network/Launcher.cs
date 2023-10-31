using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;


public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher instance;
    private bool lobbyNameGenerated = false;
    
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    
    [SerializeField] Transform PlayeristContent;
    [SerializeField] Transform roomListContent;
    
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] GameObject PlayerListItemPrefab;
    [SerializeField] GameObject StartGameButton;


    private const string LobbyNameKey = "LobbyName";

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Connecting to Master");
    }
    


    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        MenuManager.instance.OpenMenu(MenuName.TitleMenu);
    
        Debug.Log("Joined Lobby");
        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
    }


    /*
    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }

        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuManager.instance.OpenMenu(MenuName.Loading);
    }
    */
    
    
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GenerateAndSyncLobbyName();
        }
        else
        {
            RetrieveLobbyName();
        }

        MenuManager.instance.OpenMenu(MenuName.Room);
        int playerId = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        ExitGames.Client.Photon.Hashtable customProps = new ExitGames.Client.Photon.Hashtable();
        customProps["PlayerId"] = playerId;
        PhotonNetwork.LocalPlayer.SetCustomProperties(customProps);
        
        Player[] players = PhotonNetwork.PlayerList;

        foreach (Transform child in PlayeristContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(PlayerListItemPrefab, PlayeristContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        StartGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }
    
    private void RetrieveLobbyName()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(LobbyNameKey, out object lobbyNameObj))
        {
            string lobbyName = (string)lobbyNameObj;
            roomNameText.text = lobbyName;
        }
    }


    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        StartGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }


    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        MenuManager.instance.OpenMenu(MenuName.ErrorMenu);
        errorText.text = "Room Creation Failed " + message;
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        MenuManager.instance.OpenMenu(MenuName.TitleMenu);
    }


    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.instance.OpenMenu(MenuName.Loading);
    }

    

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }

        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
                continue;
            
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    public void QuickMatch()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.CreateRoom("Lobby " + Random.Range(0, 1000).ToString("0000"), roomOptions, null);
      
    }
    
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
    
        Instantiate(PlayerListItemPrefab, PlayeristContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
        Debug.Log("playerJoinRoom");
    }

    private void GenerateAndSyncLobbyName()
    {
        int randomNumber = Random.Range(1000, 10000);
        string lobbyName = "Lobby " + randomNumber.ToString();

        ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable();
        customRoomProperties[LobbyNameKey] = lobbyName;
        PhotonNetwork.CurrentRoom.SetCustomProperties(customRoomProperties);
    }


    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("playerLeftRoom");

    }
    
    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }

    
    public void Quit()
    {
        Application.Quit();
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey(LobbyNameKey))
        {
            string lobbyName = PhotonNetwork.CurrentRoom.CustomProperties[LobbyNameKey] as string;
            roomNameText.text = lobbyName;
        }
    }
    

}
