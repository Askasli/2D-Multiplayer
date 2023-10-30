using System;
using UnityEngine;


public interface IGroundChecker 
{
    event Action<bool> OnGroundChanged;
    
    bool CanCheckGround();
    
    void SetGroundCheck(bool value);
}
