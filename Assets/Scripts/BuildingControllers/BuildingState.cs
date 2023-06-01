using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingState", menuName = "ScriptableObjects/BuildingState", order = 1)]
public abstract class BuildingState : ScriptableObject
{
    public BuildingType buildingType;
    public int level = 1;
    public int maxLevel = 4;
    public float multiplier;
    public List<string> input;
    public List<string> output;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

public enum BuildingType
{
    OreProcessing,
    Hospital,
    Beacon,
    BlackSmith
}
