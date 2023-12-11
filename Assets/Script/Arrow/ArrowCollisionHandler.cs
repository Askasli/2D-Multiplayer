using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ArrowCollisionHandler : MonoBehaviourPun
{ 
    [SerializeField] private GameObject hitrefab;
    [SerializeField] private float damage;
    [SerializeField] private string sender;
    [SerializeField] private bool isUltimate = false;

    private const string EnemyTag = "Enemy";
    private const string SwordTag = "Sword";
    private const string ObstacleTag = "Obstacle";
    
    private void Start()
    {
        if (photonView.IsMine)
        { 
            photonView.RPC("SetArrow", RpcTarget.AllBuffered, photonView.Owner.NickName);
        }
    }

    [PunRPC]
    public void SetArrow(string sender)
    {
        this.sender = sender;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!photonView.IsMine) return;

        if (collision.CompareTag(EnemyTag))
        {
            HandleEnemyCollision(collision);
        }
        else if (collision.CompareTag(SwordTag)) 
        {
            HandleSwordCollision();
        }
        else if (collision.CompareTag(ObstacleTag))
        {
            HandleObstacleCollision();
        }
    }

    private void HandleEnemyCollision(Collider2D collision)
    {
        HealthPlayerHandler enemyHealth = collision.GetComponent<HealthPlayerHandler>();

        if (enemyHealth != null)
        {
            enemyHealth.photonView.RPC("TakeDamage", RpcTarget.All, damage, sender);
        }

        StartCoroutine(HitArrow());
        PhotonNetwork.Destroy(gameObject);
    }

    private void HandleSwordCollision()
    {
        StartCoroutine(HitArrow());
    }

    private void HandleObstacleCollision()
    {
        StartCoroutine(HitArrow());
    }

    private IEnumerator HitArrow()
    {
        GameObject explosionObject = PhotonNetwork.Instantiate(hitrefab.name, transform.position, transform.rotation);
    //   explosionObject.layer = gameObject.layer;

        if (explosionObject != null)
        {
            Destroy(explosionObject, 2);
        }

        Destroy(gameObject);

        yield return null;
    }
}
