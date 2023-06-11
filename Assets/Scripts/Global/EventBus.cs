using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "EventBus", menuName = "GameData/EventBus")]
public class EventBus : ScriptableObject
{
    [NonSerialized]
    public UnityEvent<GameObject> selectedBuilding;

    [NonSerialized]
    public UnityEvent<GameObject> pendingBuilding;

    [NonSerialized]
    public UnityEvent<bool> canPlaceBuilding;

    [NonSerialized]
    public UnityEvent<bool> onOreProcessingRange;

    [NonSerialized]
    public UnityEvent<float> oreProcessingProgress;

    private void OnEnable()
    {
        selectedBuilding ??= new UnityEvent<GameObject>();
        pendingBuilding ??= new UnityEvent<GameObject>();
        canPlaceBuilding ??= new UnityEvent<bool>();
        onOreProcessingRange ??= new UnityEvent<bool>();
        oreProcessingProgress ??= new UnityEvent<float>();
    }
}
