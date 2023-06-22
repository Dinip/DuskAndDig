using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private EnemyObject enemyObject;

    [SerializeField]
    private float range;

    [SerializeField]
    private float colliderDistance;

    [SerializeField]
    private BoxCollider2D boxCollider;

    [SerializeField]
    private LayerMask playerLayer;

    private float _cooldownTimer;

    private Animator _animator;

    private Player _player;

    private EnemyPatrol _enemyPatrol;

    private float _health;

    private void Awake()
    {
        _health = enemyObject.Health;
        _animator = GetComponent<Animator>();
        _enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    private void Update()
    {
        _cooldownTimer += Time.deltaTime;

        //Attack only when player in sight
        if (PlayerInSight())
        {
            if (_cooldownTimer >= enemyObject.AttackCooldown)
            {
                _cooldownTimer = 0;
                _animator.SetTrigger("meleeAttack");
            }
        }

        if (_enemyPatrol != null)
        {
            _enemyPatrol.enabled = !PlayerInSight();
        }
    }

    private bool PlayerInSight()
    {
        var collider =
            Physics2D.OverlapBox(boxCollider.bounds.center + colliderDistance * range * transform.localScale.x * transform.right,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0f, playerLayer);

        //RaycastHit2D hit =
        //    Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
        //new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
        //0, Vector2.left, .2f, playerLayer);

        if (collider != null)
        {
            _player = collider.transform.GetComponent<Player>();
        }

        return collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + colliderDistance * range * transform.localScale.x * transform.right,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    // used in animation event
    private void DamagePlayer()
    {
        if (PlayerInSight())
        {
            _player.TakeDamage(enemyObject.Damage);
        }
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            _enemyPatrol.enabled = false;
            _animator.SetTrigger("die");
        }
    }

    // used in animation event
    private void Die()
    {
        var parent = gameObject.transform.parent.gameObject;
        Destroy(gameObject);
        Destroy(parent);
    }
}