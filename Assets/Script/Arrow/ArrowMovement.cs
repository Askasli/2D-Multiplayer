using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ArrowMovement : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private float speed;
    private Rigidbody2D rigi;

    private Vector3 networkPosition;
    private float distance;
    private float lag;
    private void Awake()
    {
        rigi = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rigi.MovePosition(transform.position + transform.right * (speed * Time.fixedDeltaTime));
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            networkPosition = (Vector3)stream.ReceiveNext();
            distance = Vector3.Distance(transform.position, networkPosition);
            lag = distance / PhotonNetwork.SerializationRate;
        }
    }
}
