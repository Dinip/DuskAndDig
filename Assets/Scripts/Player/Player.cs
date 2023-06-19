using System;
using System.Linq;
using TarodevController;
using UnityEngine;

public class Player : MonoBehaviour
{
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
    private LayerMask enemyLayer;

    private Enemy _enemy;

    private PlayerController _playerController;

    private float _beaconMultiplier = 1;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _playerController.Attacked += DamageEnemy;
        var beacons = buildings.Items.FindAll(b => b.buildingType == BuildingType.Beacon);
        _beaconMultiplier = beacons.Sum(b => b.CurrentMultiplier);
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void TakeDamage(float damage)
    {
        var armor = equipment.GetSlots.FirstOrDefault(x => x.ItemObject.type == ItemType.Armor);
        if (armor != null)
        {
            var shieldItem = armor.ItemObject.data.buffs.FirstOrDefault(b => b.attribute == Attributes.Shield);
            float shieldValue = shieldItem.value / 100f;

            health.currentHealth -= damage * (1 - shieldValue);
            return;
        }
        health.currentHealth -= damage;
    }

    public void CollectItem(ItemObject item)
    {
        inventory.AddItem(item.CreateItem(), 1);
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
        Debug.Log("attack");
        if (EnemyInSight())
        {
            var sword = equipment.GetSlots[1].ItemObject.data.buffs.First(f => f.attribute == Attributes.Damage);
            var damage = sword.value + sword.value * (_beaconMultiplier / 100);
            Debug.Log($"Damage: {damage}");
            _enemy.TakeDamage(damage);
        }
    }
}
