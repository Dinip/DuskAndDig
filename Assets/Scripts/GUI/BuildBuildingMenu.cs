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
    private GameObject[] nextIcons;

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
        selectionMenu.SetActive(true);
    }

    public void Build()
    {
        if (_buildingIdx == -1) selectionMenu.SetActive(false);

        var b = buildings[_buildingIdx];
        var bc = b.GetComponent<BuildingController>();

        if ((bc.building.buildingType == BuildingType.Hospital || bc.building.buildingType == BuildingType.BlackSmith) &&
            buildingsSet.Items.FirstOrDefault(f => f.buildingType == bc.building.buildingType) != null)
        {
            missingResources.text = "Only 1 Building Of Type Allowed";
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

            var nextItems = itemMappingSet.Items.FindAll(x => x.buildingType == BuildingType.BlackSmith && x.level == building.level + 1);
            var startNextPoint = nextItems.Count != nextIcons.Length ? (nextIcons.Length / 2) - (nextItems.Count / 2) : 0;
            for (int i = 0; i < nextItems.Count; i++)
            {
                nextIcons[startNextPoint + i].SetActive(true);
                nextIcons[startNextPoint + i].GetComponent<Image>().sprite = nextItems[i].to.uiDisplay;
            }
            return;
        }

        nextMultiplier.enabled = true;
        ToggleUpgradeIcons();
        nextMultiplier.text = Utils.BuildingText(building, true);

    }

    private void ToggleUpgradeIcons(bool atual = false, bool next = false)
    {
        for (int i = 0; i < nextIcons.Length; i++)
        {
            nextIcons[i].SetActive(atual);
        }
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            selectionMenu.SetActive(!selectionMenu.activeSelf);
            eventBus.buildingUIOpened?.Invoke(selectionMenu.activeSelf);
        }
    }
}
