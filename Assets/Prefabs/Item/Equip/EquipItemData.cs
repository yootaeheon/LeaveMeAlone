using Inventory.Model;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EquipItemData : ScriptableObject
{
    [Header("Helmet")]
    private bool Helmet;
    [field: SerializeField] public EquipItemSO Helmet1 { get; set; }
    [field: SerializeField] public EquipItemSO Helmet2 { get; set; }
    [field: SerializeField] public EquipItemSO Helmet3 { get; set; }
    [field: SerializeField] public EquipItemSO Helmet4 { get; set; }
    [field: SerializeField] public EquipItemSO Helmet5 { get; set; }
    [field: SerializeField] public EquipItemSO Helmet6 { get; set; }
    [field: SerializeField] public EquipItemSO Helmet7 { get; set; }
    [field: SerializeField] public EquipItemSO Helmet8 { get; set; }
    [field: SerializeField] public EquipItemSO Helmet9 { get; set; }
    [field: SerializeField] public EquipItemSO Helmet10 { get; set; }

    [field: SerializeField] public EquipItemSO Armor1 { get; set; }
    [field: SerializeField] public EquipItemSO Armor2 { get; set; }
    [field: SerializeField] public EquipItemSO Armor3 { get; set; }
    [field: SerializeField] public EquipItemSO Armor4 { get; set; }
    [field: SerializeField] public EquipItemSO Armor5 { get; set; }
    [field: SerializeField] public EquipItemSO Armor6 { get; set; }
    [field: SerializeField] public EquipItemSO Armor7 { get; set; }
    [field: SerializeField] public EquipItemSO Armor8 { get; set; }
    [field: SerializeField] public EquipItemSO Armor9 { get; set; }
    [field: SerializeField] public EquipItemSO Armor10 { get; set; }

    [Header("Shoulder")]
    private bool Shoulder;
    [field: SerializeField] public EquipItemSO Shoulder1 { get; set; }
    [field: SerializeField] public EquipItemSO Shoulder11 { get; set; }
    [field: SerializeField] public EquipItemSO Shoulder2 { get; set; }
    [field: SerializeField] public EquipItemSO Shoulder22 { get; set; }
    [field: SerializeField] public EquipItemSO Shoulder3 { get; set; }
    [field: SerializeField] public EquipItemSO Shoulder33 { get; set; }
    [field: SerializeField] public EquipItemSO Shoulder4 { get; set; }
    [field: SerializeField] public EquipItemSO Shoulder44 { get; set; }
    [field: SerializeField] public EquipItemSO Shoulder5 { get; set; }
    [field: SerializeField] public EquipItemSO Shoulder55 { get; set; }
    [field: SerializeField] public EquipItemSO Shoulder6 { get; set; }
    [field: SerializeField] public EquipItemSO Shoulder66 { get; set; }
    [field: SerializeField] public EquipItemSO Shoulder7 { get; set; }
    [field: SerializeField] public EquipItemSO Shoulder77 { get; set; }
    [field: SerializeField] public EquipItemSO Shoulder8 { get; set; }
    [field: SerializeField] public EquipItemSO Shoulder88 { get; set; }
    [field: SerializeField] public EquipItemSO Shoulder9 { get; set; }
    [field: SerializeField] public EquipItemSO Shoulder99 { get; set; }
    [field: SerializeField] public EquipItemSO Shoulder10 { get; set; }
    [field: SerializeField] public EquipItemSO Shoulder1010 { get; set; }

    [Header("Back")]
    private bool Back;
    [field: SerializeField] public EquipItemSO Back1 { get; set; }
    [field: SerializeField] public EquipItemSO Back2 { get; set; }
    [field: SerializeField] public EquipItemSO Back3 { get; set; }
    [field: SerializeField] public EquipItemSO Back4 { get; set; }
    [field: SerializeField] public EquipItemSO Back5 { get; set; }
    [field: SerializeField] public EquipItemSO Back6 { get; set; }
    [field: SerializeField] public EquipItemSO Back7 { get; set; }
    [field: SerializeField] public EquipItemSO Back8 { get; set; }
    [field: SerializeField] public EquipItemSO Back9 { get; set; }
    [field: SerializeField] public EquipItemSO Back10 { get; set; }

    [Header("Back")]
    private bool Weapon;
    [field: SerializeField] public EquipItemSO Weapon1 { get; set; }
    [field: SerializeField] public EquipItemSO Weapon2 { get; set; }
    [field: SerializeField] public EquipItemSO Weapon3 { get; set; }
    [field: SerializeField] public EquipItemSO Weapon4 { get; set; }
    [field: SerializeField] public EquipItemSO Weapon5 { get; set; }
    [field: SerializeField] public EquipItemSO Weapon6 { get; set; }
    [field: SerializeField] public EquipItemSO Weapon7 { get; set; }
    [field: SerializeField] public EquipItemSO Weapon8 { get; set; }
    [field: SerializeField] public EquipItemSO Weapon9 { get; set; }
    [field: SerializeField] public EquipItemSO Weapon10 { get; set; }
}
