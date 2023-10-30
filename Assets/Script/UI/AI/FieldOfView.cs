using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;


public class FieldOfView : MonoBehaviourPun
{
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public Transform playerTransform;
    public Transform spawnPosition;
//    private NavMeshAgent navMeshAgent;

    // Timer variables
    public float timeSinceLastSighting = 0f;
    private float timeToReturnToSpawn = 3f;
    public bool playerInSight = false;
    public Vector3 initialPosition;

    public List<Transform> visibleTargets = new List<Transform>();


    void Start()
    {
        //   navMeshAgent = GetComponent<NavMeshAgent>();
        initialPosition = transform.position;
    }




    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector2 dirToTarget = (target.position - transform.position).normalized;

            if (Vector2.Angle(transform.right, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector2.Distance(transform.position, target.position);

                RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask);

                if (!hit)
                {
                    playerTransform = target;
                    visibleTargets.Add(target);
                    timeSinceLastSighting = 5f;
                    playerInSight = true;

                    Debug.Log(target.name + " is inside the field of view"); //add here trigger to attack
                }
                else
                {
                    Debug.Log(target.name + " is outside the field of view");
                    // Update the timer and player sighting state
                    playerInSight = false;
                    //  playerTransform = null;
                   
                }
            }
        }




        if (timeSinceLastSighting > 0)
        {
            timeSinceLastSighting -= Time.deltaTime;
            Debug.Log("Stop follow");
        }
        
        if(timeSinceLastSighting <= 0)
        {
            Debug.Log("Keep follow");
            playerTransform = null;
        }

        Debug.Log(playerInSight + "playerInSight");

    }

    void Update()
    {
        FindVisibleTargets();
      //  LocalPosition();
    }

    /*
    void LocalPosition()
    {
        if (!photonView.IsMine)
        {
            // Use network position and rotation for non-local clients
            transform.position = Vector3.Lerp(transform.position, networkPosition, Time.deltaTime * interpolationFactor);
            transform.rotation = Quaternion.Lerp(transform.rotation, networkRotation, Time.deltaTime * interpolationFactor);
            return;
        }
    }
    */

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector2 viewAngleA = DirFromAngle(-viewAngle / 2, false);
        Vector2 viewAngleB = DirFromAngle(viewAngle / 2, false);

        Gizmos.DrawLine(transform.position, transform.position + (Vector3)viewAngleA * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)viewAngleB * viewRadius);

        foreach (Transform visibleTarget in visibleTargets)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, visibleTarget.position);
        }
    }

    Vector2 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.z;
        }
        return new Vector2(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad));
    }


    /*

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Send position and rotation data to network
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(Time.time);
        }
        else
        {
            // Receive position and rotation data from network
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
            this.networkTime = (float)stream.ReceiveNext();

            // Calculate lag and set interpolation factor
            float lag = Mathf.Abs((float)(PhotonNetwork.Time - this.networkTime));
            interpolationFactor = lag * 10f;
        }
    }

    */
}