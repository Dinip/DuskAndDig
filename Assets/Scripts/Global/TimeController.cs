using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeController : MonoBehaviour
{
    private TimeSpan _sunriseTime;

    private TimeSpan _sunsetTime;

    private string _currentScene;

    private float _timeMultiplier = 1f;

    [SerializeField]
    private TextMeshProUGUI timeText;

    [SerializeField]
    private TextMeshProUGUI dayText;

    [SerializeField]
    private Light sunLight;

    [SerializeField]
    private float sunriseHour;

    [SerializeField]
    private float sunsetHour;

    [SerializeField]
    private TimeControllerObject timeControllerObject;

    void Start()
    {
        _sunriseTime = TimeSpan.FromHours(sunriseHour);
        _sunsetTime = TimeSpan.FromHours(sunsetHour);
        _currentScene = SceneManager.GetActiveScene().name;

        if (_currentScene == "CityBuilder")
        {
            _timeMultiplier = timeControllerObject.cityBuilderTimeMultiplier;
        }
        else
        {
            _timeMultiplier = timeControllerObject.actionTimeMultiplier;
        }
        UpdateDay();
    }

    void Update()
    {
        UpdateTime();
        RotateSun();
        ChangeGameMode();
    }

    private void ChangeGameMode()
    {
        if (timeControllerObject.currentTime.Hour >= timeControllerObject.actionStartTime || timeControllerObject.currentTime.Hour < timeControllerObject.cityBuilderStartTime)
        {
            //SceneManager.LoadScene("Action2D");
            //SceneManager.LoadScene("CityBuilder2");
            if (_currentScene == "CityBuilder") SceneManager.LoadScene("CityBuilder2");
        }
        else
        {
            if (_currentScene == "CityBuilder2") SceneManager.LoadScene("CityBuilder");
        }
    }

    private void UpdateTime()
    {
        var oldTime = timeControllerObject.currentTime;
        timeControllerObject.currentTime = timeControllerObject.currentTime.AddSeconds(Time.deltaTime * _timeMultiplier);

        if (oldTime.Hour == 23 && timeControllerObject.currentTime.Hour == 0)
        {
            timeControllerObject.currentDay++;
            UpdateDay();
        }


        if (timeText != null)
        {
            timeText.text = timeControllerObject.currentTime.ToString("HH:mm");
        }
    }

    private void UpdateDay()
    {
        if (dayText != null)
        {
            dayText.text = $"Day {timeControllerObject.currentDay}";
        }
    }

    private TimeSpan CalculateTimeDiff(TimeSpan fromTime, TimeSpan toTime)
    {
        var diff = fromTime - toTime;

        if (diff.TotalSeconds < 0)
        {
            diff += TimeSpan.FromHours(24);
        }
        return diff;
    }

    private void RotateSun()
    {
        if (sunLight == null) return;

        float sunLightRotation;

        if (timeControllerObject.currentTime.TimeOfDay > _sunriseTime && timeControllerObject.currentTime.TimeOfDay < _sunsetTime)
        {
            var sunriseToSensetDuration = CalculateTimeDiff(_sunsetTime, _sunriseTime);
            var timeSinceSunrise = CalculateTimeDiff(_sunriseTime, timeControllerObject.currentTime.TimeOfDay);

            var percentage = timeSinceSunrise.TotalMinutes / sunriseToSensetDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(0, 180, (float)percentage);
        }
        else
        {
            var sunsetToSunriseDuration = CalculateTimeDiff(_sunsetTime, _sunriseTime);
            var timeSinceSunset = CalculateTimeDiff(_sunsetTime, timeControllerObject.currentTime.TimeOfDay);

            var percentage = timeSinceSunset.TotalMinutes / sunsetToSunriseDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(180, 360, (float)percentage);
        }

        sunLight.transform.rotation = Quaternion.AngleAxis(sunLightRotation, Vector3.right);
    }
}