using UnityEngine;

[CreateAssetMenu(fileName = "ItemMappingSet", menuName = "GameData/ItemMappingSet")]
public class ItemMappingSet : RuntimeSet<ItemToItem>
{ }

[System.Serializable]
public class ItemToItem
{
    public ItemObject from;
    public int fromAmount;
    public ItemObject to;
    public int toAmount;
    public int level;
    public BuildingType buildingType;
}
