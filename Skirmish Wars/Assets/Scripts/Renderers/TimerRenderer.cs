using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public sealed class TimerRenderer : MonoBehaviour
{
    public SpriteRenderer clockSprite;
    public Text clockText;
    public Timer drivingTimer;
    
    private void OnTimerStarted()
    {

    }

    private IEnumerator WhileTimerTicking()
    {
        throw new NotImplementedException();
    }

    private void OnTimerElapsed()
    {

    }


}