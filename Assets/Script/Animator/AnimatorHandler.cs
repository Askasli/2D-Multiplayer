using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class AnimatorHandler : IAnimatorHandler
{
    public void MoveHorizontal(Animator anim, float horizontal, float dumpTime, float timings)
    {
        if (anim == null) return;

        anim.SetFloat(AnimatorParameters.Turn, horizontal, dumpTime, timings);
    }

    public void MoveVertical(Animator anim, float vertical, float dumpTime, float timings)
    {
        if (anim == null) return;
        anim.SetFloat(AnimatorParameters.Forward, vertical, dumpTime, timings);
    }

    public void DashAnimation(Animator anim, bool dashBool)
    {
        if (anim == null) return;

        anim.SetBool(AnimatorParameters.Dash, dashBool);
    }

    public void ShootAnimation(Animator anim, bool shootBool)
    {
        if (anim == null) return;

        anim.SetBool(AnimatorParameters.ShootBool, shootBool);
    }

    public void SwordAttackAnimation(Animator anim, bool attackBool)
    {
        if (anim == null) return;

        anim.SetBool(AnimatorParameters.SwordAttack, attackBool);
    }

    public void UltimateShootAnimation(Animator anim, bool ultimateBool)
    {
        if (anim == null) return;

        anim.SetBool(AnimatorParameters.UltimateBool, ultimateBool);
    }

    public void MoveAnimation(Animator anim, bool moveBool)
    {
        if (anim == null) return;

        anim.SetBool(AnimatorParameters.Moving, moveBool);
    }
}
