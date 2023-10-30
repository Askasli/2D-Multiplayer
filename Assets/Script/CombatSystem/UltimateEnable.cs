using System;
using UnityEngine;

public class UltimateEnable : IUltimateEnable
{
    public event Action<bool> OnUltimateChanged;
    private bool canUltimateBool;
    
    public bool CanUltimate()
    {
        return canUltimateBool;
    }

    public void SetCanUltimate(bool value)
    {
        if (canUltimateBool != value)
        {
            canUltimateBool = value;
            OnUltimateChanged?.Invoke(canUltimateBool);
        }
    }
}

