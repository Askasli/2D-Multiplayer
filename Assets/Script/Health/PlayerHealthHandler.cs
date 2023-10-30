using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerHealthHandler : MonoBehaviourPun
{
    
    public void CallTakeDamageRPC(int damage)
    {
        photonView.RPC("TakeDamageRPC", RpcTarget.AllBuffered, damage);
    }

 
}
