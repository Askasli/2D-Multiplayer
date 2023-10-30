using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterAnimatorRotation
{
    void BodyLayerRotation(Animator body, Animator hand);
    void FlipManager(Transform spriteTransform);
    void MouseRotation(Transform playerPosition);
    void HandLayerRotation(GameObject handLayer, Rigidbody2D rigidbody);
}
