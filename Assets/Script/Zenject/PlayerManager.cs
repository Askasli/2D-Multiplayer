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
    public SpriteRenderer[] playerSpriteRenderers;
   
    [SerializeField] private GameObject[] dashFX;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject ultimateArrowPrefab;
    [SerializeField] private GameObject handLayer;
    [SerializeField] private GameObject bodyLayer;
    
    [SerializeField] private Transform spawnShootPoint;
    [SerializeField] private Transform bowTransform;
    [SerializeField] private Transform swordTransform;
    
    [SerializeField] private Animator anim_body;
    [SerializeField] private Animator anim_hands;
  
    [SerializeField] private Transform colliderTransform;
    [SerializeField] private new Collider2D collider;

    private PhotonView pv;
    private Rigidbody2D rigi;

    [Inject]
    public void Construct(ICharacterAnimatorRotation animatorRotation, IMoveCharacter moveCharacter, IDash dash, IMeleeWeaponAttack meleeWeaponAttack, 
        IWeaponRotationManager weaponRotation, IWeaponShootManager weaponShootManager, ILayerManager layerManager)
    {
        _dash = dash;
        _animatorRotation = animatorRotation;
        _moveCharacter = moveCharacter;
        _meleeWeaponAttack = meleeWeaponAttack;
        _weaponRotation = weaponRotation;
        _weaponShootManager = weaponShootManager;
        _layerManager = layerManager;
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
            gameObject.tag = "Enemy";
        }
        else
        {
            gameObject.tag = "Player";
        }
       
    }

    private void Update()
    {
        if (!pv.IsMine)
        {
            return;
        }

        _dash.FxEnable(dashFX);
        _animatorRotation.MouseRotation(transform);
        _animatorRotation.FlipManager(bodyLayer.transform);
        _animatorRotation.BodyLayerRotation(anim_body, anim_hands);
        _animatorRotation.HandLayerRotation(handLayer, rigi);
        _meleeWeaponAttack.AttackBySword(anim_body, colliderTransform);
        _weaponRotation.WeaponRotation(swordTransform, bowTransform, transform);
        _weaponShootManager.BowShoot(anim_body, anim_hands, arrowPrefab, spawnShootPoint, gameObject);
        _weaponShootManager.UltimateBowShoot(anim_hands, ultimateArrowPrefab, spawnShootPoint, gameObject);

    }

    private void FixedUpdate()
    {
        if (!pv.IsMine)
        {
            return;
        }
        
        _moveCharacter.MoveBody(rigi, anim_body);
        _dash.DashController(rigi, anim_body, collider);
    }

  
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (pv.IsMine)
        {
            string layerName = GetLayerNameForCollision(collider.gameObject);

            if (!string.IsNullOrEmpty(layerName))
            {
                _layerManager.ChangeLayerName(playerSpriteRenderers, layerName);
                _layerManager.ChangeLayer(gameObject, layerName);
                _layerManager.TrailLayerName(dashFX, layerName);
            }
        }
    }
    
    private string GetLayerNameForCollision(GameObject collisionObject)
    {
        string layerName = null;

        if (collisionObject.CompareTag("FirstFloor"))
        {
            layerName = "Layer 1";
        }
        else if (collisionObject.CompareTag("SecondFloor"))
        {
            layerName = "Layer 2";
        }
        else if (collisionObject.CompareTag("ThirdFloor"))
        {
            layerName = "Layer 3";
        }

        return layerName;
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




