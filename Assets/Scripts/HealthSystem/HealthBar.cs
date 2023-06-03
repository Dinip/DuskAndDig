using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    [SerializeField]
    private Gradient gradient;

    [SerializeField]
    private Image fill;

    [SerializeField]
    private HealthObject health;

    private void Start()
    {
        slider.maxValue = health.MaxHealth;
        slider.value = health.currentHealth;

        fill.color = gradient.Evaluate(1f);
    }

    private void Update()
    {
        slider.value = health.currentHealth;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}