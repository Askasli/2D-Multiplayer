using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimatorManager
{
    void MoveHorizontal(Animator anim, float horizontal, float dumpTime, float timings);
    void MoveVertical(Animator anim, float vertical, float dumpTime, float timings);
    void DashAnimation(Animator anim, bool dashBool);
    void ShootAnimation(Animator anim, bool shootBool);
    void SwordAttackAnimation(Animator anim, bool attackBool);
    void UltimateShootAnimation(Animator anim, bool ultimateBool);
    void MoveAnimation(Animator anim, bool moveBool);
}
