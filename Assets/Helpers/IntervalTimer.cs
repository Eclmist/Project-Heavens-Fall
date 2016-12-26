#region Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion


class IntervalTimer
{
    private float elapsedTime, interval;

    public IntervalTimer(float interval)
    {
        this.interval = interval;
    }

    public bool Check(float deltaTime)
    {
        elapsedTime += deltaTime;
        if (elapsedTime >= interval)
        {
            elapsedTime -= interval;
            return true;
        }
        return false;
    }

    public void Reset()
    {
        elapsedTime = 0;
    }

    public void ChangeInterval(float interval, bool reset = true)
    {
        this.interval = interval;
        if (reset) Reset();
    }
}
