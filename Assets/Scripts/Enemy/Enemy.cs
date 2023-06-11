using UnityEditor.Tilemaps;
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

    private PlayerController2D _player;

    private EnemyPatrol _enemyPatrol;

    private void Awake()
    {
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
            Physics2D.OverlapBox(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0f, playerLayer);

        //RaycastHit2D hit =
        //    Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
        //new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
        //0, Vector2.left, .2f, playerLayer);
        
        if (collider != null)
        {
            _player = collider.transform.GetComponent<PlayerController2D>();
        }

        return collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + colliderDistance * range * transform.localScale.x * transform.right,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void DamagePlayer()
    {
        Debug.Log(enemyObject.Damage);
        if (PlayerInSight())
        {
            _player.TakeDamage(enemyObject.Damage);
        }
    }
}
