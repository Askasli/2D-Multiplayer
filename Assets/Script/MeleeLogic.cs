using System;
using UnityEngine;


public class MeleeLogic 
{
    private bool canAttack;
    private bool enableToRotate;
    public event Action<bool> OnCanAttackChanged;
    public event Action<bool> OnEnableToRotateChanged;
    
    public bool CanAttack
    {
        get { return canAttack; }
        set
        {
            if (canAttack != value)
            {
                canAttack = value;
                Debug.Log(canAttack + "canAttack value");
                OnCanAttackChanged?.Invoke(canAttack);
            }
        }
    }

    public bool EnableToRotate
    {
        get { return enableToRotate; }
        set
        {
            if (enableToRotate != value)
            {
                enableToRotate = value;
                OnEnableToRotateChanged?.Invoke(enableToRotate);
            }
        }
    }
}
