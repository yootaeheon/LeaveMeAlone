using System;
using UnityEngine;
using static Assets.Scripts.Item.ElementTypeEnum;

namespace Inventory.Model
{
    [CreateAssetMenu(menuName = "Inventory/Consume Item")]
    public class ConsumeItemSO : ItemSO
    {
        [field: SerializeField] public int Value { get; set; }

        [field: SerializeField] public ElementType ElementType { get; set; }
    }
}
