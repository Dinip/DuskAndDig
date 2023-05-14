using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Assets/ScriptableObjects/BuildingsState", menuName = "GameState/BuildingsState")]
public class BuildingsGameStateObject : ScriptableObject {
    public List<BuildingState> buildingsState;

    private void OnEnable()
    {
        if (buildingsState != null)
        {
            buildingsState.Clear();
        }
        else
        {
            buildingsState = new List<BuildingState>();
        }
    }

    public void SaveState()
    {
        buildingsState.Clear();
        var currentBuildings = GameObject.FindGameObjectsWithTag("Placeable");
        for (int i = 0; i < currentBuildings.Length; i++)
        {
            BuildingState bs = new BuildingState
            {
                Name = currentBuildings[i].name.Replace("(Clone)", ""),
                Position = currentBuildings[i].transform.position,
                Rotation = currentBuildings[i].transform.rotation
            };
            buildingsState.Add(bs);
        }
    }

    public void LoadState(GameObject[] buildings)
    {
        foreach (var building in buildingsState)
        {
            for (int i = 0; i < buildings.Length; i++)
            {
                if (building.Name == buildings[i].name)
                {
                    Instantiate(buildings[i], building.Position, building.Rotation);
                }
            }
        }
    }
}
