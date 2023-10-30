using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class BotController : MonoBehaviourPunCallbacks
{
    public float speed = 10f;
    PhotonView PV;
    private Rigidbody2D rigi;
    private Vector3 networkPosition;
    private float distance;
    private float lag;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        networkPosition = transform.position;
        rigi = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (!PV.IsMine)
            MoveBot();
    }

    void MoveBot()
    {
        PV.RPC("UpdateBotPosition", RpcTarget.Others, transform.position);
    }



    [PunRPC]
    void UpdateBotPosition(Vector3 position)
    {
        networkPosition = position;
        distance = Vector3.Distance(transform.position, networkPosition);
        lag = distance / PhotonNetwork.SerializationRate;
    } 


  

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // Synchronize the enemy's position and velocity across the network
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(rigi.velocity);
        }
        else
        {
            transform.position = (Vector3)stream.ReceiveNext();
            networkPosition = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
          //  rigi.velocity = (Rigidbody2D)stream.ReceiveNext();
        }
    }

}