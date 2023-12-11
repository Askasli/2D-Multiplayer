using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MoveCharacter : IMoveCharacter
{
    private float speed = 4f;
    private float magnitude;
    private IAnimatorHandler _animator;
    private IMoveDirection _moveDirection;

    [Inject]
    private void Construct(IAnimatorHandler animator,  IMoveDirection moveDirection)
    {
        _animator = animator;
        _moveDirection = moveDirection;
    }
    
    public void MoveBody(Rigidbody2D rigi, Animator animator) // Player Movement
    {
        rigi.velocity = _moveDirection.moveDirection() * speed;
        magnitude = _moveDirection.moveDirection().magnitude;
        
        _animator.MoveAnimation(animator, magnitude > 0);
    }
}
