using Inventory.Model;
using System.Collections.Generic;
using UnityEngine;

//Back, BodyArmor, 12_Helmet2, R_Weapon을 바꿔야 에셋 바뀜


/// <summary>
/// 장비 아이템 장착/해제/조회 관리하는 클래스
/// 각 EquipmentType에 따라 하나의 아이템만 장착 가능
/// </summary>
public class EquipmentManager : MonoBehaviour
{
    [SerializeField] CharacterModel _model;

    [SerializeField] SpriteRenderer _helmetSpriteRenderer;
    [SerializeField] SpriteRenderer _armorSpriteRenderer;
    [SerializeField] SpriteRenderer _backSpriteRenderer;
    [SerializeField] SpriteRenderer _weaponSpriteRenderer;

    /// <summary>
    /// 현재 장착중인 아이템들 저장하는 딕셔너리
    /// 키 : 장비 타입 (Ex> Aromor, Weapon 등)
    /// 값 : 해당 타입에 장착한 EquipItemSO
    /// 장착할 장비의 스탯만큼 플러스
    /// </summary>
    private Dictionary<EquipmentType, EquipItemSO> _equippedItems = new();

    /// <summary>
    /// 장비 장착 메서드
    /// </summary>
    /// <param name="item"></param>
    public void EquipItem(EquipItemSO item)
    {
        if (_equippedItems.TryGetValue(item.EquipmentType, out var equipped))
        {
            UnEquipItem(item.EquipmentType);
        }

        _equippedItems[item.EquipmentType] = item;

        switch (item.EquipmentType)
        {
            case EquipmentType.Helmet:
                _helmetSpriteRenderer.sprite = item.ItemImage;
                _model.MaxHp += item.PlusMaxHp;
                _model.CurHp += item.PlusMaxHp; // HP는 그대로 유지하면서 Max 증가분만큼 회복
                _model.DefensePower += item.DefensePower;
                _model.RerecoverHpPerSecond += item.RecoverHpPerSecond;
                break;
            case EquipmentType.Armor:
                _armorSpriteRenderer.sprite = item.ItemImage;
                _model.MaxHp += item.PlusMaxHp;
                _model.CurHp += item.PlusMaxHp; // HP는 그대로 유지하면서 Max 증가분만큼 회복
                _model.DefensePower += item.DefensePower;
                _model.RerecoverHpPerSecond += item.RecoverHpPerSecond;
                break;
            case EquipmentType.Back:
                _backSpriteRenderer.sprite = item.ItemImage;
                _model.MaxHp += item.PlusMaxHp;
                _model.CurHp += item.PlusMaxHp; // HP는 그대로 유지하면서 Max 증가분만큼 회복
                _model.DefensePower += item.DefensePower;
                _model.RerecoverHpPerSecond += item.RecoverHpPerSecond;
                break;
            case EquipmentType.Weapon:
                _weaponSpriteRenderer.sprite = item.ItemImage;
                _model.AttackPower += item.AttackPower;
                _model.AttackSpeed += item.AttackSpeed;
                _model.CriticalChacnce += item.CriticalChance;
                break;
        };
    }

    /// <summary>
    /// 장비 장착 해제 메서드
    /// 장착중인 장비의 스탯 마이너스
    /// 장착중인 장비 딕셔너리에서 제거
    /// </summary>
    /// <param name="type"></param>
    public void UnEquipItem(EquipmentType type)
    {
        if (_equippedItems.TryGetValue(type, out var item))
        {
            // 스탯 제거
            if (item.EquipmentType == EquipmentType.Weapon)
            {
                _model.AttackPower -= item.AttackPower;
                _model.AttackSpeed -= item.AttackSpeed;
                _model.CriticalChacnce -= item.CriticalChance;
            }
            else
            {
                _model.MaxHp -= item.PlusMaxHp;
                _model.CurHp = Mathf.Min(_model.CurHp, _model.MaxHp); // MaxHp보다 큰 경우 조정

                _model.DefensePower -= item.DefensePower;
                _model.RerecoverHpPerSecond -= item.RecoverHpPerSecond;
            }
            _equippedItems.Remove(type);
        }
    }

    /// <summary>
    /// 특정 EquipmentType에 장착된 아이템을 반환
    /// 없으면 null 반환.
    /// </summary>
    /// <param name="type">조회할 장비의 타입</param>
    /// <returns>장착된 장비 아이템 또는 null</returns>
    public EquipItemSO GetEquippedItem(EquipmentType type)
    {
        _equippedItems.TryGetValue(type, out EquipItemSO item);
        return item;
    }
}
