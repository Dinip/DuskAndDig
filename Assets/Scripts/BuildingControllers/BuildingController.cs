using UnityEngine;

public abstract class BuildingController : MonoBehaviour
{
    public BuildingState buildingState;

    public abstract void UpgradeBuilding();
}
