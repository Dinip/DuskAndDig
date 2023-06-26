using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlackSmithUI : MonoBehaviour
{
    [SerializeField]
    private GameObject[] slots;

    [SerializeField]
    private EventBus eventBus;

    [SerializeField]
    private ItemMappingSet itemMappingSet;

    [SerializeField]
    private GameObject menu;

    [SerializeField]
    private GameObject openLabel;

    [SerializeField]
    private Slider slider;

    [SerializeField]
    private Gradient gradient;

    [SerializeField]
    private Image fill;

    private bool _inRange;

    private Building _building;

    private bool _showLabel = true;

    private ItemToItem _currentCraft;

    private bool _paused;

    private void OnEnable()
    {
        eventBus.onBlackSmithRange.AddListener(InRange);
        eventBus.blackSmithProgress.AddListener(BlackSmithProcessingProgress);
        eventBus.selectedBuilding.AddListener(ShowLabel);
        eventBus.itemToCraft.AddListener(SelectedCraft);
        eventBus.gamePaused.AddListener(HandlePause);
    }

    private void OnDisable()
    {
        eventBus.onBlackSmithRange.RemoveListener(InRange);
        eventBus.blackSmithProgress.RemoveListener(BlackSmithProcessingProgress);
        eventBus.selectedBuilding.RemoveListener(ShowLabel);
        eventBus.itemToCraft.RemoveListener(SelectedCraft);
        eventBus.gamePaused.RemoveListener(HandlePause);
    }

    private void SelectedCraft(ItemToItem item)
    {
        _currentCraft = item;
    }

    private void ShowLabel(GameObject obj)
    {
        _showLabel = obj == null;
    }

    private void HandlePause(bool paused)
    {
        _paused = paused;
        if (paused)
        {
            menu.SetActive(false);
            openLabel.SetActive(false);
        }
        else
        {
            if (_inRange && _showLabel)
            {
                openLabel.GetComponentInChildren<TextMeshProUGUI>().text = "Press F to open (Workshop)";
            }
        }
    }

    private void InRange(Building building)
    {
        _building = building;
        openLabel.SetActive(building != null);
        if (_showLabel) openLabel.GetComponentInChildren<TextMeshProUGUI>().text = "Press F to open (Workshop)";
        _inRange = building != null;

        if (!_inRange)
        {
            menu.SetActive(false);
            eventBus.buildingUIOpened?.Invoke(false);
        }
    }

    private void BlackSmithProcessingProgress(float value)
    {
        slider.value = value;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    private void Start()
    {
        slider.maxValue = 100;
        slider.value = 0;

        fill.color = gradient.Evaluate(1f);
    }

    private void Update()
    {
        if (_inRange && !_paused && Input.GetKeyDown(KeyCode.F))
        {
            menu.SetActive(!menu.activeSelf);
            openLabel.SetActive(!menu.activeSelf);
            eventBus.buildingUIOpened?.Invoke(menu.activeSelf);
            if (menu.activeSelf) LoadSlots();
        }
    }

    private void LoadSlots()
    {
        ToggleSlots(false);
        var items = itemMappingSet.Items.FindAll(i => i.buildingType == BuildingType.BlackSmith && i.level <= _building.level);
        for (var i = 0; i < items.Count; i++)
        {
            slots[i].SetActive(true);
            slots[i].GetComponent<CraftItemUI>().Initialize(items[i], _currentCraft);
        }
    }

    private void ToggleSlots(bool value)
    {
        foreach (var slot in slots)
        {
            slot.SetActive(value);
        }
    }
}
