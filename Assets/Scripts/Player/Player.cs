using TarodevController;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private EventBus eventBus;

    [SerializeField]
    private HealthObject health;

    [SerializeField]
    private InventoryObject inventory;

    [SerializeField]
    private InventoryObject equipment;

    [SerializeField]
    private BuildingsSet buildings;

    [SerializeField]
    private float range;

    [SerializeField]
    private float colliderDistance;

    [SerializeField]
    private BoxCollider2D boxCollider;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip[] sounds;

    [SerializeField]
    private LayerMask enemyLayer;

    private Enemy _enemy;

    private PlayerController _playerController;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _playerController.Attacked += DamageEnemy;
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void TakeDamage(float damage)
    {
        var shieldValue = Utils.ComputePlayerShield(equipment);
        health.currentHealth -= damage * (1 - (shieldValue / 100));
        if (health.currentHealth <= 0)
        {
            health.currentHealth = 0;
            eventBus.gameOverEvent?.Invoke(true);
        }
    }

    public void CollectItem(ItemObject item)
    {
        inventory.AddItem(item.CreateItem(), 1);
        audioSource.PlayOneShot(sounds[0]);
    }

    private bool EnemyInSight()
    {
        var collider =
            Physics2D.OverlapBox(boxCollider.bounds.center + colliderDistance * range * (_spriteRenderer.flipX ? -1 : 1) * transform.right,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0f, enemyLayer);

        if (collider != null)
        {
            _enemy = collider.transform.GetComponent<Enemy>();
        }

        return collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(boxCollider.bounds.center + colliderDistance * range * (!Application.isPlaying ? 1 : (_spriteRenderer.flipX ? -1 : 1)) * transform.right,
                new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void DamageEnemy()
    {
        if (EnemyInSight())
        {
            var damage = Utils.ComputePlayerDamage(equipment, buildings);
            _enemy.TakeDamage(damage);
        }
    }
}
