using UnityEngine;

namespace Inventory.Model
{
    public class ItemSO : ScriptableObject
    {
        [HideInInspector] public bool IsStackable { get; set; } = true;

        // 용도 : 아이템의 고유 ID
        public int ID => GetInstanceID();

        // 아이템 인덱스는 인벤토리에서 아이템을 식별하는 용도로 사용
        [field : SerializeField] public int ItemIndex { get; set; }

        [field: SerializeField] public int MaxStackSize { get; set; } = 1;

        [field: SerializeField] public string Name { get; set; }

        [field: SerializeField][field: TextArea] public string Description { get; set; }

        [field: SerializeField] AudioClip ActionSFX { get; set; }

        [field: SerializeField] public Sprite ItemImage { get; set; }
    }
}