using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Dash : IDash
{
    private IAnimatorManager _animatorManager;
    private ICombatInput _combatInput;
    private IStaminaManager _staminaManager;
    private IMoveDirection _moveDirection;
    private IGroundChecker _groundChecker;
    
    private bool enableFx = false;
    private bool dashBool = false;
    private bool dashForce = false;
    private Vector3 moveDir;
    
    [Inject]
    private void Construct(IAnimatorManager animatorManager, ICombatInput combatInput,  IStaminaManager staminaManager, IMoveDirection moveDirection, IGroundChecker groundChecker)
    {
        _animatorManager = animatorManager;
        _combatInput = combatInput;
        _staminaManager = staminaManager;
        _moveDirection = moveDirection;
        _groundChecker = groundChecker;
    }
    
    public void FxEnable(GameObject[] transform)
    {
        for (int i = 0; i < transform.Length; i++)
        {
            transform[i].SetActive(enableFx);
        }
    }

    public void DashController(Rigidbody2D rigi, Animator animator, Collider2D collider2D)
    {
        _animatorManager.DashAnimation(animator, dashBool); //Dash animator 

        if (_combatInput.IsDashButtonDown() && _staminaManager.CanDash()) //Dash activation
        {
            if (_moveDirection.moveDirection().magnitude > 0) 
            {
                dashForce = true;
                CoroutineRunner.Instance.StartCoroutine(DashFxOn()); //Enable the trail renderer.
                CoroutineRunner.Instance.StartCoroutine(DashForce(rigi)); //Rigidbody force to make a dash.
                CoroutineRunner.Instance.StartCoroutine(DashAnimatorEnable());  //Enable the animation.
                CoroutineRunner.Instance.StartCoroutine(DashColliderEnable(collider2D)); //Disable the collider to pass through walls
            }
        }
    }

    IEnumerator DashFxOn()
    {
        enableFx = true;
        yield return new WaitForSeconds(0.26f);
        enableFx = false;
    }

    IEnumerator DashAnimatorEnable()
    {
        dashBool = true;
        yield return new WaitForSeconds(0.1f);
        dashBool = false;
    }

    IEnumerator DashForce(Rigidbody2D rigidbody2D)
    {
        _staminaManager.UseStamina(0.2f);
        
        if (dashForce)
        {
            rigidbody2D.AddForce(_moveDirection.moveDirection() * 100f, ForceMode2D.Impulse);
            dashForce = false;
        }
        
        yield return null;
    }
    
    IEnumerator DashColliderEnable(Collider2D collider2D)
    {
        _groundChecker.SetGroundCheck(true);
        collider2D.enabled = false;
        yield return new WaitForSeconds(0.3f);
        collider2D.enabled = true;
        yield return new WaitForSeconds(0.5f);
        _groundChecker.SetGroundCheck(false);
    }
}
