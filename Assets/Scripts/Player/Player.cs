using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private HealthObject health;

    [SerializeField]
    private InventoryObject inventory;

    [SerializeField]
    private InventoryObject equipment;

    public void TakeDamage(float damage)
    {
        var armor = equipment.GetSlots.FirstOrDefault(x => x.ItemObject.type == ItemType.Armor);
        if (armor != null)
        {
            var shieldItem = armor.ItemObject.data.buffs.FirstOrDefault(b => b.attribute == Attributes.Shield);
            float shieldValue = shieldItem.value / 100f;

            health.currentHealth -= damage * (1 - shieldValue);
            return;
        }
        health.currentHealth -= damage;
    }

    public void CollectItem(ItemObject item)
    {
        inventory.AddItem(item.CreateItem(), 1);
    }
}
