using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CharacterAnimatorRotation : ICharacterAnimatorRotation
{
    private IAnimatorHandler _animator;
    private ICombatInput _combatInput;
    private IFlipSprite _flipSprite;
    private IHandAnimator _handAnimator;
    private IMousePosition _mousePosition;
    private IMoveInput _moveInput;
    private IUltimateEnable _ultimateEnable;
    
    private float verticalDirection;
    private float horizontalDirection;
    
    private bool afterAttackEnable = false;
    
    private float bowRot_z;
    private float timingAnimator;
    
    private Vector3 movement;
    private Vector2 mousePos;
    
    private Transform _sprite;
    private Transform _playerTransform;
    
    
    [Inject]
    private void Construct(IAnimatorHandler animator, ICombatInput combatInput, IFlipSprite flipSprite, IHandAnimator handAnimator, IMousePosition mousePosition, IMoveInput moveInput, IUltimateEnable ultimateEnable)
    {
        _animator = animator;
        _combatInput = combatInput;
        _flipSprite = flipSprite;
        _handAnimator = handAnimator;
        _mousePosition = mousePosition;
        _moveInput = moveInput;
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
        _animator.MoveHorizontal(body, horizontalDirection, 0.1f, timingAnimator);
        _animator.MoveVertical(body, verticalDirection, 0.1f, timingAnimator);
        _handAnimator.RotationHand(hand, horizontalDirection, verticalDirection, 0.1f, 1f);
        
        // During actions, attacks, shooting, and using ultimate, 
        // the player will look in the direction of the mouse.
        
        if (_combatInput.IsRightMouseButtonDown() || _combatInput.IsLeftMouseButtonDown() || _ultimateEnable.CanUltimate()) 
        {
            verticalDirection = mousePos.y;
            horizontalDirection = mousePos.x;
            afterAttackEnable = true;
        }
        else
        { 
            if(!afterAttackEnable)
            {
                // Get movement input from the input manager
                movement = new Vector3(_moveInput.GetTurnInput(), 0f, _moveInput.GetForwardInput());
                timingAnimator = Time.deltaTime;
               
                
                if (movement.magnitude > 0)
                {
                    // Calculate vertical and horizontal directions based on movement
                    verticalDirection = Vector3.Dot(movement.normalized, _playerTransform.forward);
                    horizontalDirection = Vector3.Dot(movement.normalized, _playerTransform.right);

                    // Normalize movement and scale it by 1 * Time.deltaTime
                    movement.Normalize();
                    movement *= 1 * Time.deltaTime;
                }
            }
            else
            {
                CoroutineRunner.Instance.StartCoroutine(TimerAfterAttack());
            }
        }
        
        if (horizontalDirection < 0 && !_flipSprite.IsFacingRight() || horizontalDirection > 0 && _flipSprite.IsFacingRight())
        {
            _flipSprite.Flip(_sprite);
        }
    }
    
    // Control hand animations.
    public void HandLayerRotation(GameObject handLayer, Rigidbody2D rigidbody) 
    {
        if (_combatInput.IsLeftMouseButtonDown() || _ultimateEnable.CanUltimate()) 
        {
            // Activate shooting layer with a short delay
            CoroutineRunner.Instance.StartCoroutine(ActivateShootLayer(0.1f, true, handLayer));
            
            // Check if the vertical direction is within a certain range
            if (verticalDirection < 0.71f && verticalDirection > -0.71f)
            {
                // Calculate the rotation angle based on the mouse position
                bowRot_z = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
                handLayer.transform.rotation = Quaternion.Euler(0f, 0f, bowRot_z);
            }
            else
            {
                // Set a default rotation 
                handLayer.transform.rotation = Quaternion.Euler(0f, 0f, 0);
            }

            // Check the magnitude of the rigidbody's velocity to determine layer activation
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
        afterAttackEnable = false;
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
            sortingOrder = 2;
        }
        else
        {
            sortingOrder = 3;
        }

        spriteRenderer.sortingOrder = sortingOrder;

        yield return null;
    }
    
}
