using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Weapon,
    Armor,
    Ore,
    Ingot
}

public enum Attributes
{
    Damage,
    Health,
    Shield
}

[CreateAssetMenu(fileName = "New Item", menuName = "InventorySystem/Items/item")]
public class ItemObject : ScriptableObject
{
    public Sprite uiDisplay;

    public GameObject characterDisplay;

    public bool stackable;

    public ItemType type;

    public Item data = new();

    public Item CreateItem()
    {
        Item newItem = new(this);
        return newItem;
    }
}

[System.Serializable]
public class Item
{
    public string Name;

    public int Id = -1;

    public ItemBuff[] buffs;

    public Item()
    {
        Name = "";
        Id = -1;
    }

    public Item(ItemObject item)
    {
        Name = item.name;
        Id = item.data.Id;
        buffs = new ItemBuff[item.data.buffs.Length];
        for (int i = 0; i < buffs.Length; i++)
        {
            buffs[i] = new ItemBuff(item.data.buffs[i].min, item.data.buffs[i].max)
            {
                attribute = item.data.buffs[i].attribute
            };
        }
    }
}

[System.Serializable]
public class ItemBuff : IModifier
{
    public Attributes attribute;

    public int value;

    public int min;

    public int max;

    public ItemBuff(int min, int max)
    {
        this.min = min;
        this.max = max;
        GenerateValue();
    }

    public void AddValue(ref int baseValue)
    {
        baseValue += value;
    }

    public void GenerateValue()
    {
        value = UnityEngine.Random.Range(min, max);
    }
}