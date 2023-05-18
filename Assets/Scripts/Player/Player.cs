using UnityEngine;

public class Player : MonoBehaviour
{
    public InventoryObject inventory;
    public InventoryObject equipment;

    public Attribute[] attributes;

    private Transform armor;
    private Transform sword;

    public Transform weaponTransform;
    public Transform offhandWristTransform;
    public Transform offhandHandTransform;


    private void Start()
    {
        for (int i = 0; i < attributes.Length; i++)
        {
            attributes[i].SetParent(this);
        }

        for (int i = 0; i < equipment.GetSlots.Length; i++)
        {
            equipment.GetSlots[i].OnBeforeUpdate += OnRemoveItem;
            equipment.GetSlots[i].OnAfterUpdate += OnAddItem;
        }
    }


    public void OnRemoveItem(InventorySlot slot)
    {
        if (slot.ItemObject == null)
            return;
        switch (slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                print(string.Concat("Removed ", slot.ItemObject, " on ", slot.parent.inventory.type,
                    ", Allowed Items: ", string.Join(", ", slot.AllowedItems)));

                for (int i = 0; i < slot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].type == slot.item.buffs[i].attribute)
                            attributes[j].value.RemoveModifier(slot.item.buffs[i]);
                    }
                }

                if (slot.ItemObject.characterDisplay != null)
                {
                    switch (slot.AllowedItems[0])
                    {
                        case ItemType.Weapon:
                            Destroy(sword.gameObject);
                            break;
                        case ItemType.Armor:
                            Destroy(armor.gameObject);
                            break;
                    }
                }
                break;
            case InterfaceType.Chest:
                break;
            default:
                break;
        }
    }

    public void OnAddItem(InventorySlot slot)
    {
        if (slot.ItemObject == null)
            return;
        switch (slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                print(
                    $"Placed {slot.ItemObject}  on {slot.parent.inventory.type}, Allowed Items: {string.Join(", ", slot.AllowedItems)}");

                for (int i = 0; i < slot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].type == slot.item.buffs[i].attribute)
                            attributes[j].value.AddModifier(slot.item.buffs[i]);
                    }
                }

                if (slot.ItemObject.characterDisplay != null)
                {
                    switch (slot.AllowedItems[0])
                    {
                        case ItemType.Weapon:
                            sword = Instantiate(slot.ItemObject.characterDisplay, weaponTransform).transform;
                            break;
                        case ItemType.Armor:
                            armor = Instantiate(slot.ItemObject.characterDisplay, armor).transform;
                            break;
                    }
                }
                break;
            case InterfaceType.Chest:
                break;
            default:
                break;
        }
    }


    public void OnTriggerEnter(Collider other)
    {
        var groundItem = other.GetComponent<GroundItem>();
        if (groundItem)
        {
            Item _item = new(groundItem.item);
            if (inventory.AddItem(_item, 1))
            {
                Destroy(other.gameObject);
            }
        }
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        inventory.Save();
    //        equipment.Save();
    //    }

    //    if (Input.GetKeyDown(KeyCode.KeypadEnter))
    //    {
    //        inventory.Load();
    //        equipment.Load();
    //    }
    //}

    public void AttributeModified(Attribute attribute)
    {
        Debug.Log(string.Concat(attribute.type, " was updated! Value is now ", attribute.value.ModifiedValue));
    }


    private void OnApplicationQuit()
    {
        inventory.Clear();
        equipment.Clear();
    }
}

[System.Serializable]
public class Attribute
{
    [System.NonSerialized]
    public Player parent;

    public Attributes type;

    public ModifiableInt value;

    public void SetParent(Player parent)
    {
        this.parent = parent;
        value = new ModifiableInt(AttributeModified);
    }

    public void AttributeModified()
    {
        parent.AttributeModified(this);
    }
}