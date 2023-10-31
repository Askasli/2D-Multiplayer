using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;


public class LayerManager : ILayerManager
{
    public const string FirstFloor = "FirstFloor";
    public const string SecondFloor = "SecondFloor";
    public const string ThirdFloor = "ThirdFloor";
    
    
    // Change the layer of a GameObject and its children
    public void ChangeLayer(GameObject gameObject, string layerName)
    {
        gameObject.layer = LayerMask.NameToLayer(layerName);
        ChangeLayerInChildren(gameObject.transform, layerName);
    }

    // Change the sorting layer name of SpriteRenderers 
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
    
    // Change the sorting layer name of TrailRenderers
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

    public string GetLayerNameForCollision(GameObject collisionObject)
    {
        string layerName = null;

        
        if (collisionObject.CompareTag(LayerTags.FirstFloor))
        {
            layerName = "Layer 1";
        }
        else if (collisionObject.CompareTag(LayerTags.SecondFloor))
        {
            layerName =  "Layer 2";
        }
        else if (collisionObject.CompareTag(LayerTags.ThirdFloor))
        {
            layerName =  "Layer 3";
        }
        

        return layerName;
    }
}
