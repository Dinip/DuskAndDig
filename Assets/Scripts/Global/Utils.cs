using System;
using System.Linq;

public class Utils
{
    public static float ComputePlayerDamage(InventoryObject equipment, BuildingsSet buildings)
    {
        var swordSlot = equipment.GetSlots.FirstOrDefault(f => f.ItemObject?.type == ItemType.Weapon);
        if (swordSlot == null) return 0f;

        var beacons = buildings.Items.FindAll(b => b.buildingType == BuildingType.Beacon);
        float beaconMultiplier = beacons.Sum(b => b.CurrentMultiplier);

        var sword = swordSlot.ItemObject.data.buffs.First(f => f.attribute == Attributes.Damage);

        var damage = sword.value + sword.value * (beaconMultiplier / 100);
        return damage;
    }

    public static float ComputePlayerShield(InventoryObject equipment)
    {
        var armorSlot = equipment.GetSlots.FirstOrDefault(x => x.ItemObject?.type == ItemType.Armor);
        if (armorSlot == null) return 0f;

        var armor = armorSlot.ItemObject.data.buffs.First(f => f.attribute == Attributes.Shield);
        return armor.value;
    }

    [Serializable]
    public class ItemObjectAmount
    {
        public ItemObject item;
        public int amount;
    }

    public static string BuildingText(Building building, bool next = false)
    {
        var value = next ? building.NextMultiplier : building.CurrentMultiplier;
        return building.buildingType switch
        {
            BuildingType.Hospital => $"{value}HP/sec",
            BuildingType.OreProcessing => $"{value} sec burn time",
            BuildingType.Beacon => $"+{value}% attack dmg",
            _ => "",
        };
    }
}