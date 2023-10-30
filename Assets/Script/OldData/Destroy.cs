using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Destroy : MonoBehaviour
{
    [SerializeField]private float timeToDelete;

    private void Start()
    {
        DestroyObj();
    }
    
    [PunRPC]
    public void DestroyObj()
    {
        Destroy(gameObject, timeToDelete);
    }
}
