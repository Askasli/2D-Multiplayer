using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;


public class RpcManager : MonoBehaviourPun
 {
     public static RpcManager Instance { get; private set; }
 
     private void Awake()
     {
         if (Instance != null && Instance != this)
         {
             Destroy(this.gameObject);
             return;
         }
 
         Instance = this;
         DontDestroyOnLoad(this.gameObject);
     }
 
     public void CallRpc(string methodName, int targetViewId, params object[] parameters)
     {
         if (photonView != null)
         {
             photonView.RPC(methodName, PhotonNetwork.GetPhotonView(targetViewId).Owner, parameters);
         }
     }
 }