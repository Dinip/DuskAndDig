using UnityEngine;

[CreateAssetMenu(fileName = "PlayerHealth", menuName = "PlayerOrEnemy/PlayerHealth")]
public class HealthObject : ScriptableObject
{
    [SerializeField]
    private float maxHealth;

    public float currentHealth;

    public float MaxHealth { get { return maxHealth; } }
}
