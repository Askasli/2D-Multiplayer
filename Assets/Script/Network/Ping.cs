using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class Ping : MonoBehaviour
{
    private TMP_Text pingText;
    
    void Start()
    {
        pingText = GetComponent<TMP_Text>();
    }

    void LateUpdate()
    {
        int ping = PhotonNetwork.GetPing();
        pingText.text = "Ping: " + ping + "ms";
    }
}
