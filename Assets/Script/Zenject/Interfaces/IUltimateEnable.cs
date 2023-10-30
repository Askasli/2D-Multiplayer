using System;
using UnityEngine;


public interface IUltimateEnable 
{
    event Action<bool> OnUltimateChanged;
    bool CanUltimate();
    void SetCanUltimate(bool value);
}
