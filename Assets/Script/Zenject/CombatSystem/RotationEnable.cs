using System;
using UnityEngine;


public class RotationEnable : IRotationEnable
{
    public event Action<bool> OnRotationChange;
    private bool canRotate;
    
    public bool CanRotate()
    {
        return canRotate;
    }

    public void SetRotationValue(bool value)
    {
        if (canRotate != value)
        {
            Debug.Log(value + "RotateEnableStatus");
            canRotate = value;
            OnRotationChange?.Invoke(canRotate);
        }
    }
}
