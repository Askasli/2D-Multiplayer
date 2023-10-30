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
    [SerializeField] TMP_Text playersAmount;
    [SerializeField] int playerAmountInRoom;
    private bool lobbyNameGenerated = false;




    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] TMP_Text playerCounter;

    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;

    [SerializeField] GameObject PlayerListItemPrefab;
    [SerializeField] Transform PlayeristContent;

    [SerializeField] GameObject StartGameButton;


    [SerializeField] private GameObject delayStartButton;
    [SerializeField] private GameObject delayCancelButton;
    [SerializeField] private int roomSize;

    //  [SerializeField] Transform StartGameButtonContent;

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

   
   
    

    
    // Update is called once per frame


    private void AutoStartGame()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount >= 4)
        {
          //  HandleStartGame();

        }
    }

    public override void OnConnectedToMaster()
    {
        //base.OnConnectedToMaster();
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        MenuManager.instance.OpenMenu("Title");
        Debug.Log("Joined Lobby");
        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
    }


    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }

        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuManager.instance.OpenMenu("Loading");

    }

    public override void OnJoinedRoom()
    {
        //  base.OnJoinedRoom();

        if (PhotonNetwork.IsMasterClient)
        {
            GenerateAndSyncLobbyName();
        }
        else
        {
            RetrieveLobbyName();
        }

        MenuManager.instance.OpenMenu("Room");
       // roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        PlayerCounter();

    

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
        // Check if the lobby name exists in custom room properties
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(LobbyNameKey, out object lobbyNameObj))
        {
            // Retrieve the lobby name
            string lobbyName = (string)lobbyNameObj;

            // Update the lobby name text in the UI
            roomNameText.text = lobbyName;
        }
    }


    private string GenerateRandomName()
    {
        string prefix = "Lobby_";
        string randomName = prefix + UnityEngine.Random.Range(1000, 9999);
        return randomName;
    }


    void PlayerCounter()
    {
       // playersAmount.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString("0/4");
        //  playersAmount.text = PhotonNetwork.CountOfPlayersInRooms.ToString("0/4");
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        //base.OnMasterClientSwitched(newMasterClient);
        StartGameButton.SetActive(PhotonNetwork.IsMasterClient);

    }


    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        MenuManager.instance.OpenMenu("Error");
        errorText.text = "Room Creation Failed " + message;
        //  base.OnCreateRoomFailed(returnCode, message);

    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();

        MenuManager.instance.OpenMenu("Loading");
    }

    public override void OnLeftRoom()
    {
        //base.OnLeftRoom();
        MenuManager.instance.OpenMenu("Title");
    }


    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.instance.OpenMenu("Loading");

       

    }

    

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        //base.OnRoomListUpdate(roomList);
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
        //  base.OnPlayerEnteredRoom(newPlayer);
        Instantiate(PlayerListItemPrefab, PlayeristContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
     
        //roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        Debug.Log("playerJoinRoom");

        /*
        if (PhotonNetwork.IsMasterClient)
        {
            GenerateAndSyncLobbyName();
        }
        */
    }

    private void GenerateAndSyncLobbyName()
    {
        // Generate a random number
        int randomNumber = Random.Range(1000, 10000);

        // Assign the random number as the lobby name
        string lobbyName = "Lobby " + randomNumber.ToString();

        // Store the lobby name in custom room properties
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
            // Retrieve the updated lobby name
            string lobbyName = PhotonNetwork.CurrentRoom.CustomProperties[LobbyNameKey] as string;

            // Update the lobby name text in the UI
            roomNameText.text = lobbyName;
        }
    }
    

}
