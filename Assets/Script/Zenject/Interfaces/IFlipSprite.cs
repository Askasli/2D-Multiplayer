
    using Photon.Pun;
    using UnityEngine;

    public interface IFlipSprite
    {
        bool IsFacingRight();
        void Flip(Transform transform);
      
    }
