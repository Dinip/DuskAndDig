using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Building
{
    public BuildingType buildingType;
    public int level = 1;
    public int maxLevel = 4;
    public List<float> levelMultipler;
    public List<UpgradeBuildingLevel> levelCost;
    public InventoryObject input;
    public InventoryObject output;
    public Vector3 Position;
    public Quaternion Rotation;

    public float CurrentMultiplier
    {
        get
        {
            return levelMultipler[level - 1];
        }
    }
}

[Serializable]
public class UpgradeBuildingLevel
{
    public List<UpgradeBuildingItem> levelUpgrade;
}

[Serializable]
public class UpgradeBuildingItem
{
    public ItemObject item;
    public int amount;
}

public enum BuildingType
{
    OreProcessing,
    Hospital,
    Beacon,
    BlackSmith
}