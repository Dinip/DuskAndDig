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

    private void OnEnable()
    {
        eventBus.onBlackSmithRange.AddListener(InRange);
        eventBus.blackSmithProgress.AddListener(BlackSmithProcessingProgress);
    }

    private void OnDisable()
    {
        eventBus.onBlackSmithRange.RemoveListener(InRange);
        eventBus.blackSmithProgress.RemoveListener(BlackSmithProcessingProgress);
    }

    private void InRange(Building building)
    {
        _building = building;
        openLabel.SetActive(building != null);
        openLabel.GetComponentInChildren<TextMeshProUGUI>().text = "Press F to open (Black Smith)";
        _inRange = building != null;

        if (!_inRange)
        {
            menu.SetActive(false);
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
        if (_inRange && Input.GetKeyDown(KeyCode.F))
        {
            menu.SetActive(!menu.activeSelf);
            openLabel.SetActive(!menu.activeSelf);
            if (menu.activeSelf) LoadSlots();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menu.SetActive(false);
            if (_inRange) openLabel.SetActive(true);
        }
    }

    private void LoadSlots()
    {
        ToggleSlots(false);
        var items = itemMappingSet.Items.FindAll(i => i.buildingType == BuildingType.BlackSmith && i.level <= _building.level);
        for (var i = 0; i < items.Count; i++)
        {
            slots[i].SetActive(true);
            slots[i].GetComponent<CraftItemUI>().Initialize(items[i]);
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
