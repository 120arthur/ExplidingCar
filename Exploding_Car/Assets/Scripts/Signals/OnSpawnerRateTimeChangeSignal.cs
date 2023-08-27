using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Represents a signal that is triggered when the player changes the value of the slider
/// for the spawning rate time in the HUD.
/// </summary>
public class OnSpawnerRateTimeChangeSignal
{
    public float RateTime;

    public OnSpawnerRateTimeChangeSignal(float rateTime)
    {
        RateTime = rateTime;
    }
}
