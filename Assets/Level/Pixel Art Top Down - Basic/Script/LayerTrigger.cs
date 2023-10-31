using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    public class LayerTrigger : MonoBehaviour
    {
        public string layer;
        public string sortingLayer;

        private void OnTriggerEnter2D(Collider2D other)
        {
            ChangeLayerAndSortingLayerRecursively(other.gameObject.transform, layer, sortingLayer);
        }

        private void ChangeLayerAndSortingLayerRecursively(Transform obj, string newLayer, string newSortingLayer)
        {
            var spriteRenderer = obj.GetComponent<SpriteRenderer>();
            obj.gameObject.layer = LayerMask.NameToLayer(newLayer);
            
            if (spriteRenderer != null)
            {
                spriteRenderer.sortingLayerName = newSortingLayer;
            }

            foreach (Transform child in obj)
            {
                ChangeLayerAndSortingLayerRecursively(child, newLayer, newSortingLayer);
            }
        }
    }
}
