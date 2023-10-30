using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;


public class LayerManager : ILayerManager
{
    public void ChangeLayer(GameObject gameObject, string layerName)
    {
        gameObject.layer = LayerMask.NameToLayer(layerName);
        ChangeLayerInChildren(gameObject.transform, layerName);
    }

    public void ChangeLayerName(SpriteRenderer[] layer, string layerName)
    {
        if (layer != null)
        {
            for (int i = 0; i < layer.Length; i++)
            {
                layer[i].sortingLayerName = layerName;
            }
        }
    }
    
    private void ChangeLayerInChildren(Transform parent, string layerName)
    {
        foreach (Transform child in parent)
        {
            child.gameObject.layer = LayerMask.NameToLayer(layerName);
            ChangeLayerInChildren(child, layerName);
        }
    }
    
    public void TrailLayerName(GameObject[] gameObjects, string layerName)
    {
        if (gameObjects != null)
        {
            for (int i = 0; i < gameObjects.Length; i++)
            {
                gameObjects[i].GetComponent<TrailRenderer>().sortingLayerName = layerName;
            }
        }
    }
}
