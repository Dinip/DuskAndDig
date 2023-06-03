using UnityEngine;

[CreateAssetMenu]
public class HealthObject : ScriptableObject
{
    [SerializeField]
    private float maxHealth;

    public float currentHealth;

    public float MaxHealth { get { return maxHealth; } }
}
