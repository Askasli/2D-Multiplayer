using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;


public class ZoneController : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField]private TMP_Text coolDownZone;
    [SerializeField]private CircleCollider2D capsulZone;

    [SerializeField] private float initialSize = 10f; 
    [SerializeField] private float targetSize = 1f;  
    [SerializeField] private float coolDownTimer;

    [SerializeField] private float shrinkDuration = 120f;  
    [SerializeField] private float moveSpeed = 2f;    
    [SerializeField] private float maxDistanceFromCenter = 5f;   
    [SerializeField] private float rotationSpeed = 10f;

    [SerializeField] private Color startColor = Color.green;  
    [SerializeField] private Color endColor = Color.red;     
    PhotonView pv;
    
 
    private CircleCollider2D circleCollider;
    private float currentSize;
    private float elapsedTime;
    [SerializeField]private Vector2 targetPoint;
    public Vector2 centerPosition;
    private int circleStage;
    [SerializeField] private float lastSize = 0.01f;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        //    spriteRenderer = GetComponent<SpriteRenderer>();
        coolDownTimer = 90;
        currentSize = initialSize;
        elapsedTime = 0f;

        if (!photonView.IsMine)
        {
            return;
        }

     
        UpdateColliderSize(currentSize);
        RandomizeTargetPoint();
 
    }
    

    private void Update()
    {
        coolDownZone.text = coolDownTimer.ToString("00:00");

        if (coolDownTimer > 0)
        {
            coolDownTimer -= Time.deltaTime;
        }

        if(coolDownTimer <= 0)
        {
            targetSize = Mathf.Lerp(targetSize, lastSize, Time.deltaTime / 2f);
        }

        if(targetSize < 0.02)
        {
            capsulZone.enabled = false;
        }

        elapsedTime += Time.deltaTime;
        currentSize = Mathf.Lerp(initialSize, targetSize, elapsedTime / shrinkDuration);
        Vector3 newScale = new Vector3(currentSize, currentSize, 1f);
        transform.localScale = newScale;
        float sizeRatio = Mathf.Clamp01((initialSize - currentSize) / (initialSize - targetSize));

        Color zoneColor = Color.Lerp(startColor, endColor, sizeRatio);
   
        UpdateColliderSize(currentSize);

     
        if (currentSize <= targetSize)
        {
            return;
        }
        
        MoveTowardsTargetPoint();
        Vector2 directionToCenter = Vector2.zero - (Vector2)transform.position;
        float targetRotation = Mathf.Atan2(directionToCenter.y, directionToCenter.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, targetRotation), rotationSpeed * Time.deltaTime);
    }

    private void UpdateColliderSize(float size)
    {
        float colliderRadius = size / 2f;
        //circleCollider.radius = colliderRadius / Mathf.Max(transform.localScale.x, transform.localScale.y);
    }

    private void RandomizeTargetPoint()
    {
        float randomAngle = Random.Range(0f, 360f);
        float randomDistance = Random.Range(0f, maxDistanceFromCenter);

        Vector2 offset = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad)) * randomDistance;
        targetPoint = centerPosition + offset;
        pv.RPC("UpdateTargetPoint", RpcTarget.All, targetPoint);
    }

    [PunRPC]
    private void UpdateTargetPoint(Vector2 newTargetPoint)
    {
        targetPoint = newTargetPoint;
    }

    private void MoveTowardsTargetPoint()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPoint, moveSpeed * Time.deltaTime);
    }
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(targetPoint);
        }
        else if (stream.IsReading)
        {
            targetPoint = (Vector2)stream.ReceiveNext();
        }
    }
}