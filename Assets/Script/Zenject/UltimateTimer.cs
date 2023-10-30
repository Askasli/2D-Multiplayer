using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UltimateTimer : IUltimateTimer
{
    public event Action<float> OnTimerUpdated = delegate { };

    private float timer;
    private float ultimateDuration;

    public void StartUltimateTimer(float duration)
    {
        ultimateDuration = duration;
        timer = 0f;
    }
    public void ResetUltimateTimer()
    {
        timer = 0f;
    }
    public float GetTimeRemaining()
    {
        return Mathf.Clamp01(timer / ultimateDuration);
    }

    public bool IsUltimateReady()
    {
        return timer >= ultimateDuration;
    }

    public void UpdateTimer()
    {
        if(!IsUltimateReady())
        {
            timer += Time.deltaTime;
            OnTimerUpdated.Invoke(GetTimeRemaining());
        }
    }
}