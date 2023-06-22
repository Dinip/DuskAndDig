using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OreProcessingUI : MonoBehaviour
{
    [SerializeField]
    private EventBus eventBus;

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

    private bool _showLabel = true;

    private void OnEnable()
    {
        eventBus.onOreProcessingRange.AddListener(InRange);
        eventBus.oreProcessingProgress.AddListener(OreProcessingProgress);
        eventBus.selectedBuilding.AddListener(ShowLabel);
    }

    private void OnDisable()
    {
        eventBus.onOreProcessingRange.RemoveListener(InRange);
        eventBus.oreProcessingProgress.RemoveListener(OreProcessingProgress);
        eventBus.selectedBuilding.RemoveListener(ShowLabel);
    }

    private void ShowLabel(GameObject obj)
    {
        _showLabel = obj == null;
    }

    private void InRange(Building building)
    {
        openLabel.SetActive(building != null);
        if (_showLabel) openLabel.GetComponentInChildren<TextMeshProUGUI>().text = "Press F to open (Ore Processing)";
        _inRange = building != null;

        if (!_inRange)
        {
            menu.SetActive(false);
            eventBus.buildingUIOpened?.Invoke(false);
        }
    }

    private void OreProcessingProgress(float value)
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
            eventBus.buildingUIOpened?.Invoke(menu.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menu.SetActive(false);
            eventBus.buildingUIOpened?.Invoke(false);
            if (_inRange) openLabel.SetActive(true);
        }
    }
}
