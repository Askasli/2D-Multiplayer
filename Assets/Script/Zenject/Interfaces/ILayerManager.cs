using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILayerManager
{
    void ChangeLayer(GameObject gameObject, string layerName);

    void ChangeLayerName(SpriteRenderer[] layer, string layerName);

    void TrailLayerName(GameObject[] gameObjects, string layerName);
}
