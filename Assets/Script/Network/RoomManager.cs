using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using Zenject;


public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager instance;
  //  [SerializeField] private int playersInRoom;

    private void Start()
    {
        instance = this;
    }
    
    public void DisconnectPlayer()
    {
        StartCoroutine(DisconnectAndLoad());
    }

    public void PlayerDefeat()
    {
        StartCoroutine(DisconnectAndLoadAfterDefeat());
    }

    IEnumerator DisconnectAndLoad()
    {
        PhotonNetwork.Disconnect();

        while (PhotonNetwork.IsConnected)
            yield return null;


        SceneManager.LoadScene(0);
    }

    IEnumerator DisconnectAndLoadAfterDefeat()
    {
        PhotonNetwork.Disconnect();

        while (PhotonNetwork.IsConnected)
            yield return null;

        SceneManager.LoadScene(2);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log(otherPlayer.NickName + "Has left the game");
    //    playersInRoom--;
     //   Debug.Log(playersInRoom + " amount of players in room");

    }
}


