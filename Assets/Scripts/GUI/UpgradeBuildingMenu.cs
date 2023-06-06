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
        Debug.Log(_building.UpgradeBuilding());
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
        if (building.buildingType == BuildingType.Hospital)
        {
            actualMultiplier.text = $"{building.levelMultipler[building.level - 1]}HP/sec";
            if (building.level + 1 > building.maxLevel)
            {
                nextMultiplier.text = "Maxed Out";
            }
            else
            {
                nextMultiplier.text = $"{building.levelMultipler[building.level]}HP/sec";
            }
        }

        if (building.buildingType == BuildingType.OreProcessing)
        {
            actualMultiplier.text = $"{building.levelMultipler[building.level - 1]} sec burn time";
            if (building.level + 1 > building.maxLevel)
            {
                nextMultiplier.text = "Maxed Out";
            }
            else
            {
                nextMultiplier.text = $"{building.levelMultipler[building.level]} sec burn time";
            }
        }

        if (building.buildingType == BuildingType.Beacon)
        {
            actualMultiplier.text = $"+{building.levelMultipler[building.level - 1]}%";
            if (building.level + 1 > building.maxLevel)
            {
                nextMultiplier.text = "Maxed Out";
            }
            else
            {
                nextMultiplier.text = $"+{building.levelMultipler[building.level]}%";
            }
        }
    }
}
