using Inventory.Model;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaSystem : MonoBehaviour
{
    [SerializeField] ProgressSO _progressInfo;
    [SerializeField] EquipItemData _equipItemData;
    [SerializeField] GameObject _item;
    [SerializeField] Transform _spawnPos;

    [SerializeField] Canvas _gachaCanvas;
    [SerializeField] GameObject _gachaPrefabOnCanvas;
    [SerializeField] GameObject _gachaParentObj;

    [SerializeField] InventorySO InventoryData; // �κ��丮 ������

    [Header("ĳ���� ���� ���� (1~100)")]
    /*[Range(1, 100)]*/
    public int _characterLevel;  // �÷��̾��� ���� ���� (Ȯ�� ���� �߽ɰ��� ����)

    [Header("ǥ������: Ŭ���� ���� �л� Ŀ��")]
    public float stdDev;  // ���Ժ����� ǥ������, �������� �� �پ��� ���� ����

    [Header("������ ������ Ŀ���� Ȯ�� ���̺�")]
    public DropTable helmetTable = new DropTable("Helmet"); // ��� ������ ��� ���̺�
    public DropTable armorTable = new DropTable("Armor");   // ���� ������ ��� ���̺�
    public DropTable weaponTable = new DropTable("Weapon"); // ���� ������ ��� ���̺�
    public DropTable Back = new DropTable("Back");          // ���� ������ ��� ���̺�

    [Header("õ�� �ý���: ���� ���� �� ����")]
    public int pityThreshold = 10; // ���� ���� �������� �������� ���� �� �ִ� �ִ� Ƚ��
    private int pityCounter = 0;   // ������� ���� ������ ���� Ƚ��

    // ������ ���� Ÿ�� ����
    public enum EquipType { Helmet, Armor, Back, Weapon }
    public EquipType type;

    private List<ItemSO> HelmetList;
    private List<ItemSO> ArmorList;
    private List<ItemSO> BackList;
    private List<ItemSO> WeaponList;

    private Dictionary<EquipType, List<ItemSO>> EqipItemDataDic;

    private void Awake()
    {
        InitData();
        LevelChanged();
    }

    private void OnEnable()
    {
        _progressInfo.OnChapterChanged += LevelChanged;
        _progressInfo.OnStageChanged += LevelChanged;
    }

    private void OnDisable()
    {
        _progressInfo.OnChapterChanged -= LevelChanged;
        _progressInfo.OnStageChanged -= LevelChanged;
    }

    public void LevelChanged()
    {
        _characterLevel = (_progressInfo.Chapter - 1) * 10 + _progressInfo.Stage;

        OnValidate();
    }

    /// <summary>
    /// ������ �󿡼� ĳ���� ������ ������ �� �ڵ����� ��� ���̺� ����
    /// </summary>
    void OnValidate()
    {
        helmetTable.GenerateDistribution(_characterLevel, stdDev);
        armorTable.GenerateDistribution(_characterLevel, stdDev);
        weaponTable.GenerateDistribution(_characterLevel, stdDev);
        Back.GenerateDistribution(_characterLevel, stdDev);
    }

    /// <summary>
    /// ��í ��ư ���� �Լ�
    /// ���� ������ �����Ͽ� �κ��丮 �߰� ���
    /// </summary>
    public void Button_Gacha()
    {
        if (GameManager.Instance.Gold < 5000)
            return;

        GameManager.Instance.Gold -= 5000;

        ItemSO resultItem = GetRandomItem();
        AddInventory(resultItem, 1);

        _gachaCanvas.gameObject.SetActive(true); // ��í UI Ȱ��ȭ

        // ������ ����
        GameObject resultUI = Instantiate(_gachaPrefabOnCanvas);
        resultUI.transform.SetParent(_gachaParentObj.transform, false);

        //������ �����տ� ������� ������ �̹����� ���� ���� �־��ֱ�
        resultUI.transform.GetChild(0).GetComponent<Image>().sprite = resultItem.ItemImage;
    }

    /// <summary>
    /// 10ȸ ��í ��ư (���� 10�� �̱�)
    /// </summary>
    public void Button_Gacha10()
    {
        int cost = 5000 * 10;

        if (GameManager.Instance.Gold < cost)
            return;

        GameManager.Instance.Gold -= cost;

        _gachaCanvas.gameObject.SetActive(true); // ��í UI Ȱ��ȭ

        for (int i = 0; i < 10; i++)
        {
            ItemSO resultItem = GetRandomItem();

            // �κ��丮�� �߰�
            AddInventory(resultItem, 1);

            // UI ����
            GameObject resultUI = Instantiate(_gachaPrefabOnCanvas);
            resultUI.transform.SetParent(_gachaParentObj.transform, false);
            resultUI.transform.GetChild(0).GetComponent<Image>().sprite = resultItem.ItemImage;

            Debug.Log($"[10���� {i + 1}ȸ��] {resultItem.Name}");
        }
    }

    // ����� �κ��丮�� �߰��ϴ� �Լ�
    public void AddInventory(ItemSO itemSO, int quantity)
    {
        int reminder = InventoryData.AddItem(itemSO, quantity);

        if (reminder > 0)
        {
            Debug.LogWarning($"�κ��丮�� ������ �����ؼ� {reminder}���� ���� ���߽��ϴ�.");
            // TODO: ������, ��� �� �߰� ó��
        }
        else
        {
            Debug.Log($"�κ��丮�� [{itemSO.Name}] {quantity}�� �߰� �Ϸ�");
        }
    }

    public void Button_Hide()
    {
        _gachaCanvas.gameObject.SetActive(false); // ��í UI ��Ȱ��ȭ
        foreach (Transform child in _gachaParentObj.transform)
        {
            Destroy(child.gameObject); // ������ ��í ��� UI ����
        }
    }

    /// <summary>
    /// ���� �̱� ���� �Լ�
    /// ���� ���� ���� �� ���� Ȯ�� ��� �� õ�� �ý��� ���� �� ��� ��ȯ
    /// </summary>
    /// <returns>(����, ������ ����) Ʃ�� ��ȯ</returns>
    public ItemSO GetRandomItem()
    {
        // 0~3 ���� ���� ������ ���� ����
        EquipType type = (EquipType)UnityEngine.Random.Range(0, 4);

        int level = _characterLevel; // �⺻ ������ ����
        DropTable table = GetTable(type); // �ش� ������ ��� ���̺� ��������

        // õ�� �ý���: �������� ���� ������ ������ �ְ� ���� ����
        if (pityCounter >= pityThreshold)
        {
            level = 10;          // �ְ� ���� ������ ����
            pityCounter = 0;     // pity �ʱ�ȭ
        }
        else
        {
            // Ȯ�� ��� ���� �̱�
            level = table.GetWeightedItemLevel();

            // ���� ����(9 �̻�)�̸� pity �ʱ�ȭ, �ƴϸ� ����
            pityCounter = level >= 9 ? 0 : pityCounter + 1;
        }

        // ����� �α� ���
        Debug.Log($"[{type}] Lv.{level} ������ ȹ��");

        var resultItemList = EqipItemDataDic[type];

        return resultItemList[level - 1]; ; // ��� ��ȯ
    }

    /// <summary>
    /// ������ Ÿ�Կ� ���� �ش� ��� ���̺� ��ȯ
    /// </summary>
    /// <param name="type">������ ���� Ÿ��</param>
    /// <returns>�ش� ������ ��� ���̺�</returns>
    DropTable GetTable(EquipType type)
    {
        return type switch
        {
            EquipType.Helmet => helmetTable,
            EquipType.Armor => armorTable,
            EquipType.Weapon => weaponTable,
            EquipType.Back => Back,
            _ => helmetTable // �⺻���� ���
        };
    }

    #region ��í������ ����Ʈ �߰�
    public void InitData()
    {
        HelmetList = new List<ItemSO>()
        {
            _equipItemData.Helmet1, _equipItemData.Helmet2, _equipItemData.Helmet3, _equipItemData.Helmet4, _equipItemData.Helmet5,
            _equipItemData.Helmet6, _equipItemData.Helmet7, _equipItemData.Helmet8, _equipItemData.Helmet9, _equipItemData.Helmet10
        };

        ArmorList = new List<ItemSO>()
        {
            _equipItemData.Armor1, _equipItemData.Armor2, _equipItemData.Armor3, _equipItemData.Armor4, _equipItemData.Armor5,
            _equipItemData.Armor6, _equipItemData.Armor7, _equipItemData.Armor8, _equipItemData.Armor9, _equipItemData.Armor10
        };

        BackList = new List<ItemSO>()
        {
            _equipItemData.Back1, _equipItemData.Back2, _equipItemData.Back3, _equipItemData.Back4, _equipItemData.Back5,
            _equipItemData.Back6, _equipItemData.Back7, _equipItemData.Back8, _equipItemData.Back9, _equipItemData.Back10
        };

        WeaponList = new List<ItemSO>()
        {
            _equipItemData.Weapon1, _equipItemData.Weapon2, _equipItemData.Weapon3, _equipItemData.Weapon4, _equipItemData.Weapon5,
            _equipItemData.Weapon6, _equipItemData.Weapon7, _equipItemData.Weapon8, _equipItemData.Weapon9, _equipItemData.Weapon10
        };

        // ��ųʸ� �ʱ�ȭ
        EqipItemDataDic = new Dictionary<EquipType, List<ItemSO>>()
        {
            { EquipType.Helmet, HelmetList },
            { EquipType.Armor, ArmorList },
            { EquipType.Back, BackList },
            { EquipType.Weapon, WeaponList }
        };
    }
    #endregion
}
