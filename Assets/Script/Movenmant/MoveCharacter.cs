using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MoveCharacter : IMoveCharacter
{
    private float speed = 4f;
    private float magnitude;
    private IAnimatorManager _animatorManager;
    private IMoveDirection _moveDirection;

    [Inject]
    private void Construct(IAnimatorManager animatorManager,  IMoveDirection moveDirection)
    {
        _animatorManager = animatorManager;
        _moveDirection = moveDirection;
    }
    
    public void MoveBody(Rigidbody2D rigi, Animator animator) // Player Movement
    {
        rigi.velocity = _moveDirection.moveDirection() * speed;
        magnitude = _moveDirection.moveDirection().magnitude;
        
        _animatorManager.MoveAnimation(animator, magnitude > 0);
    }
}
