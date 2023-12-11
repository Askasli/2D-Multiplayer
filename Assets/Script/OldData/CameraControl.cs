using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraControl : MonoBehaviour
{
    PhotonView pv;

    public CharacterLogicHandler playerLocal;
    public float speed;
    public float floatShake;
    private Vector3 rotationAmount;
    private float timeOutShake = 1f;
    public float shakePower;

    private void Awake()
    {
        transform.parent = null;
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if(!pv.IsMine)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (pv.IsMine)
        {
            CameraShake();
        }
    }

    void CameraShake()
    {
        if (floatShake > 0)
        {
            floatShake -= Time.deltaTime * timeOutShake;
            rotationAmount = Random.insideUnitSphere * shakePower;
            rotationAmount.z = 0f;
            transform.rotation = Quaternion.Euler(rotationAmount);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 3f);
        }
    }

    public void LateUpdate()
    {
        if (playerLocal == null)
        {
            playerLocal = GetComponentInParent<CharacterLogicHandler>();

            if (playerLocal == null) 
            { 
                Destroy(gameObject); 
                return; 
            }
            
            return;
        }

        if (pv.IsMine)
        {
            transform.position = Vector3.Lerp(transform.position, playerLocal.transform.position + new Vector3(0, 0, -10), Time.deltaTime * speed);
        }
    }
}
