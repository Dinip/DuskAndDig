using UnityEngine;

[CreateAssetMenu(fileName = "ItemMappingSet", menuName = "GameData/ItemMappingSet")]
public class ItemMappingSet : RuntimeSet<ItemToItem>
{ }

[System.Serializable]
public class ItemToItem
{
    public ItemObject From;
    public int FromAmount;
    public ItemObject To;
    public int ToAmount;
    public BuildingType BuildingType;
}
