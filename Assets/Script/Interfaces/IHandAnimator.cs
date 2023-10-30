using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHandAnimator
{
     void RotationHand(Animator anim, float horizontal, float vertical, float dampTime, float timing);
}
