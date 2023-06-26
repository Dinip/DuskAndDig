using System;
using System.Linq;
using UnityEngine;
using static Utils;

[CreateAssetMenu(fileName = "GameManager", menuName = "GameData/GameManagerObject", order = 1)]
public class GameManagerObject : ScriptableObject
{
    [SerializeField]
    private EventBus eventBus;

    [SerializeField]
    private InventoryObject[] inventories;

    [SerializeField]
    private ItemObjectAmount[] initialEquipment;

    [SerializeField]
    private TimeControllerObject timeControllerObj;

    [SerializeField]
    private BuildingsSet buildings;

    private bool _isPaused = false;

    public bool IsPaused => _isPaused;

    public void SetPause(bool paused)
    {
        eventBus.gamePaused.Invoke(paused);
        _isPaused = paused;
        Time.timeScale = paused ? 0f : 1f;
    }

    public void ResetGame()
    {
        foreach (var inventory in inventories)
        {
            inventory.Clear();
            inventory.Save();
            if (inventory.type == InterfaceType.Equipment)
            {
                PopulateDefaultEquipment(inventory);
            }
        }
        buildings.Clear();
        timeControllerObj.currentDay = 1;
        timeControllerObj.currentTime = DateTime.Now.Date;
    }

    private void PopulateDefaultEquipment(InventoryObject inv)
    {
        foreach (var itemAmount in initialEquipment)
        {
            foreach (var slot in inv.GetSlots)
            {
                if (slot.AllowedItems.Contains(itemAmount.item.type))
                {
                    slot.UpdateSlot(itemAmount.item.CreateItem(), itemAmount.amount);
                    break;
                }
            }
        }
    }
}