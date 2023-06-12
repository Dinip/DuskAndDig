using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private BoxCollider2D boxCollider;

    [SerializeField]
    private float moveSpeed;

    private ItemObject _itemObject;

    public void Initialize(ItemObject item)
    {
        _itemObject = item;
        spriteRenderer.sprite = item.uiDisplay;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(MoveAndCollect(collision.transform));
        }
    }

    private IEnumerator MoveAndCollect(Transform target)
    {
        Destroy(boxCollider);
        while (Vector3.Distance(transform.position, target.position) > 1.25f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            yield return 0;
        }
        target.GetComponent<Player>().CollectItem(_itemObject);
        Destroy(gameObject);
    }
}
