using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Zenject;


public class PlayerHealth : IPlayerHealth
{
    public event Action<float> OnHealthUpdated;
    private const float maxHealth = 100f;
    private float currentHealth;
    private bool isAlive = true; // Initialize as true

    public PlayerHealth(int maxHp)
    {
        currentHealth = maxHp;
    }
    public void UseHealth(float damage)
    {
        if (!isAlive) 
            return; 

        currentHealth -= damage;
        
        if (currentHealth < 0)
        {
            currentHealth = 0; 
            isAlive = false;
        }

        OnHealthUpdated?.Invoke((int)currentHealth);
    }
    

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public bool IsAlive()
    {
        return isAlive;
    }

    public void RestoreHealth(float amount)
    {
        
        if (!isAlive) return; 

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (currentHealth > 0)
        {
            isAlive = true;
        }
    }
}
