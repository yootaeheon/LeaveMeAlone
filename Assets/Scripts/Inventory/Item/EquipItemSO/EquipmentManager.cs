using Inventory.Model;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    [SerializeField] CharacterModel _model;

    [SerializeField] SpriteRenderer _helmetSpriteRenderer;
    [SerializeField] SpriteRenderer _armorSpriteRenderer;
    [SerializeField] SpriteRenderer _backSpriteRenderer;
    [SerializeField] SpriteRenderer _weaponSpriteRenderer;

    private Dictionary<EquipmentType, EquipItemSO> _equippedItems = new();

    public void EquipItem(EquipItemSO item)
    {
        // 기존 아이템 해제
        if (_equippedItems.TryGetValue(item.EquipmentType, out var equippedItem))
        {
            UnEquipItem(item.EquipmentType);
        }

        // 새 아이템 장착
        _equippedItems[item.EquipmentType] = item;

        switch (item.EquipmentType)
        {
            case EquipmentType.Helmet:
                _helmetSpriteRenderer.sprite = item.ItemImage;
                ApplyCommonStats(item);
                break;
            case EquipmentType.Armor:
                _armorSpriteRenderer.sprite = item.ItemImage;
                ApplyCommonStats(item);
                break;
            case EquipmentType.Back:
                _backSpriteRenderer.sprite = item.ItemImage;
                ApplyCommonStats(item);
                break;
            case EquipmentType.Weapon:
                _weaponSpriteRenderer.sprite = item.ItemImage;
                _model.AttackPower += item.AttackPower;
                _model.AttackSpeed += item.AttackSpeed;
                _model.CriticalChacnce += item.CriticalChance;
                break;
        }
    }

    public void UnEquipItem(EquipmentType type)
    {
        if (_equippedItems.TryGetValue(type, out var item))
        {
            switch (type)
            {
                case EquipmentType.Helmet:
                    _helmetSpriteRenderer.sprite = null;
                    RemoveCommonStats(item);
                    break;
                case EquipmentType.Armor:
                    _armorSpriteRenderer.sprite = null;
                    RemoveCommonStats(item);
                    break;
                case EquipmentType.Back:
                    _backSpriteRenderer.sprite = null;
                    RemoveCommonStats(item);
                    break;
                case EquipmentType.Weapon:
                    _weaponSpriteRenderer.sprite = null;
                    _model.AttackPower -= item.AttackPower;
                    _model.AttackSpeed -= item.AttackSpeed;
                    _model.CriticalChacnce -= item.CriticalChance;
                    break;
            }

            _equippedItems.Remove(type);
        }
    }

    public EquipItemSO GetEquippedItem(EquipmentType type)
    {
        _equippedItems.TryGetValue(type, out EquipItemSO item);
        return item;
    }

    private void ApplyCommonStats(EquipItemSO item)
    {
        _model.MaxHp += item.PlusMaxHp;
        _model.CurHp += item.PlusMaxHp; // 회복
        _model.DefensePower += item.DefensePower;
        _model.RerecoverHpPerSecond += item.RecoverHpPerSecond;
    }

    private void RemoveCommonStats(EquipItemSO item)
    {
        _model.MaxHp -= item.PlusMaxHp;
        _model.CurHp = Mathf.Min(_model.CurHp, _model.MaxHp);
        _model.DefensePower -= item.DefensePower;
        _model.RerecoverHpPerSecond -= item.RecoverHpPerSecond;
    }
}
