using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Assets/ScriptableObjects/BuildingsState", menuName = "GameState/BuildingsState")]
public class BuildingsGameStateObject : ScriptableObject
{
    public List<BuildingStateModel> buildingsState;

    private void OnEnable()
    {
        if (buildingsState != null)
        {
            buildingsState.Clear();
        }
        else
        {
            buildingsState = new List<BuildingStateModel>();
        }
    }

    public void SaveState()
    {
        buildingsState.Clear();
        var currentBuildings = GameObject.FindGameObjectsWithTag("Placeable");
        for (int i = 0; i < currentBuildings.Length; i++)
        {
            BuildingStateModel bs = new BuildingStateModel
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
                    var a = Instantiate(buildings[i], building.Position, building.Rotation);
                    var comp = a.AddComponent<OreProcessingController>();
                    //comp.buildingState = ScriptableObject.FindObjectsByType<BuildingStateModel>();
                    //var c = ScriptableObject.CreateInstance<BuildingStateModel>();
                    //for(int i = 0; i < c.Length; i++)
                    //{
                    //    if (c[i].Name == building.Name)
                    //    {
                    //        comp.buildingState = c[i];
                    //    }
                    //}
                }
            }
        }
    }
}
