using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    public enum RecoveryType
    {
        HP,
        MP,
    }

    [CreateAssetMenu]
    public class ConsumeItemSO : ItemSO
    {
        [field: SerializeField] public int Value {  get; set; }

        public void Consume()
        {

        }

    }
}