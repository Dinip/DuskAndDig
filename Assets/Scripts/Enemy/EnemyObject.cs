using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "PlayerOrEnemy/EnemyObject")]
public class EnemyObject : ScriptableObject
{
    [SerializeField]
    private TimeControllerObject timeObject;

    [SerializeField]
    private float baseHealth = 100f;

    [SerializeField]
    private float baseDamage = 10f;

    [SerializeField]
    private float healthMultiplier = 1.2f;

    [SerializeField]
    private float damageMultiplier = 1.1f;

    [SerializeField]
    private float attackCooldown = 2f;

    public float Health { get; private set; }
    public float Damage { get; private set; }
    public float AttackCooldown => attackCooldown;

    private void OnEnable()
    {
        Debug.Log("EnemyObject.OnEnable");
        ComputeValues();
        Debug.Log($"Health {Health} - Dmg {Damage}");
    }

    private void ComputeValues()
    {
        int day = timeObject.currentDay > 7 ? 7 : timeObject.currentDay;

        Health = baseHealth + (day - 1) * healthMultiplier * 30;
        Damage = baseDamage + (day - 1) * damageMultiplier * 10;
    }
}

//b   100 20
//x   1.2 1.1

//1   100 20
//2   136 31
//3   172 42
//4   208 53
//5   244 64
//6   280 75
//7   316 86

//*   30  10