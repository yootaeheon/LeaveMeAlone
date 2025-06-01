using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class ConsumeItemDataSO : ScriptableObject
{
    public List<ItemSO> ItemList;

    public void OnEnable()
    {
        ItemList = new List<ItemSO>()
        {
            Item1, Item2, Item3, Item4, Item5, Item6, Item7, Item8
        };
    }

    [field : SerializeField] ItemSO Item1 { get; set; }
    [field : SerializeField] ItemSO Item2 { get; set; }
    [field : SerializeField] ItemSO Item3 { get; set; }
    [field : SerializeField] ItemSO Item4 { get; set; }
    [field : SerializeField] ItemSO Item5 { get; set; }
    [field : SerializeField] ItemSO Item6 { get; set; }
    [field : SerializeField] ItemSO Item7 { get; set; }
    [field : SerializeField] ItemSO Item8 { get; set; }
}
