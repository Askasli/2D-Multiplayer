using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
public class ServerPickList : MonoBehaviourPunCallbacks
{
    private Button connectButton;
    public TMP_Text serverDisplayText;
    private string currentServer;



    private void Start()
    {
        currentServer = PlayerPrefs.GetString("CurrentServer", "auto");

        /*

        if (currentServer == "auto")
        {
            PhotonNetwork.ConnectToBestCloudServer("eu,us,as");
        }
        else
        {
            PhotonNetwork.ConnectToRegion(currentServer);
        }


        */
        serverDisplayText.text = PhotonNetwork.CloudRegion;
        //     connectButton.onClick.AddListener(ConnectToUSServer);
    }


    public void SwitchToEUServer()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.ConnectToRegion("eu");
        currentServer = "eu";
        PlayerPrefs.SetString("CurrentServer", currentServer);
        serverDisplayText.text = PhotonNetwork.CloudRegion;

    }

    public void ConnectToUSServer()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.ConnectToRegion("us");
        currentServer = "us";
        PlayerPrefs.SetString("CurrentServer", currentServer);
        serverDisplayText.text = PhotonNetwork.CloudRegion;
    }

    public void SwitchToASServer()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.ConnectToRegion("asia");
        currentServer = "asia";
        PlayerPrefs.SetString("CurrentServer", currentServer);
        serverDisplayText.text = PhotonNetwork.CloudRegion;
    }

    public void SwitchToAutoServer()
    {
        PhotonNetwork.Disconnect();
 //      PhotonNetwork.ConnectToBestCloudServer("eu,us,as");
        currentServer = "auto";
        PlayerPrefs.SetString("CurrentServer", "auto");
        serverDisplayText.text =  PhotonNetwork.CloudRegion;
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        serverDisplayText.text = PhotonNetwork.CloudRegion;
    }
}
