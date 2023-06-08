using UnityEngine;

public class BuildingController : MonoBehaviour
{
    [SerializeField]
    private BuildingsSet buildingsSet;

    [SerializeField]
    private InventoryObject inventoryObject;

    [SerializeField]
    private EventBus eventBus;

    public Building building;

    private float _lastTime;

    public bool UpgradeBuilding()
    {
        if (!CanUpgrade()) return false;

        var invSlots = inventoryObject.GetSlots;

        var levelCost = building.levelCost[building.level];

        foreach (var upgradeItem in levelCost.levelUpgrade)
        {
            foreach (var invSlot in invSlots)
            {
                if (invSlot.item.Id == upgradeItem.item.data.Id)
                {
                    invSlot.UpdateSlot(invSlot.item, invSlot.amount - upgradeItem.amount);
                }
            }
        }

        building.level++;

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

        var levelCost = building.levelCost[building.level];

        foreach (var upgradeItem in levelCost.levelUpgrade)
        {
            foreach (var invSlot in invSlots)
            {
                if (invSlot.item.Id == upgradeItem.item.data.Id && invSlot.amount >= upgradeItem.amount)
                {
                    count++;
                }
            }
        }

        return count == levelCost.levelUpgrade.Count;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && building.buildingType == BuildingType.Hospital)
        {
            HealPlayer(other.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && building.buildingType == BuildingType.OreProcessing)
        {
            eventBus.onOreProcessingRange.Invoke(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && building.buildingType == BuildingType.OreProcessing)
        {
            eventBus.onOreProcessingRange.Invoke(false);
        }
    }

    private void HealPlayer(GameObject player)
    {
        var healthGained = building.levelMultipler[building.level - 1] * Time.deltaTime;
        var playerController = player.GetComponent<PlayerController3D>();
        playerController.Heal(healthGained);
    }

    private void FixedUpdate()
    {
        if (building.buildingType == BuildingType.OreProcessing)
        {
            if (Time.deltaTime + _lastTime > building.levelMultipler[building.level - 1])
            {
                TransferItemFromInputToOutput();
                _lastTime = 0;
                eventBus.oreProcessingProgress.Invoke(0f);
                return;
            }
            _lastTime += Time.deltaTime;
            eventBus.oreProcessingProgress.Invoke((_lastTime / building.levelMultipler[building.level - 1]) * 100);
        }
    }

    private void TransferItemFromInputToOutput()
    {
        var slots = building.input.GetSlots.Length;
        for (var i = 0; i < slots; i++)
        {
            var inputSlot = building.input.GetSlots[i];
            if (inputSlot.item.Id == -1) continue; // slot is empty

            //get output item (ingot) from input item (ore)
            var outputItem = new Item(building.input.database.ItemObjects[OreToIngotMap(inputSlot.item.Id)]);

            //check if output has space for this item
            var outputHasSpace = building.output.FindItemOnInventory(outputItem) != null || building.output.EmptySlotCount > 0;
            if (!outputHasSpace) continue;

            building.output.AddItem(outputItem, 1);
            if (inputSlot.amount == 1)
            {
                inputSlot.UpdateSlot(new Item(), 0);
                return;
            }
            inputSlot.UpdateSlot(inputSlot.item, inputSlot.amount - 1);
            return;
        }
    }

    private int OreToIngotMap(int ore)
    {
        if (ore == 3) return 1;
        if (ore == 4) return 2;
        return -1;
    }
}
