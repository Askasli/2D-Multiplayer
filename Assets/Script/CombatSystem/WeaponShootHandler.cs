using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Zenject;

public class WeaponShootHandler : IWeaponShootHandler
{
    private ICombatInput _combatInput;
    private IUltimateTimer _ultimateTimer;
    private IStaminaHandler _staminaHandler;
    private IAnimatorHandler _animator;
    private IUltimateEnable _ultimateEnable;
    
    private const float ArrowSpawnDelay = 0.4f;
    private const float ShootCooldown = 0.66f;
    private const float StaminaCostPerShot = 0.1f;

    private bool shootArrowBool;
    private float time;
    private int spawnShoot;
  
    
    [Inject]
    public void Construct(ICombatInput combatInput, IUltimateTimer ultimateTimer, IStaminaHandler staminaHandler, IAnimatorHandler animator, IUltimateEnable ultimateEnable)
    {
        _combatInput = combatInput;
        _ultimateTimer = ultimateTimer;
        _staminaHandler = staminaHandler;
        _animator = animator;
        _ultimateEnable = ultimateEnable;
    }

    // Handle shooting with a bow.
    public void BowShoot(Animator bodyAnimator, GameObject bullet, Transform spawnPoint, GameObject player)
    {
        _animator.ShootAnimation(bodyAnimator, shootArrowBool);
    
        if (_combatInput.IsLeftMouseButtonDown())
        {
            if (time <= 0)
            {
                if (_staminaHandler.CanShoot() && !_ultimateEnable.CanUltimate())
                {
                    CoroutineRunner.Instance.StartCoroutine(ArrowSpawn(bullet, spawnPoint, player));
                }

                time = ShootCooldown;
            }
        }
        else
        {
            shootArrowBool = _ultimateEnable.CanUltimate();
        }

        time -= 1 * Time.deltaTime;
    }

    //Spawn arrows.
    IEnumerator ArrowSpawn(GameObject bulletPrefab, Transform spawnPoint, GameObject player)
    {  
        shootArrowBool = true;
        yield return new WaitForSeconds(ArrowSpawnDelay);

        if (shootArrowBool)
        { 
            _staminaHandler.UseStamina(StaminaCostPerShot);
            //  Later, I want to create an object pool using Zenject's IMemoryPool. For now, a temporary solution.
            GameObject arrow = PhotonNetwork.Instantiate(bulletPrefab.name, spawnPoint.position, spawnPoint.rotation); 
            arrow.layer = player.layer;
        }
    }

    // Handle shooting with an ultimate
    public void UltimateBowShoot(Animator handAnimator, GameObject bullet, Transform spawnPoint, GameObject player)
    {
        _animator.UltimateShootAnimation(handAnimator, _ultimateEnable.CanUltimate());
        AnimatorStateInfo stateInfo = handAnimator.GetCurrentAnimatorStateInfo(0);
        
        if (_combatInput.IsUltimateButtonDown() && _ultimateTimer.IsUltimateReady())
        {
            _ultimateTimer.ResetUltimateTimer();
            _ultimateEnable.SetCanUltimate(true);
        }
        
        if ( _ultimateEnable.CanUltimate())
        {
            if (stateInfo.IsName("UltimateShooting") && stateInfo.normalizedTime >= 0.7f)
            {
                Debug.Log("Spawn Ultimate Arrow");
                CoroutineRunner.Instance.StartCoroutine(UltimateShoot(bullet, spawnPoint, player));
            }
            
            if (stateInfo.IsName("UltimateShooting") && stateInfo.normalizedTime >= 1f)
            {
                Debug.Log("UltimateShoot animation clip is over.");
                _ultimateEnable.SetCanUltimate(false);
                spawnShoot = 0;
            }
        }
    }

    // Spawn ultimate arrows.
    IEnumerator UltimateShoot(GameObject bulletPrefab, Transform spawnPoint, GameObject player)
    {
        if (spawnShoot != 1)
        {
            GameObject ultimateArrow =
                PhotonNetwork.Instantiate(bulletPrefab.name, spawnPoint.position, spawnPoint.rotation);
            ultimateArrow.layer = player.layer;
            spawnShoot = 1;
        }

        yield return null;
    }
}
