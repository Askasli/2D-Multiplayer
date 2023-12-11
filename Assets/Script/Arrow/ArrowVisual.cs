using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Zenject;
public class ArrowVisual : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private SpriteRenderer[] arrowSpriteRenderers;
    private ICharacterLayerHandler _layer;
    private string layerName; 

    [Inject]
    public void Construct(ICharacterLayerHandler layer)
    {
        _layer = layer;
    }

    private void Start()
    {
      //  StartCoroutine(TimeToDisable());
    }

    private IEnumerator TimeToDisable()
    {
        yield return new WaitForEndOfFrame();
      //  disable = true;
    }


   
 
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            for (int i = 0; i < arrowSpriteRenderers.Length; i++)
            {
               stream.SendNext(arrowSpriteRenderers[i].sortingLayerName);
            }
        }
        else
        {
           
            for (int i = 0; i < arrowSpriteRenderers.Length; i++)
            {
               arrowSpriteRenderers[i].sortingLayerName = (string)stream.ReceiveNext();
            }
        }
    }
   
}
