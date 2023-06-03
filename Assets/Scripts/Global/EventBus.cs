using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Assets/Scripts/EventBus", menuName = "EventBus")]
public class EventBus : ScriptableObject
{
    [NonSerialized]
    public UnityEvent<GameObject> selectedBuilding;

    [NonSerialized]
    public UnityEvent<GameObject> pendingBuilding;

    [NonSerialized]
    public UnityEvent<bool> canPlaceBuilding;

    private void OnEnable()
    {
        selectedBuilding ??= new UnityEvent<GameObject>();
        pendingBuilding ??= new UnityEvent<GameObject>();
        canPlaceBuilding ??= new UnityEvent<bool>();
    }
}
