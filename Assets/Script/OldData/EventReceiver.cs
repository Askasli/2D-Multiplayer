using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class EventReceiver : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        
        if (eventCode == 1)
        {
           
            object[] data = (object[])photonEvent.CustomData;
            string killerName = (string)data[0];
            string victimName = (string)data[1];

        }
    }
}