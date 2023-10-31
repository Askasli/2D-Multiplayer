using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

// Temporary solution
public class HealthManager : MonoBehaviourPun, IPunObservable
{
    private IPlayerHealth _playerHealth;
    private PhotonView _photonView;
    private bool allowedTakeDamage;
    private float currentHealth;
    private bool checkSpawn = false;
    private bool zoneBool;
    
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform hpBar;
    [SerializeField] private GameObject deadBody;
    [SerializeField] private CameraControl camSetup;
    
    
    [Inject]
    public void Construct(IPlayerHealth playerHealth)
    {
        _playerHealth = playerHealth;
    }
    
    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }
    
    private void Start()
    {
        if (_photonView.IsMine)
        {
             hpBar.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        hpBar.gameObject.GetComponent<Slider>().value = currentHealth;

        if (_photonView.IsMine)
        {
            currentHealth = _playerHealth.GetCurrentHealth();
            
            if (currentHealth <= 0)
            {
                HandlePlayerDeath();
            }
            
            DamageInZone();
        }
    }
    
    private void DamageInZone()
    {
        if (zoneBool)
        {
            if (photonView.IsMine)
                _playerHealth.UseHealth(Time.deltaTime * 3f);
        }
    }
    
    [PunRPC]
    public void TakeDamage(float dmg, string sender)
    {
        if (!_photonView.IsMine)
            return;
        
        _playerHealth.UseHealth(dmg);
      
        if (camSetup != null)
            camSetup.floatShake = 0.3f;

        if (gameObject != null && _playerHealth.GetCurrentHealth() > 0.1f)
            _photonView.RPC("SpriteFlash", RpcTarget.All);
    }

    [PunRPC]
    private IEnumerator SpriteFlash()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.05f);
        spriteRenderer.color = Color.white;
    }

    private void HandlePlayerDeath()
    {
        _photonView.RPC("Defeat", RpcTarget.All);
       
        if (checkSpawn)
        {
            StartCoroutine(PlayerDefeat());
        }
    }

    [PunRPC]
    private void Defeat()
    {
        if (!checkSpawn)
        {
            if (deadBody != null)
            {
                checkSpawn = true;
                GameObject spawnBody =  PhotonNetwork.Instantiate(deadBody.name, transform.position, transform.rotation);
                SpriteRenderer newSpriteRenderer = spawnBody.GetComponent<SpriteRenderer>();
                newSpriteRenderer.sortingLayerName = spriteRenderer.sortingLayerName;
            }
        }
    }


    private IEnumerator PlayerDefeat()
    {
        yield return new WaitForSeconds(0.02f);
        RoomManager.instance.PlayerDefeat();

    }

    private void OnTriggerStay2D(Collider2D collider2d)
    {
        if (collider2d.gameObject.CompareTag("Zone"))
        {
            zoneBool = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collider2d)
    {
        if (collider2d.gameObject.CompareTag("Zone"))
        {
            zoneBool = true;
        }
    }
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentHealth);
        }
        else
        {
            currentHealth = (float)stream.ReceiveNext();
        }
    }
}
