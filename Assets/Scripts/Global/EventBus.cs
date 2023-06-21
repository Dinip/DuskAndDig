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
    public UnityEvent<Building> onOreProcessingRange;

    [NonSerialized]
    public UnityEvent<float> oreProcessingProgress;

    [NonSerialized]
    public UnityEvent<ItemToItem> itemToCraft;

    [NonSerialized]
    public UnityEvent<Building> onBlackSmithRange;

    [NonSerialized]
    public UnityEvent<float> blackSmithProgress;

    private void OnEnable()
    {
        selectedBuilding ??= new UnityEvent<GameObject>();
        pendingBuilding ??= new UnityEvent<GameObject>();
        canPlaceBuilding ??= new UnityEvent<bool>();
        onOreProcessingRange ??= new UnityEvent<Building>();
        oreProcessingProgress ??= new UnityEvent<float>();
        itemToCraft ??= new UnityEvent<ItemToItem>();
        onBlackSmithRange ??= new UnityEvent<Building>();
        blackSmithProgress ??= new UnityEvent<float>();
    }
}
