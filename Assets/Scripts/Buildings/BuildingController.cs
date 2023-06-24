using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class BuildingController : MonoBehaviour
{
    [SerializeField]
    private BuildingsSet buildingsSet;

    [SerializeField]
    private InventoryObject inventoryObject;

    [SerializeField]
    private EventBus eventBus;

    [SerializeField]
    private ItemMappingSet itemMappingSet;

    public Building building;

    [HideInInspector]
    public bool placed = false;

    private float _lastTime;

    private ItemToItem _itemToCraft;

    private void OnEnable()
    {
        eventBus.itemToCraft.AddListener(SelectedCraft);
    }

    private void OnDisable()
    {
        eventBus.itemToCraft.RemoveListener(SelectedCraft);
    }

    private void SelectedCraft(ItemToItem item)
    {
        if (building.buildingType == BuildingType.BlackSmith) _itemToCraft = item;
        if (item == null) ResetProgress(eventBus.blackSmithProgress);
    }

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
                    int newAmount = invSlot.amount - upgradeItem.amount;
                    invSlot.UpdateSlot(newAmount == 0 ? new Item() : invSlot.item, newAmount);
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
        if (!placed || !other.CompareTag("Player") || building.buildingType != BuildingType.Hospital) return;
        HealPlayer(other.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!placed || !other.CompareTag("Player")) return;

        if (building.buildingType == BuildingType.OreProcessing)
        {
            eventBus.onOreProcessingRange.Invoke(building);
            return;
        }

        if (building.buildingType == BuildingType.BlackSmith)
        {
            eventBus.onBlackSmithRange.Invoke(building);
            return;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!placed || !other.CompareTag("Player")) return;

        if (building.buildingType == BuildingType.OreProcessing)
        {
            eventBus.onOreProcessingRange.Invoke(null);
            return;
        }

        if (building.buildingType == BuildingType.BlackSmith)
        {
            eventBus.onBlackSmithRange.Invoke(null);
            return;
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
        OreProcessing();
        ItemCrafting();
    }

    private void OreProcessing()
    {
        if (building.buildingType != BuildingType.OreProcessing) return;

        //reset if input is/becomes empty
        if (building.input.EmptySlotCount == building.input.GetSlots.Length)
        {
            ResetProgress(eventBus.oreProcessingProgress);

        }

        if (Time.deltaTime + _lastTime > building.CurrentMultiplier)
        {
            SmeltOreToIngot();
            ResetProgress(eventBus.oreProcessingProgress);
        }

        _lastTime += Time.deltaTime;
        eventBus.oreProcessingProgress.Invoke((_lastTime / building.CurrentMultiplier) * 100);
    }

    private void ItemCrafting()
    {
        if (building.buildingType != BuildingType.BlackSmith || _itemToCraft == null) return;

        //reset if input is/becomes empty
        if (building.input.EmptySlotCount == building.input.GetSlots.Length)
        {
            ResetProgress(eventBus.blackSmithProgress);
        }

        //doesnt have the required input item amount or output is full
        var hasInputItems = building.input.GetSlots.Where(s => s.item.Id == _itemToCraft.from.data.Id).Sum(s => s.amount) >= _itemToCraft.fromAmount;
        var outputFull = building.output.EmptySlotCount == 0;

        if (!hasInputItems || outputFull)
        {
            ResetProgress(eventBus.blackSmithProgress);
        }

        if (Time.deltaTime + _lastTime > building.CurrentMultiplier)
        {
            CraftItemToResult();
            ResetProgress(eventBus.blackSmithProgress);
            eventBus.itemToCraft?.Invoke(null);
        }

        _lastTime += Time.deltaTime;
        eventBus.blackSmithProgress.Invoke((_lastTime / building.CurrentMultiplier) * 100);
    }

    private void ResetProgress(UnityEvent<float> evt)
    {
        _lastTime = 0;
        evt?.Invoke(0f);
        return;
    }

    private void SmeltOreToIngot()
    {
        var slots = building.input.GetSlots.Length;
        for (var i = 0; i < slots; i++)
        {
            var inputSlot = building.input.GetSlots[i];
            if (inputSlot.item.Id == -1) continue; // slot is empty

            //get output item (ingot) from input item (ore)
            var outputItem = itemMappingSet.Items.Find(i => i.buildingType == building.buildingType && i.from.data.Id == inputSlot.item.Id).to.CreateItem();

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

    private void CraftItemToResult()
    {
        var inputSlots = building.input.GetSlots.Where(s => s.item.Id == _itemToCraft.from.data.Id).OrderBy(o => o.amount).ToList();

        //remove from input slots the amount of items required to craft (fromAmount)
        var amountToRemove = _itemToCraft.fromAmount;
        foreach (var slot in inputSlots)
        {
            if (slot.amount >= amountToRemove)
            {
                slot.UpdateSlot(slot.item, slot.amount - amountToRemove);
                amountToRemove = 0;
                break;
            }
            amountToRemove -= slot.amount;
            slot.UpdateSlot(new Item(), 0);
        }

        //add result to output
        building.output.AddItem(_itemToCraft.to.CreateItem(), _itemToCraft.toAmount);
    }
}
