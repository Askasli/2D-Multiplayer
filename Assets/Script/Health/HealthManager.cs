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

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform hpBar;
    [SerializeField] private GameObject deadBody;
    [SerializeField] private CameraControl camSetup;
    
    private const float ZoneDamageMultiplier = 3f;
    private const float FlashDuration = 0.05f;
    
    private float currentHealth;
    private bool allowedTakeDamage;
    private bool checkSpawn = false;
    private bool zoneBool;

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
        UpdateHealthBar();
        
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
    
    private void UpdateHealthBar()
    {
        hpBar.gameObject.GetComponent<Slider>().value = currentHealth;
    }

    private void DamageInZone()
    {
        if (zoneBool && photonView.IsMine)
        {
            _playerHealth.UseHealth(Time.deltaTime * ZoneDamageMultiplier);
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
        yield return new WaitForSeconds(FlashDuration);
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
        UpdateZoneState(collider2d, false);
    }

    private void OnTriggerExit2D(Collider2D collider2d)
    {
        UpdateZoneState(collider2d, true);
    }
    
    private void UpdateZoneState(Collider2D collider2d, bool isExit)
    {
        if (collider2d.gameObject.CompareTag("Zone"))
        {
            zoneBool = isExit;
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
