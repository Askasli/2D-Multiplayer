using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public interface IPlayerHealth
{
   event Action<float> OnHealthUpdated;
   void UseHealth(float amount);
   float GetCurrentHealth();
   bool IsAlive();
   void RestoreHealth(float amount);
}
