using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider hpSlider;
    private IPlayerHealth _playerHealth;

    [Inject]
    public void Construct(IPlayerHealth playerHealth)
    {
        _playerHealth = playerHealth;
    }

    private void Start()
    {
        if (_playerHealth != null)
        {
            _playerHealth.OnHealthUpdated += UpdateHealth;
        }
    }
    
    private void OnDestroy()
    {
        if (_playerHealth != null)
        {
            _playerHealth.OnHealthUpdated -= UpdateHealth;
        }
    }

    private void UpdateHealth(float value)
    {
        if (hpSlider != null)
        {
            hpSlider.value = value;
        }
    }
}


