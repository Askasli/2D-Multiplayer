using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WeaponRotationManager : IWeaponRotationManager
{
    private IMousePosition _mousePosition;
    private IRotationEnable _rotationEnable;
    private Vector3 mousePos;

    [Inject]
    public void Construct(IMousePosition mousePosition,  IRotationEnable rotationEnable)
    {
        _mousePosition = mousePosition;
        _rotationEnable = rotationEnable;
    }
    
    public void WeaponRotation(Transform swordTransform, Transform bowTransform, Transform playerTransform)
    {
        mousePos = _mousePosition.mousePosition(playerTransform);  // Rotate the weapon based on mouse position.
        mousePos.Normalize();

        float rot_z = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;  // Calculate the rotation angle in degrees based on the mouse position.
        bowTransform.rotation = Quaternion.Euler(0f, 0f, rot_z);    // Rotate the bow to aim at the mouse position.

        if (_rotationEnable.CanRotate())    // Check if weapon rotation is enabled
        {
            swordTransform.rotation = Quaternion.Euler(0f, 0f, rot_z);
        }
    }
}
