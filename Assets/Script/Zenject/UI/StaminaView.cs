using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class StaminaView : MonoBehaviourPun
{
   private IStaminaManager _staminaManager;
   [SerializeField]private Slider staminaSlider;

   [Inject]
   public void Construct(IStaminaManager staminaManager)
   {
      _staminaManager = staminaManager;
   }

   private void Start()
   {
      _staminaManager.OnStaminaUpdated += UpdateStamina; 
   }
   
   private void OnDestroy()
   {
      _staminaManager.OnStaminaUpdated -= UpdateStamina;
   }
   
   private void UpdateStamina(float value)
   {
       staminaSlider.value = value;
   }
}
