using DG.Tweening;
using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [field : SerializeField] public ItemSO InventoryItem { get; set; }
    [field: SerializeField] public int Quantity { get; set; } = 1;

    [SerializeField] float _animDuration = 0.3f;

    [SerializeField] ConsumeItemDataSO ItemData;

    private AudioSource _audioSource;


    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        InventoryItem = ItemData.ItemList[Random.Range(0, ItemData.ItemList.Count-1)];
    }

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = InventoryItem.ItemImage;
    }

    private void Update()
    {
        ItemMove();
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
        sequence.Append(transform.DOScale(Vector3.zero, _animDuration));
        sequence.OnComplete(()=>Destroy(gameObject));
    }

    public void ItemMove()
    {
        transform.Translate(Vector2.left * 1f * Time.deltaTime);
    }
}
