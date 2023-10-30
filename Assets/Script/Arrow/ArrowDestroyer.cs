using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ArrowDestroyer : MonoBehaviourPun
{
    [SerializeField] private float destroyDelay = 4f;

    private void Start()
    {
        if (photonView.IsMine)
        {
            StartCoroutine(DestroyArrowDelayed());
        }
    }

    private IEnumerator DestroyArrowDelayed()
    {
        yield return new WaitForSeconds(destroyDelay);
        PhotonNetwork.Destroy(gameObject);
    }
}
