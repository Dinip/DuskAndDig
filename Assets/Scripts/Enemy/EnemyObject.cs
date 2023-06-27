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

    public float Health
    {
        get
        {
            return baseHealth + (timeObject.currentDay - 1) * healthMultiplier * 15;
        }
    }
    public float Damage
    {
        get
        {
            return baseDamage + (timeObject.currentDay - 1) * damageMultiplier * 10;
        }
    }
    public float AttackCooldown => attackCooldown;
}

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

//----------------------//
//b   15 10
//x   1.2 1.1

//1   15  10
//2   33  21
//3   51  32
//4   69 43
//5   87 54
//6   105 65
//7   123 76

//*   15  10