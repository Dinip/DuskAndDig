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

    private void OnEnable()
    {
        eventBus.onOreProcessingRange.AddListener(InRange);
        eventBus.oreProcessingProgress.AddListener(OreProcessingProgress);
    }

    private void OnDisable()
    {
        eventBus.onOreProcessingRange.RemoveListener(InRange);
        eventBus.oreProcessingProgress.RemoveListener(OreProcessingProgress);
    }

    private void InRange(bool value)
    {
        openLabel.SetActive(value);
        _inRange = value;

        if (!_inRange)
        {
            menu.SetActive(false);
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
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menu.SetActive(false);
            if (_inRange) openLabel.SetActive(true);
        }
    }
}
