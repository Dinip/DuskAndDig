using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "PlayerOrEnemy/EnemyObject")]
public class EnemyObject : ScriptableObject
{
    [SerializeField]
    private TimeControllerObject timeObject;

    [SerializeField]
    private float baseHealth = 10f;

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
        ComputeValues();
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

//----------------------//

//b   15 10
//x   1.2 1.1

//1   15  10
//2   51  21
//3   87  32
//4   123 43
//5   159 54
//6   195 65
//7   231 76

//*   30  10