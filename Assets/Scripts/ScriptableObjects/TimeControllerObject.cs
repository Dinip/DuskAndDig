using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Assets/ScriptableObjects/TimeControllerState", menuName = "GameState/TimeControllerState")]
public class TimeControllerObject : ScriptableObject {

    public float startCityBuilderTime;

    public float startActionTime;

    public DateTime currentTime;

    public int currentDay = 1;

    [SerializeField]
    private float startHour;

    private void OnEnable()
    {
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(startHour);
    }
}
