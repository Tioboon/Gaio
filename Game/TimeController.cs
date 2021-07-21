using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimeController
{
    public static int timeInMin;
    public static bool raining;
    public static TimeInfo time;

    public enum TimeInfo
    {
        morning,
        evening,
        night,
        raining,
    }
    
    public static void AddTime(int timeToAdd)
    {
        switch (timeInMin + timeToAdd < 1440)
        {
            case true:
                timeInMin += timeToAdd;
                break;
            case false:
                timeInMin += timeToAdd;
                timeInMin -= 1440;
                break;
        }

        var randomRain = Random.Range(0, 20);
        switch (raining)
        {
            case true:
                switch (randomRain == 1)
                {
                    case true:
                        raining = true;
                        break;
                }
                break;
            case false:
                switch (randomRain < 20)
                {
                    case true:
                        raining = false;
                        break;
                }
                break;
        }
        ChangeTimeInfo(timeInMin);
    }

    private static void ChangeTimeInfo(int actualTime)
    {
        time = TimeInfo.raining;
        if (raining) return;
        switch (actualTime > 420)
        {
            case true:
                switch (actualTime > 840) 
                {
                    case true:
                        switch (actualTime > 1140)
                        {
                            case true:
                                time = TimeInfo.night;
                                break;
                            case false:
                                time = TimeInfo.evening;
                                break;
                        }
                        break;
                    case false:
                        time = TimeInfo.morning;
                        break;
                }
                break;
            case false:
                time = TimeInfo.night;
                break;
        }
    }
}
