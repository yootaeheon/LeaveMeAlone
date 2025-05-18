using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class EdibleItemSO : ItemSO, IDestoryableItem, IItemAction
    {
        public string ActionName => "Consume";

        public AudioClip ActionSFX { get; private set; }

        public int Value;

        public void Use()
        {
            Debug.Log("º“∫Ò«‘");
        }
    }

    public interface IDestoryableItem
    {

    }

    public interface IItemAction
    {
        public string ActionName { get; }
        public AudioClip ActionSFX { get; }
        public void Use();
    }
}