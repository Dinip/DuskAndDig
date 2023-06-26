using UnityEngine;
using UnityEngine.UI;

public class HealthEnemy : MonoBehaviour
{
    [SerializeField]
    public GameObject objectToFollow;

    [SerializeField]
    private RectTransform healthBar;

    [SerializeField]
    private Slider slider;

    [SerializeField]
    private Image fill;

    [SerializeField]
    private Gradient gradient;

    private Enemy _enemy;

    private void Awake()
    {
        healthBar = GetComponent<RectTransform>();
        _enemy = objectToFollow.GetComponent<Enemy>();
    }

    private void Start()
    {
        slider.maxValue = _enemy.MaxHealth;
        slider.value = _enemy.Health;
        fill.color = gradient.Evaluate(1f);
    }

    private void Update()
    {
        slider.value = _enemy.Health;
        fill.color = gradient.Evaluate(slider.normalizedValue);

        if (objectToFollow != null)
        {
            healthBar.anchoredPosition = objectToFollow.transform.localPosition;
        }
    }
}
