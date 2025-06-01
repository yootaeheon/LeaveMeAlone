using UnityEngine;

namespace Inventory.Model
{
    public class ItemSO : ScriptableObject
    {
        [HideInInspector] public bool IsStackable { get; set; } = true;

        public int ID => GetInstanceID();

        [field: SerializeField] public int MaxStackSize { get; set; } = 1;

        [field: SerializeField] public string Name { get; set; }

        [field: SerializeField][field: TextArea] public string Description { get; set; }

        [field: SerializeField] AudioClip ActionSFX { get; set; }

        [field: SerializeField] public Sprite ItemImage { get; set; }
    }
}