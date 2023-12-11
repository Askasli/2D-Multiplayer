using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MeleeWeaponAttack : IMeleeWeaponAttack
{
    private IAnimatorHandler _animator;
    private ICombatInput _combatInput;
    private IStaminaHandler _staminaHandler;
    private IRotationEnable _rotationEnable;

    private const float TimeToEnableCollider = 0.1f;
    private const float DeactivateCollider = 0.3f;
    
    private bool swordBool = false;
    private bool enableToRotate;

    [Inject]
    public void Construct(IAnimatorHandler animator, IStaminaHandler staminaHandler, ICombatInput combatInput, IRotationEnable rotationEnable)
    {
        _animator = animator;
        _staminaHandler = staminaHandler;
        _combatInput = combatInput;
        
        _rotationEnable = rotationEnable;
        _rotationEnable.SetRotationValue(true);
    }
    
    public void AttackBySword(Animator anim,  Transform colliderTransform)
    {
        _animator.SwordAttackAnimation(anim, swordBool);
        
        // Check if you can attack by sword
        if (_combatInput.IsRightMouseButtonDown() &&  !swordBool && _staminaHandler.CanSwordAttack()) 
        {
            CoroutineRunner.Instance.StartCoroutine(AnimatorOn()); // Start sword attack animation
            _staminaHandler.UseStamina(0.25f);  // Using Stamina
            CoroutineRunner.Instance.StartCoroutine(ActivateObject(colliderTransform)); // Activate collider
        }
    }
    
    IEnumerator AnimatorOn()
    {
        swordBool = true;
        yield return new WaitForSeconds(0.3f);
        swordBool = false;
    }
    
    IEnumerator ActivateObject(Transform colliderTransform)
    {
        if (colliderTransform != null)
        {
            _rotationEnable.SetRotationValue(false);
            yield return new WaitForSeconds(TimeToEnableCollider);
            colliderTransform.gameObject.SetActive(true);
            yield return new WaitForSeconds(DeactivateCollider);
            _rotationEnable.SetRotationValue(true);
            colliderTransform.gameObject.SetActive(false);
        }
    }
}
