using UnityEngine;
using Photon.Pun;
using TMPro;

public class ZoneController : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] private TMP_Text coolDownZone;
    [SerializeField] private CircleCollider2D capsulZone;

    [SerializeField] private float initialSize = 10f;
    [SerializeField] private float targetSize = 1f;
    [SerializeField] private float shrinkDuration = 120f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float maxDistanceFromCenter = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private Color startColor = Color.green;
    [SerializeField] private Color endColor = Color.red;

    PhotonView pv;

    private float coolDownTimer;
    private float currentSize;
    private float elapsedTime;
    private Vector2 targetPoint;
    private Vector2 centerPosition;
    private float lastSize = 0.01f;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
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
        UpdateCoolDownZone();
        HandleCoolDown();
        UpdateSizeAndColor();
        MoveTowardsTargetPoint();
    }

    private void UpdateCoolDownZone()
    {
        coolDownZone.text = FormatTime(coolDownTimer);
    }

    private void HandleCoolDown()
    {
        if (coolDownTimer <= 0)
        {
            targetSize = Mathf.Lerp(targetSize, lastSize, Time.deltaTime / 2f);
            if (targetSize < 0.02)
            {
                capsulZone.enabled = false;
            }
        }
        else
        {
            coolDownTimer -= Time.deltaTime;
        }
    }

    private void UpdateSizeAndColor()
    {
        elapsedTime += Time.deltaTime;
        currentSize = Mathf.Lerp(initialSize, targetSize, elapsedTime / shrinkDuration);
        Vector3 newScale = new Vector3(currentSize, currentSize, 1f);
        transform.localScale = newScale;
        float sizeRatio = Mathf.Clamp01((initialSize - currentSize) / (initialSize - targetSize));
        Color zoneColor = Color.Lerp(startColor, endColor, sizeRatio);
        UpdateColliderSize(currentSize);
    }

    private void MoveTowardsTargetPoint()
    {
        if (currentSize > targetSize)
        {
            Vector2 directionToCenter = Vector2.zero - (Vector2)transform.position;
            float targetRotation = Mathf.Atan2(directionToCenter.y, directionToCenter.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, targetRotation), rotationSpeed * Time.deltaTime);
        }

        if (currentSize > targetSize)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPoint, moveSpeed * Time.deltaTime);
        }
    }

    private void UpdateColliderSize(float size)
    {
        float colliderRadius = size / 2f;
    }

    private void RandomizeTargetPoint()
    {
        float randomAngle = Random.Range(0f, 360f);
        float randomDistance = Random.Range(0f, maxDistanceFromCenter);
        Vector2 offset = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad)) * randomDistance;
        targetPoint = centerPosition + offset;
        pv.RPC("UpdateTargetPoint", RpcTarget.All, targetPoint);
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        return string.Format("{0}:{1:00}", minutes, seconds);
    }

    [PunRPC]
    private void UpdateTargetPoint(Vector2 newTargetPoint)
    {
        targetPoint = newTargetPoint;
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