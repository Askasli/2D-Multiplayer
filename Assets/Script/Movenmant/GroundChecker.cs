using System;
using UnityEngine;


public class GroundChecker : IGroundChecker
{
    public event Action<bool> OnGroundChanged;
    private bool groundBool; 
    
    public bool CanCheckGround()
    {
        return groundBool;
    }
    public void SetGroundCheck(bool value)
    {
        if (groundBool != value)
        {
            groundBool = value;
            OnGroundChanged?.Invoke(groundBool);
        }
    }
}
