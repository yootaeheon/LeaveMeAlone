using UnityEngine;

namespace Inventory.Model
{
    public class ItemSO : ScriptableObject
    {
        [HideInInspector] public bool IsStackable { get; set; } = true;

        // �뵵 : �������� ���� ID
        public int ID => GetInstanceID();

        // ������ �ε����� �κ��丮���� �������� �ĺ��ϴ� �뵵�� ���
        [field : SerializeField] public int ItemIndex { get; set; }

        [field: SerializeField] public int MaxStackSize { get; set; } = 1;

        [field: SerializeField] public string Name { get; set; }

        [field: SerializeField][field: TextArea] public string Description { get; set; }

        [field: SerializeField] AudioClip ActionSFX { get; set; }

        [field: SerializeField] public Sprite ItemImage { get; set; }
    }
}