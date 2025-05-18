using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public abstract class EdibleItemSO : ItemSO
    {
        public string ActionName => "Consume";

        public AudioClip ActionSFX { get; private set; }

        public int Value;

        public abstract void Consume();
    }
}