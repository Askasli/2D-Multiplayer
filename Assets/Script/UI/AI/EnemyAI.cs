using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Photon.Pun;

public class EnemyAI : MonoBehaviourPun
{
    private Vector2 previousPosition;
    public Animator anim_AI;
    public FieldOfView fieldOfView;
    public AIDestinationSetter aiFollow;
    private Rigidbody2D rigi;
    private Vector3 networkPosition;
    private float distance;
    private float lag;

    PhotonView PV;
    // Start is called before the first frame update
    void Start()
    {
        fieldOfView = GetComponent<FieldOfView>();
        PV = GetComponent<PhotonView>();
        rigi = GetComponent<Rigidbody2D>();
        networkPosition = transform.position;

    }

    // Update is called once per frame
    void Update()
    {

        if(!PV.IsMine)
        {
            
        }    


        Debug.Log(rigi.velocity.magnitude);

       // CheckMovement();


        if (fieldOfView.timeSinceLastSighting > 0)
        {
            // Debug.Log("Player Inside");
            aiFollow.target = fieldOfView.playerTransform;
        }
        else
        {
            Debug.Log("Back To Spawn position");
            aiFollow.target = fieldOfView.spawnPosition;
            //aiFollow.target = fieldOfView.initialPosition;
        }

    }

    void CheckMovement()
    {


        // Check if the enemy's velocity is non-zero
        if (rigi.velocity.magnitude > 0)
        {
            Debug.Log("Enemy is moving!");
         //   photonView.RPC("OnEnemyMoved", RpcTarget.All);
        }
        else
        {
         //   photonView.RPC("OnEnemyStop", RpcTarget.All);
        }

        previousPosition = transform.position;
    }


  

    [PunRPC]
    void OnEnemyMoved()
    {
        anim_AI.SetBool("IsMoving", true);
    }

    [PunRPC]
    void OnEnemyStop()
    {
        anim_AI.SetBool("IsMoving", false);
    }


    


}


