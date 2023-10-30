
    using System;

    public interface IStaminaManager
    {
        event Action<float> OnStaminaUpdated;
        void UseStamina(float amount);
        float GetCurrentStamina();
        void UpdateStamina();
        bool CanDash();
        
        bool CanShoot();
        
        bool CanSwordAttack();

    }
