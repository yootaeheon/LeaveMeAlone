using DG.Tweening;
using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [field : SerializeField] public ItemSO InventoryItem { get; private set; }
    [field: SerializeField] public int Quantity { get; set; } = 1;

    [SerializeField] AudioSource _audioSource;

    [SerializeField] float _duration = 0.3f;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = InventoryItem.ItemImage;
    }

    private void Update()
    {
        Move();
    }

    public void DestroyItem()
    {
        GetComponent<Collider2D>().enabled = false;
        AnimPickUpItem();
    }

    public void  AnimPickUpItem()
    {
        _audioSource.Play();
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(Vector3.zero, _duration));
        sequence.OnComplete(()=>Destroy(gameObject));
    }

    public void Move()
    {
        transform.Translate(Vector2.left * 1f * Time.deltaTime);
    }
}
