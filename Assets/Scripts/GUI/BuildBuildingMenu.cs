using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildBuildingMenu : MonoBehaviour
{
    [SerializeField]
    private EventBus eventBus;

    [SerializeField]
    private GameObject selectionMenu;

    [SerializeField]
    private GameObject buildingMenu;

    [SerializeField]
    private TextMeshProUGUI nextMultiplier;

    [SerializeField]
    private TextMeshProUGUI missingResources;

    [SerializeField]
    private GameObject[] upgradeUIs;

    [SerializeField]
    private GameObject[] atualIcons;

    [SerializeField]
    private ItemMappingSet itemMappingSet;

    [SerializeField]
    private GameObject[] buildings;

    [SerializeField]
    private BuildingsSet buildingsSet;

    private int _buildingIdx;

    public void BuildCancel()
    {
        _buildingIdx = -1;
        buildingMenu.SetActive(false);
    }

    public void Build()
    {
        if (_buildingIdx == -1) selectionMenu.SetActive(false);

        var b = buildings[_buildingIdx];
        var bc = b.GetComponent<BuildingController>();

        if (bc.building.buildingType != BuildingType.Beacon && buildingsSet.Items.FirstOrDefault(f => f.buildingType == bc.building.buildingType) != null)
        {
            missingResources.text = "Building Already Placed"; 
            missingResources.enabled = true;
            return;
        }

        if (!bc.CanUpgrade())
        {
            missingResources.text = "Not Enough Resources";
            missingResources.enabled = true;
            return;
        }

        buildingMenu.SetActive(false);
        eventBus.placeBuilding?.Invoke(_buildingIdx);
    }

    private void UpgradeCost(Building building)
    {
        if (building.level + 1 > building.maxLevel)
        {
            for (int i = 0; i < upgradeUIs.Length; i++)
            {
                upgradeUIs[i].SetActive(false);
            }
            return;
        }

        for (int i = 0; i < upgradeUIs.Length; i++)
        {
            var upgradeList = building.levelCost[building.level].levelUpgrade;
            if (i >= upgradeList.Count)
            {
                upgradeUIs[i].SetActive(false);
                continue;
            }
            var upgradeItem = upgradeList[i];
            var upgradeUI = upgradeUIs[i];
            upgradeUI.SetActive(true);
            var itemImage = upgradeUI.transform.Find("ItemImage").GetComponent<Image>();
            var itemAmount = upgradeUI.transform.Find("ItemAmount").GetComponent<TextMeshProUGUI>();
            itemImage.sprite = upgradeItem.item.uiDisplay;
            itemAmount.text = $"{upgradeItem.amount}";
        }
    }


    private void MultiplierTexts(Building building)
    {
        if (building.buildingType == BuildingType.BlackSmith)
        {
            nextMultiplier.enabled = false;

            //actualMultiplier.text = $"+{building.CurrentMultiplier}%";
            var currentItems = itemMappingSet.Items.FindAll(x => x.buildingType == BuildingType.BlackSmith && x.level <= building.level);
            var startCurrentPoint = currentItems.Count != atualIcons.Length ? (atualIcons.Length / 2) - (currentItems.Count / 2) : 0;
            for (int i = 0; i < currentItems.Count; i++)
            {
                atualIcons[startCurrentPoint + i].SetActive(true);
                atualIcons[startCurrentPoint + i].GetComponent<Image>().sprite = currentItems[i].to.uiDisplay;
            }
        }

        nextMultiplier.enabled = true;
        ToggleUpgradeIcons();
        nextMultiplier.text = $"{building.NextMultiplier}{BuildingText(building.buildingType)}";

    }

    private void ToggleUpgradeIcons(bool atual = false, bool next = false)
    {
        for (int i = 0; i < atualIcons.Length; i++)
        {
            atualIcons[i].SetActive(atual);
        }
    }

    private string BuildingText(BuildingType type)
    {
        return type switch
        {
            BuildingType.Hospital => "HP/sec",
            BuildingType.OreProcessing => " sec burn time",
            BuildingType.Beacon => "%",
            _ => "",
        };
    }

    public void SelectBuilding(int index)
    {
        var building = buildings[index];
        _buildingIdx = index;
        var bc = building.GetComponent<BuildingController>();
        MultiplierTexts(bc.building);
        UpgradeCost(bc.building);

        selectionMenu.SetActive(false);
        buildingMenu.SetActive(true);
    }
}
