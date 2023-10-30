using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICombatInput 
{
    bool IsLeftMouseButtonDown();
    bool IsRightMouseButtonDown();
    bool IsUltimateButtonDown();
    bool IsDashButtonDown();

}
