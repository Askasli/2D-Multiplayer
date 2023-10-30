using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponRotationManager
{
    void WeaponRotation(Transform swordTransform, Transform bowTransform, Transform playerTransform);

}
