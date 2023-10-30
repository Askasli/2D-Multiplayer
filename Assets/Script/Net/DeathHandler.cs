using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class DeathHandler : MonoBehaviourPun
{
    public Animator animator;
    public static DeathHandler Instance;
    private PhotonView pv;

    private void Start()
    {
        Instance = this;
        pv = GetComponent<PhotonView>();
    }

    public void Die()
    {
        animator.SetBool("Alive", false);
        // Call an RPC to trigger the death animation on all clients
        pv.RPC("UnparentObject", RpcTarget.All);
    }



    [PunRPC]
    private void UnparentObject()
    {
        transform.parent = null;
    }

  

}
