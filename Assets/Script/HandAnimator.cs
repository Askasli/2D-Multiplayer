using UnityEngine;


public class HandAnimator : IHandAnimator
{
    public void RotationHand(Animator anim, float horizontal, float vertical, float dampTime, float timing)
    {
        anim.SetFloat("Forward", vertical, dampTime, timing);
        anim.SetFloat("Turn", horizontal, dampTime, timing);
    }
}
