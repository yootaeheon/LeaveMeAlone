using Inventory.Model;
using System.Collections.Generic;
using UnityEngine;

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

    private Dictionary<EquipmentType, EquipItemSO> _equippedItems = new();

    public EquipItemSO SelectedItem { get; set;}

    /// <summary>
    /// 장비 장착 메서드
    /// </summary>
    /// <param name="item">장착할 아이템</param>
    public void EquipItem(EquipItemSO item)
    {
        if (item == null)
        {
            Debug.LogWarning("장착할 아이템이 null입니다.");
            return;
        }

        // 아이템 타입이 EquipmentType에 해당하는지 확인
        if (!(item is EquipItemSO))
        {
            Debug.LogWarning("이 아이템은 장비 아이템이 아닙니다. 장착할 수 없습니다.");
            return;
        }

        if (_equippedItems.TryGetValue(item.EquipmentType, out var equipped))
        {
            UnEquipItem(item.EquipmentType);
        }

        _equippedItems[item.EquipmentType] = item;

        switch (item.EquipmentType)
        {
            case EquipmentType.Helmet:
                _helmetSpriteRenderer.sprite = item.ItemImage;
                ApplyStat(item);
                break;
            case EquipmentType.Armor:
                _armorSpriteRenderer.sprite = item.ItemImage;
                ApplyStat(item);
                break;
            case EquipmentType.Back:
                _backSpriteRenderer.sprite = item.ItemImage;
                ApplyStat(item);
                break;
            case EquipmentType.Weapon:
                _weaponSpriteRenderer.sprite = item.ItemImage;
                _model.AttackPower += item.AttackPower;
                _model.AttackSpeed += item.AttackSpeed;
                _model.CriticalChacnce += item.CriticalChance;
                break;
        }
    }

    /// <summary>
    /// 장비 장착 해제 메서드 (EquipItemSO 기반)
    /// </summary>
    /// <param name="item">해제할 아이템</param>
    public void UnEquipSelectedItem(EquipItemSO item)
    {
        if (item == null)
        {
            Debug.LogWarning("해제할 아이템이 null입니다.");
            return;
        }

        UnEquipItem(item.EquipmentType);
    }

    /// <summary>
    /// 타입 기반 장비 해제
    /// </summary>
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
                _model.CurHp = Mathf.Min(_model.CurHp, _model.MaxHp);
                _model.DefensePower -= item.DefensePower;
                _model.RerecoverHpPerSecond -= item.RecoverHpPerSecond;
            }

            // 스프라이트 초기화
            switch (type)
            {
                case EquipmentType.Helmet:
                    _helmetSpriteRenderer.sprite = null;
                    break;
                case EquipmentType.Armor:
                    _armorSpriteRenderer.sprite = null;
                    break;
                case EquipmentType.Back:
                    _backSpriteRenderer.sprite = null;
                    break;
                case EquipmentType.Weapon:
                    _weaponSpriteRenderer.sprite = null;
                    break;
            }

            _equippedItems.Remove(type);
        }
    }

    /// <summary>
    /// 장비 스탯 적용 (방어구 전용)
    /// </summary>
    private void ApplyStat(EquipItemSO item)
    {
        _model.MaxHp += item.PlusMaxHp;
        _model.CurHp += item.PlusMaxHp;
        _model.DefensePower += item.DefensePower;
        _model.RerecoverHpPerSecond += item.RecoverHpPerSecond;
    }

    /// <summary>
    /// 특정 타입에 장착된 아이템 반환
    /// </summary>
    public EquipItemSO GetEquippedItem(EquipmentType type)
    {
        _equippedItems.TryGetValue(type, out EquipItemSO item);
        return item;
    }

    public void Button_Equip()
    {
        if (SelectedItem == null)
        {
            Debug.LogWarning("[EquipManager] 장착할 장비 아이템이 선택되지 않았습니다.");
            return;
        }


        EquipItem(SelectedItem);
    }

    public ItemSO SetSelectedItem(ItemSO item)
    {
        // 아이템 타입 체크 및 할당
        if (item is EquipItemSO equipItem)
        {
            SelectedItem = equipItem;
            Debug.Log($"{SelectedItem}이 선택됨");
            return SelectedItem;
        }
        else
        {
            Debug.LogWarning("선택된 아이템은 장비 아이템이 아닙니다.");
            SelectedItem = null;
            return null;
        }
    }
}
