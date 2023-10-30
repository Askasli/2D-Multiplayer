using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerHealthUI : MonoBehaviourPun, IPunObservable
{
    [SerializeField]private Slider _slider;
    private IPlayerHealth _playerHealth;
    private float currentHealth;
    private PhotonView pv;
    
    [Inject]
    public void Construct(IPlayerHealth playerHealth)
    {
        _playerHealth = playerHealth;
        currentHealth = _playerHealth.GetCurrentHealth();
    }

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    private void Update()
    {
        _slider.value = currentHealth;

        if (pv.IsMine)
        {
            currentHealth = _playerHealth.GetCurrentHealth();
            Debug.Log(currentHealth + "currentHealthUI");
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
            currentHealth = (int)stream.ReceiveNext();
        }
    }
}
