using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Assets/ScriptableObjects/TimeControllerState", menuName = "GameState/TimeControllerState")]
public class TimeControllerObject : ScriptableObject
{
    public float cityBuilderTimeMultiplier = 144f; // 12h in 5 minutes

    public float actionTimeMultiplier = 72f; // 12h in 10 minutes

    public float cityBuilderStartTime;

    public float actionStartTime;

    public DateTime currentTime;

    public int currentDay = 1;

    [SerializeField]
    private float startHour;

    private void OnEnable()
    {
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(startHour);
    }
}
