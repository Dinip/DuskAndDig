using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeController : MonoBehaviour {

    private TimeSpan _sunriseTime;

    private TimeSpan _sunsetTime;

    [SerializeField]
    private float timeMultiplier = 1f;

    [SerializeField]
    private TextMeshProUGUI timeText;

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
    }

    void Update()
    {
        UpdateTime();
        RotateSun();
        ChangeGameMode();
    }

    private void ChangeGameMode()
    {
        if (timeControllerObject.currentTime.Hour >= timeControllerObject.startActionTime || timeControllerObject.currentTime.Hour < timeControllerObject.startCityBuilderTime)
        {
            //SceneManager.LoadScene("Action2D");
            //SceneManager.LoadScene("CityBuilder2");
            if (SceneManager.GetActiveScene().name == "CityBuilder") SceneManager.LoadScene("CityBuilder2");
        }
        else
        {
            if (SceneManager.GetActiveScene().name == "CityBuilder2") SceneManager.LoadScene("CityBuilder");
        }
    }

    private void UpdateTime()
    {
        var oldTime = timeControllerObject.currentTime;
        timeControllerObject.currentTime = timeControllerObject.currentTime.AddSeconds(Time.deltaTime * timeMultiplier);

        if (oldTime.Hour == 23 && timeControllerObject.currentTime.Hour == 0)
        {
            timeControllerObject.currentDay++;
            //TODO
            //display day
        }

        if (timeText != null)
        {
            timeText.text = timeControllerObject.currentTime.ToString("HH:mm");
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