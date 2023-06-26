using System;
using System.Collections.Generic;
using UnityEngine;
using static Utils;

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

    public float NextMultiplier
    {
        get
        {
            return levelMultipler[level];
        }
    }
}

[Serializable]
public class UpgradeBuildingLevel
{
    public List<ItemObjectAmount> levelUpgrade;
}

public enum BuildingType
{
    OreProcessing,
    Hospital,
    Beacon,
    BlackSmith
}