using UnityEngine;
using TMPro;
using Photon.Realtime;
using UnityEngine.UI;
using Photon.Pun;
using ExitGames.Client.Photon;
using System;




public class PlayerListItem : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text text;
    Player player;


    public Player Owner
    {
        get { return player; }
        private set { player = value; }
    }

    public void SetUp(Player _player)
    {
        player = _player;
        text.text = _player.NickName;
     //   text.text = PlayerPrefs.GetString("USERNAME");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
       
        if(player == otherPlayer)
        {
            Destroy(gameObject);
        }

    }

    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }

}
