
    using System;
    using UnityEngine;


    public class StaminaManager : IStaminaManager
    {
        public event Action<float> OnStaminaUpdated;

        public float MaxStamina { get; }
        public float StaminaRegenRate { get; }

        // Properties for different stamina costs
        public float DashStaminaCost { get; }
        public float ShootStaminaCost { get; }
        public float SwordStaminaCost { get; }

        // Current stamina field
        private float currentStamina = 1f;

        public StaminaManager(float maxStamina, float staminaRegenRate)
        {
            MaxStamina = maxStamina;
            StaminaRegenRate = staminaRegenRate;
            
            // Initialize stamina costs
            DashStaminaCost = 0.2f;
            ShootStaminaCost = 0.1f;
            SwordStaminaCost = 0.25f;
        }

        public void UseStamina(float amount)
        {
            if (currentStamina >= amount)
            {
                currentStamina -= amount;
                OnStaminaUpdated?.Invoke(currentStamina);
            }
        }
        public float GetCurrentStamina()
        {
            return currentStamina;
        }

        public void UpdateStamina()
        {
            if (currentStamina < MaxStamina)
            {
                currentStamina += StaminaRegenRate * Time.deltaTime;
                currentStamina = Mathf.Clamp(currentStamina, 0f, MaxStamina);
                OnStaminaUpdated?.Invoke(currentStamina);
            }
        }
        
        public bool CanDash()
        {
            return currentStamina >= DashStaminaCost;
        }

        public bool CanShoot()
        {
            return currentStamina >= ShootStaminaCost;
        }

        public bool CanSwordAttack()
        {
            return currentStamina >= SwordStaminaCost;
        }
    }
