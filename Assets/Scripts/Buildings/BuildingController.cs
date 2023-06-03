using UnityEngine;

public class BuildingController : MonoBehaviour
{
    [SerializeField]
    private BuildingsSet buildingsSet;

    [SerializeField]
    private InventoryObject inventoryObject;

    public Building building;

    public bool UpgradeBuilding()
    {
        if (!CanUpgrade()) return false;

        building.level++;

        var invSlots = inventoryObject.GetSlots;

        var levelCost = building.levelCost[building.level - 1];

        foreach (var upgradeItem in levelCost.levelUpgrade)
        {
            foreach (var invSlot in invSlots)
            {
                if (invSlot.ItemObject == upgradeItem.item)
                {
                    invSlot.AddAmount(upgradeItem.amount);
                }
            }
        }

        return true;
    }

    public bool CanUpgrade()
    {
        if (building.level >= building.maxLevel)
        {
            return false;
        }

        var invSlots = inventoryObject.GetSlots;

        int count = 0;

        var levelCost = building.levelCost[building.level - 1];

        foreach (var upgradeItem in levelCost.levelUpgrade)
        {
            foreach (var invSlot in invSlots)
            {
                if (invSlot.ItemObject == upgradeItem.item && invSlot.amount >= upgradeItem.amount)
                {
                    count++;
                }
            }
        }

        return count == building.levelCost.Count;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && building.buildingType == BuildingType.Hospital)
        {
            HealPlayer(other.gameObject);
        }
    }

    private void HealPlayer(GameObject player)
    {
        var healthGained = building.levelMultipler[building.level - 1] * Time.deltaTime;
        var playerController = player.GetComponent<PlayerController>();
        playerController.Heal(healthGained);
    }
}
