using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatInput : ICombatInput
{
    public bool IsLeftMouseButtonDown()
    {
        return Input.GetKey(KeyCode.Mouse0); 
    }

    public bool IsRightMouseButtonDown()
    {
        return Input.GetKeyDown(KeyCode.Mouse1);
    }

    public bool IsUltimateButtonDown()
    {
        return Input.GetKey(KeyCode.Q); 
    }

    public bool IsDashButtonDown()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }
}