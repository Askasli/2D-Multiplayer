using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CharacterAnimatorRotation : ICharacterAnimatorRotation
{
    private IAnimatorManager _animatorManager;
    private ICombatInput _combatInput;
    private IFlipSprite _flipSprite;
    private IHandAnimator _handAnimator;
    private IMousePosition _mousePosition;
    private IInputManager _inputManager;
    private IUltimateEnable _ultimateEnable;
    
    private float verticalDirection;
    private float horizontalDirection;
    private float timeAfterAttack;
    private float bowRot_z;
    private float timingAnimator;
    
    private Vector3 movement;
    private Vector2 mousePos;
    
    private Transform _sprite;
    private Transform _playerTransform;
    
    
    [Inject]
    private void Construct(IAnimatorManager animatorManager, ICombatInput combatInput, IFlipSprite flipSprite, IHandAnimator handAnimator, IMousePosition mousePosition, IInputManager inputManager, IUltimateEnable ultimateEnable)
    {
        _animatorManager = animatorManager;
        _combatInput = combatInput;
        _flipSprite = flipSprite;
        _handAnimator = handAnimator;
        _mousePosition = mousePosition;
        _inputManager = inputManager;
        _ultimateEnable = ultimateEnable;
    }
    
    public void FlipManager(Transform spriteTransform)
    {
        _sprite = spriteTransform;
    }

    public void MouseRotation(Transform playerPosition) //Check the mouse position relative to the player.
    {
        _playerTransform = playerPosition;
        mousePos = _mousePosition.mousePosition(playerPosition);
        mousePos.Normalize();
    }

    // Control the character's animations, including rotation and movement.
    public void BodyLayerRotation(Animator body, Animator hand)  // Control the character's animations, rotation
    {
        _animatorManager.MoveHorizontal(body, horizontalDirection, 0.1f, timingAnimator);
        _animatorManager.MoveVertical(body, verticalDirection, 0.1f, timingAnimator);
        _handAnimator.RotationHand(hand, horizontalDirection, verticalDirection, 0.1f, 1f);
        
        // During actions, attacks, shooting, and using ultimate, 
        // the player will look in the direction of the mouse.
        
        if (_combatInput.IsRightMouseButtonDown() || _combatInput.IsLeftMouseButtonDown() || _ultimateEnable.CanUltimate()) 
        {
            verticalDirection = mousePos.y;
            horizontalDirection = mousePos.x;
            timeAfterAttack = 1;
        }
        else
        { 
            if(timeAfterAttack == 0)
            {
                movement = new Vector3(_inputManager.GetTurnInput(), 0f, _inputManager.GetForwardInput());
                timingAnimator = Time.deltaTime;
               
                
                if (movement.magnitude > 0)
                {
                    verticalDirection = Vector3.Dot(movement.normalized, _playerTransform.forward);
                    horizontalDirection = Vector3.Dot(movement.normalized, _playerTransform.right);

                    movement.Normalize();
                    movement *= 1 * Time.deltaTime;
                }
            }
            else if(timeAfterAttack == 1)
            {
                CoroutineRunner.Instance.StartCoroutine(TimerAfterAttack());
            }
        }
        
        if (horizontalDirection < 0 && !_flipSprite.IsFacingRight() || horizontalDirection > 0 && _flipSprite.IsFacingRight())
        {
            _flipSprite.Flip(_sprite);
        }
    }
    
    // Control hand animations and shooting.
    public void HandLayerRotation(GameObject handLayer, Rigidbody2D rigidbody) 
    {
        if (_combatInput.IsLeftMouseButtonDown() || _ultimateEnable.CanUltimate()) 
        {
            CoroutineRunner.Instance.StartCoroutine(ActivateShootLayer(0.1f, true, handLayer));
            
            if (verticalDirection < 0.71f && verticalDirection > -0.71f)
            {
                bowRot_z = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
                handLayer.transform.rotation = Quaternion.Euler(0f, 0f, bowRot_z);
            }
            else
            {
                handLayer.transform.rotation = Quaternion.Euler(0f, 0f, 0);
            }

            if (rigidbody.velocity.magnitude > 0) // activation hands layer
            {
                CoroutineRunner.Instance.StartCoroutine(LayerController(handLayer, 0.39f));
            }
            else if (rigidbody.velocity.magnitude <= 0) // deactivation hands layer
            {
                CoroutineRunner.Instance.StartCoroutine(LayerController(handLayer, 0.7f));
            }
        }
        else
        {
            CoroutineRunner.Instance.StartCoroutine(ActivateShootLayer(0.1f, false, handLayer));
            Debug.Log("stop shoot");
        }
    }
   // Timer after an attack.
    IEnumerator TimerAfterAttack()
    {
        yield return new WaitForSeconds(0.4f);
        timeAfterAttack = 0;
    }
    
    // Activate/Deactivate the shooting layer.
    IEnumerator ActivateShootLayer(float timeToActivate, bool activateBool, GameObject gameObject)
    {
        yield return new WaitForSeconds(timeToActivate);
        
        if (gameObject != null)
            gameObject.SetActive(activateBool);
    }
    
    // Control the animation layer.
    IEnumerator LayerController(GameObject gameObject, float angle)
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        string sortingLayerName;
        int sortingOrder;

        if (verticalDirection > angle)
        {
            sortingLayerName = "Far";
            sortingOrder = 2;
        }
        else
        {
            sortingLayerName = "MuchCloser";
            sortingOrder = 3;
        }

        spriteRenderer.sortingLayerName = sortingLayerName;
        spriteRenderer.sortingOrder = sortingOrder;

        yield return null;
    }
    
}
