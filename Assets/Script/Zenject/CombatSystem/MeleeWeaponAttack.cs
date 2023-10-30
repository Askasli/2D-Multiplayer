using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MeleeWeaponAttack : IMeleeWeaponAttack
{
    private IAnimatorManager _animatorManager;
    private ICombatInput _combatInput;
    private IStaminaManager _staminaManager;
    private IRotationEnable _rotationEnable;

    private float HoldSpawnSparkl = 0.2f;
    private float timeToEnableCollider = 0.1f;
    private float diactivateCollider = 0.3f;
    private bool swordBool = false;
    private bool enableToRotate;

    [Inject]
    public void Construct(IAnimatorManager animatorManager, IStaminaManager staminaManager, ICombatInput combatInput, IRotationEnable rotationEnable)
    {
        _animatorManager = animatorManager;
        _staminaManager = staminaManager;
        _combatInput = combatInput;
        
        _rotationEnable = rotationEnable;
        _rotationEnable.SetRotationValue(true);
    }
    
    public void AttackBySword(Animator anim,  Transform colliderTransform)
    {
        _animatorManager.SwordAttackAnimation(anim, swordBool);
        
        if (_combatInput.IsRightMouseButtonDown() &&  !swordBool && _staminaManager.CanSwordAttack()) 
        {
            CoroutineRunner.Instance.StartCoroutine(AnimatorOn());
            _staminaManager.UseStamina(0.25f);
            CoroutineRunner.Instance.StartCoroutine(ActivateObject(colliderTransform));
        }
    }
    
    IEnumerator AnimatorOn()
    {
        swordBool = true;
        yield return new WaitForSeconds(0.3f);
        swordBool = false;
    }
    
    private IEnumerator ActivateObject(Transform colliderTransform)
    {
        if (colliderTransform == null)
        {
            yield break; 
        }

        _rotationEnable.SetRotationValue(false);
        yield return new WaitForSeconds(timeToEnableCollider);
        colliderTransform.gameObject.SetActive(true);
        yield return new WaitForSeconds(diactivateCollider);
        _rotationEnable.SetRotationValue(true);
        colliderTransform.gameObject.SetActive(false);
    }
}
