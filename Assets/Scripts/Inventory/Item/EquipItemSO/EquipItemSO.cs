using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    public enum EquipmentType
    {
        Armor,
        Helmet,
        Back,
        Weapon
    }

    [CreateAssetMenu(fileName = "New Equipment Item", menuName = "EquipItemSO")]
    public class EquipItemSO : ItemSO
    {
        [Header("-------------------------------------------")]
        [SerializeField] EquipmentType _equipmetnType;
        public EquipmentType EquipmentType { get { return _equipmetnType;  } set { _equipmetnType = value; } }

        [Header("Weapon")]
        [SerializeField] float _attackPower;
        public float AttackPower { get { return _attackPower; } set { _attackPower = value; } }

        [SerializeField] float _attackSpeed;
        public float AttackSpeed { get { return _attackSpeed; } set { _attackSpeed = value; } }

        [SerializeField] float _criticalChance; // 크리티컬 확률
        public float CriticalChance { get { return _criticalChance; } set { _criticalChance = value; } }

        [Header("Armor & Helmet & Back")]
        [SerializeField] float _plusHp; // 추가 Hp
        public float PlusHp { get { return _plusHp; } set { _plusHp = value; } }

        [SerializeField] float _recoverHpPerSecond; // 초당 Hp 회복량
        public float RecoverHpPerSecond { get { return _recoverHpPerSecond; } set { _recoverHpPerSecond = value; } }

        [SerializeField] float _defensePower;
        public float DefensePower { get { return _defensePower; } set { _defensePower = value; } }


    }
}