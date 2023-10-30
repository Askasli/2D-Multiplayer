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
    // Update is called once per frame
    void Update()
    {
    //    Destroy(gameObject, timeToDelete);
    }

    [PunRPC]
    public void DestroyObj()
    {
        Destroy(gameObject, timeToDelete);
    }
}
