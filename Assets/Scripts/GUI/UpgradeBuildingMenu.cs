using Unity.VisualScripting;
using UnityEngine;

public class UpgradeBuildingMenu : MonoBehaviour
{
    [SerializeField]
    private EventBus buildingObject;

    [SerializeField]
    private GameObject menu;

    private GameObject _building;

    private void OnEnable()
    {
        buildingObject.selectedBuilding.AddListener(ShowMenu);
    }

    private void OnDisable()
    {
        buildingObject.selectedBuilding.RemoveListener(ShowMenu);
    }

    private void ShowMenu(GameObject obj)
    {
        _building = obj;
        menu.SetActive(obj != null);
    }

    public void UpgradeBuilding()
    {
        var buildingController = _building.GetComponent<BuildingController>();
        Debug.Log(buildingController.CanUpgrade());
    }

    public void UpgradeCancel()
    {
        buildingObject.selectedBuilding.Invoke(null);
    }
}
