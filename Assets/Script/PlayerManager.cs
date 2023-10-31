using System;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    private IDash _dash;
    private IMeleeWeaponAttack _meleeWeaponAttack;
    private IWeaponRotationManager _weaponRotation;
    private IWeaponShootManager _weaponShootManager;
    private IMoveCharacter _moveCharacter;
    private ICharacterAnimatorRotation _animatorRotation;
    private ILayerManager _layerManager;
    private IUltimateTimer _ultimateTimer;
    private IStaminaManager _staminaManager;
    private IGroundChecker _groundChecker;
    
    [SerializeField] private  SpriteRenderer[] playerSpriteRenderers;
    [SerializeField] private GameObject[] dashFX;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject ultimateArrowPrefab;
    [SerializeField] private GameObject handLayer;
    [SerializeField] private GameObject bodyLayer;
    
    [SerializeField] private Transform spawnShootPoint;
    [SerializeField] private Transform bowTransform;
    [SerializeField] private Transform swordTransform;
    [SerializeField] private Transform colliderTransform;
    
    [SerializeField] private Animator anim_body;
    [SerializeField] private Animator anim_hands;
    [SerializeField] private new Collider2D collider;

    private PhotonView pv;
    private Rigidbody2D rigi;
    
    private const string PlayerTag = "Player";
    private const string EnemyTag = "Enemy";
    
    [Inject]
    public void Construct(ICharacterAnimatorRotation animatorRotation, IMoveCharacter moveCharacter, IDash dash, IMeleeWeaponAttack meleeWeaponAttack, 
        IWeaponRotationManager weaponRotation, IWeaponShootManager weaponShootManager, ILayerManager layerManager, IUltimateTimer ultimateTimer, IStaminaManager staminaManager, IGroundChecker groundChecker)
    {
        _dash = dash;
        _animatorRotation = animatorRotation;
        _moveCharacter = moveCharacter;
        _meleeWeaponAttack = meleeWeaponAttack;
        _weaponRotation = weaponRotation;
        _weaponShootManager = weaponShootManager;
        _layerManager = layerManager;
        _ultimateTimer = ultimateTimer;
        _staminaManager = staminaManager;
        _groundChecker = groundChecker;
    }

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        rigi = GetComponent<Rigidbody2D>();
        colliderTransform.gameObject.SetActive(false);
    }

    private void Start()
    {
        if (!pv.IsMine)
        { 
            rigi.isKinematic = true;
            gameObject.tag = EnemyTag;
        }
        else
        {
            gameObject.tag = PlayerTag;
        }
       
    }

    private void Update()
    {
        if (!pv.IsMine)
        {
            return;
        }
        
        HandlePlayerActions();
        
    }

    private void FixedUpdate()
    {
        if (!pv.IsMine)
        {
            return;
        }

        HandlePlayerMovement();
    }
    
    
    private void HandlePlayerActions()
    {
        _dash.FxEnable(dashFX); // Dash enable/disable
        _ultimateTimer.UpdateTimer(); 
        _staminaManager.UpdateStamina();
        
        _animatorRotation.MouseRotation(transform); 
        _animatorRotation.FlipManager(bodyLayer.transform); 
        _animatorRotation.BodyLayerRotation(anim_body, anim_hands); 
        _animatorRotation.HandLayerRotation(handLayer, rigi);
        
        _meleeWeaponAttack.AttackBySword(anim_body, colliderTransform);
        _weaponRotation.WeaponRotation(swordTransform, bowTransform, transform); 
        _weaponShootManager.BowShoot(anim_body, arrowPrefab, spawnShootPoint, gameObject); 
        _weaponShootManager.UltimateBowShoot(anim_hands, ultimateArrowPrefab, spawnShootPoint, gameObject);
    }

    private void HandlePlayerMovement()
    {
        _moveCharacter.MoveBody(rigi, anim_body); // Player Movement
        _dash.DashController(rigi, anim_body, collider); 
    }
    
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (pv.IsMine)
        {
            if (_groundChecker.CanCheckGround())
            {
                string layerName = _layerManager.GetLayerNameForCollision(collider.gameObject);

                if (!string.IsNullOrEmpty(layerName))
                {
                    _layerManager.ChangeLayerName(playerSpriteRenderers, layerName);
                    _layerManager.ChangeLayer(gameObject, layerName);
                    _layerManager.TrailLayerName(dashFX, layerName);
                }
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(bodyLayer.transform.localScale);
            stream.SendNext(handLayer.transform.rotation);
            stream.SendNext(handLayer.GetComponent<SpriteRenderer>().sortingLayerName);
            stream.SendNext(handLayer.activeSelf); // Send the activation status
            stream.SendNext(colliderTransform.gameObject.activeSelf);
            stream.SendNext(gameObject.layer);

            for (int i = 0; i < dashFX.Length; i++)
            {
                stream.SendNext(dashFX[i].gameObject.activeSelf);
                stream.SendNext(dashFX[i].GetComponent<TrailRenderer>().sortingLayerName);
            }

            for (int i = 0; i < playerSpriteRenderers.Length; i++)
            {
                stream.SendNext(playerSpriteRenderers[i].sortingLayerName);
            }
        }
        else
        {
            bodyLayer.transform.localScale = (Vector3)stream.ReceiveNext();
            handLayer.transform.rotation = (Quaternion)stream.ReceiveNext();
            string sortingLayerName = (string)stream.ReceiveNext();
            handLayer.GetComponent<SpriteRenderer>().sortingLayerName = sortingLayerName;
            handLayer.SetActive((bool)stream.ReceiveNext());
            colliderTransform.gameObject.SetActive((bool)stream.ReceiveNext());
            gameObject.layer = (int)stream.ReceiveNext();
            
            for (int i = 0; i < dashFX.Length; i++)
            {
                dashFX[i].gameObject.SetActive((bool)stream.ReceiveNext());
                dashFX[i].GetComponent<TrailRenderer>().sortingLayerName = (string)stream.ReceiveNext();
            }

            for (int i = 0; i < playerSpriteRenderers.Length; i++)
            {
                playerSpriteRenderers[i].sortingLayerName = (string)stream.ReceiveNext();
            }
        }
    }

}




