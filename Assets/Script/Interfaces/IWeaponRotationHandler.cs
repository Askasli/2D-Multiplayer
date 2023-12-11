using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponRotationHandler
{
    void WeaponRotation(Transform swordTransform, Transform bowTransform, Transform playerTransform);

}
