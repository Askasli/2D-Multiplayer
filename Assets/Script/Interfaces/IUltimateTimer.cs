using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUltimateTimer
{
    event Action<float> OnTimerUpdated;
    void StartUltimateTimer(float duration);
    void ResetUltimateTimer();
    float GetTimeRemaining();
    bool IsUltimateReady();
    void UpdateTimer();
}
