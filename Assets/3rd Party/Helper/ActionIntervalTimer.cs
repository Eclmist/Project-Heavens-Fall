using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionIntervalTimer
{
    private MonoBehaviour attachedBehaviour;
    private Action action;
    public bool running = true;
    private float interval;

    /// <summary>
    /// Create an ActionIntervalTimer that calls an Action every interval
    /// </summary>
    /// <param name="attachedBehaviour"> Type "this" here</param>
    /// <param name="action">Function delegate to be called every interval</param>
    /// <param name="interval">Time between function calls</param>
    public ActionIntervalTimer(MonoBehaviour attachedBehaviour, Action action, float interval)
    {
        this.interval = interval;
        this.action = action;
        this.attachedBehaviour = attachedBehaviour;
        attachedBehaviour.StartCoroutine(Repeater());
    }

    IEnumerator Repeater()
    {
        while (true)
        {
            action.Invoke();
            yield return new WaitForSeconds(interval);
            if (!GetState())yield return new WaitUntil(GetState);
        }
    }

    private bool GetState()
    {
        return running;
    }
}
