
    using UnityEngine;
    using Photon.Pun;
    using Zenject;

    public class FlipSprite : IFlipSprite
    {
        private bool isFacingRight = true;
      
        public bool IsFacingRight()
        {
            return isFacingRight;
        }

        public void Flip(Transform transform)
        {
            isFacingRight = !isFacingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
