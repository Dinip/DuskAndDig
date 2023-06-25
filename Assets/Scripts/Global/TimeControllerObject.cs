using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TimeObject", menuName = "GameData/TimeObject")]
public class TimeControllerObject : ScriptableObject
{
    [SerializeField]
    private int cityBuilderTimeRuntime = 3; // 12h in 3 minutes

    [SerializeField]
    private int actionTimeRuntime = 5; // 12h in 5 minutes

    [SerializeField]
    private float cityBuilderStartTime;

    [SerializeField]
    private float actionStartTime;

    [SerializeField]
    private float sunriseHour;

    [SerializeField]
    private float sunsetHour;

    public DateTime currentTime;

    public int currentDay = 1;

    [SerializeField]
    private float startHour;

    public float CityBuilderTimeMultiplier
    {
        get
        {
            return 1f / (cityBuilderTimeRuntime / (60f * 24f));
        }
    }

    public float ActionTimeMultiplier
    {
        get
        {
            return 1f / (actionTimeRuntime / (60f * 24f));
        }
    }

    public float CityBuilderStartTime
    {
        get
        {
            return cityBuilderStartTime;
        }
    }

    public float ActionStartTime
    {
        get
        {
            return actionStartTime;
        }
    }

    public float StartHour
    {
        get
        {
            return startHour;
        }
    }

    public float SunriseHour
    {
        get
        {
            return sunriseHour;
        }
    }

    public float SunsetHour
    {
        get
        {
            return sunsetHour;
        }
    }

    private void OnEnable()
    {
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(startHour);
    }
}
