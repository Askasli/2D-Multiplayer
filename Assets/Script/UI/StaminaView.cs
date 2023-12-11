using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class StaminaView : MonoBehaviourPun
{
   private IStaminaHandler _staminaHandler;
   [SerializeField]private Slider staminaSlider;

   [Inject]
   public void Construct(IStaminaHandler staminaManager)
   {
      _staminaHandler = staminaManager;
   }

   private void Start()
   {
      _staminaHandler.OnStaminaUpdated += UpdateStamina; 
   }
   
   private void OnDestroy()
   {
      _staminaHandler.OnStaminaUpdated -= UpdateStamina;
   }
   
   private void UpdateStamina(float value)
   {
       staminaSlider.value = value;
   }
}
