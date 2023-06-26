using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeBuildingMenu : MonoBehaviour
{
    [SerializeField]
    private EventBus eventBus;

    [SerializeField]
    private GameObject menu;

    [SerializeField]
    private TextMeshProUGUI actualMultiplier;

    [SerializeField]
    private TextMeshProUGUI nextMultiplier;

    [SerializeField]
    private GameObject[] upgradeUIs;

    [SerializeField]
    private GameObject[] atualIcons;

    [SerializeField]
    private GameObject[] nextIcons;

    [SerializeField]
    private ItemMappingSet itemMappingSet;

    private BuildingController _building;

    private void OnEnable()
    {
        eventBus.selectedBuilding.AddListener(ShowMenu);
    }

    private void OnDisable()
    {
        eventBus.selectedBuilding.RemoveListener(ShowMenu);
    }

    private void ShowMenu(GameObject obj)
    {
        if (obj != null)
        {
            _building = obj.GetComponent<BuildingController>();
            MultiplierTexts(_building.building);
            UpgradeCost(_building.building);
        }
        else
        {
            _building = null;
        }

        menu.SetActive(obj != null);
    }

    public void UpgradeBuilding()
    {
        _building.UpgradeBuilding();
        eventBus.selectedBuilding.Invoke(null);
    }

    public void UpgradeCancel()
    {
        eventBus.selectedBuilding.Invoke(null);
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
            actualMultiplier.enabled = false;
            nextMultiplier.enabled = false;

            //actualMultiplier.text = $"+{building.CurrentMultiplier}%";
            var currentItems = itemMappingSet.Items.FindAll(x => x.buildingType == BuildingType.BlackSmith && x.level <= building.level);
            var startCurrentPoint = currentItems.Count != atualIcons.Length ? (atualIcons.Length / 2) - (currentItems.Count / 2) : 0;
            for (int i = 0; i < currentItems.Count; i++)
            {
                atualIcons[startCurrentPoint + i].SetActive(true);
                atualIcons[startCurrentPoint + i].GetComponent<Image>().sprite = currentItems[i].to.uiDisplay;
            }

            if (building.level + 1 > building.maxLevel)
            {
                nextMultiplier.enabled = true;
                nextMultiplier.text = "Maxed Out";
                ToggleUpgradeIcons(true, false);
            }
            else
            {
                var nextItems = itemMappingSet.Items.FindAll(x => x.buildingType == BuildingType.BlackSmith && x.level == building.level + 1);
                var startNextPoint = nextItems.Count != nextIcons.Length ? (nextIcons.Length / 2) - (nextItems.Count / 2) : 0;
                for (int i = 0; i < nextItems.Count; i++)
                {
                    nextIcons[startNextPoint + i].SetActive(true);
                    nextIcons[startNextPoint + i].GetComponent<Image>().sprite = nextItems[i].to.uiDisplay;
                }
            }
            return;
        }

        actualMultiplier.enabled = true;
        nextMultiplier.enabled = true;
        ToggleUpgradeIcons();

        actualMultiplier.text = Utils.BuildingText(building);
        if (building.level + 1 > building.maxLevel)
        {
            nextMultiplier.text = "Maxed Out";
        }
        else
        {
            nextMultiplier.text = Utils.BuildingText(building, true);
        }
    }

    private void ToggleUpgradeIcons(bool atual = false, bool next = false)
    {
        for (int i = 0; i < atualIcons.Length; i++)
        {
            atualIcons[i].SetActive(atual);
        }
        for (int i = 0; i < nextIcons.Length; i++)
        {
            nextIcons[i].SetActive(next);
        }
    }
}
