using Inventory.Model;
using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GachaSystem : MonoBehaviour
{
    [SerializeField] ProgressSO _progressInfo;
    [SerializeField] EquipItemData _equipItemData;
    [SerializeField] GameObject _item;
    [SerializeField] Transform _spawnPos;

    [Header("캐릭터 레벨 설정 (1~100)")]
    /*[Range(1, 100)]*/
    public int _characterLevel;  // 플레이어의 현재 레벨 (확률 분포 중심값에 영향)

    [Header("표준편차: 클수록 레벨 분산 커짐")]
    public float stdDev;  // 정규분포의 표준편차, 높을수록 더 다양한 레벨 등장

    [Header("아이템 부위별 커스텀 확률 테이블")]
    public DropTable helmetTable = new DropTable("Helmet"); // 헬멧 아이템 드롭 테이블
    public DropTable armorTable = new DropTable("Armor");   // 갑옷 아이템 드롭 테이블
    public DropTable weaponTable = new DropTable("Weapon"); // 무기 아이템 드롭 테이블
    public DropTable Back = new DropTable("Back");          // 망토 아이템 드롭 테이블

    [Header("천장 시스템: 실패 누적 시 보상")]
    public int pityThreshold = 10; // 낮은 레벨 아이템이 연속으로 나올 수 있는 최대 횟수
    private int pityCounter = 0;   // 현재까지 낮은 레벨이 나온 횟수

    // 아이템 부위 타입 정의
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
    /// 에디터 상에서 캐릭터 레벨을 변경할 때 자동으로 드롭 테이블 갱신
    /// </summary>
    void OnValidate()
    {
        helmetTable.GenerateDistribution(_characterLevel, stdDev);
        armorTable.GenerateDistribution(_characterLevel, stdDev);
        weaponTable.GenerateDistribution(_characterLevel, stdDev);
        Back.GenerateDistribution(_characterLevel, stdDev);
    }

    private void Update()
    {
      /*  Debug.Log($"진행도 및 캐릭터 레벨 = {_characterLevel}");*/

        if (Input.GetKeyDown(KeyCode.V))
        {
            LevelChanged();
        }
    }

    public void Button_Gacha()
    {
        if (GameManager.Instance.Gold < 5000)
            return;

        GameManager.Instance.Gold -= 5000;

        var (itemType, level) = GetRandomItem();
        var list = EqipItemDataDic[itemType];

        // 아이템 생성
        if (level >= 1 && level <= list.Count)
        {
            GameObject item = Instantiate(_item, _spawnPos.position, Quaternion.identity);
            item.GetComponent<Item>().InventoryItem = list[level-1];
            Debug.Log($"[소환됨] {itemType} Lv.{level}");
        }
        else
        {
            Debug.LogWarning($"레벨 {level}에 해당하는 아이템이 존재하지 않습니다.");
        }
    }



    /// <summary>
    /// 실제 뽑기 실행 함수
    /// 부위 랜덤 선택 → 레벨 확률 계산 → 천장 시스템 적용 → 결과 반환
    /// </summary>
    /// <returns>(부위, 아이템 레벨) 튜플 반환</returns>
    public (EquipType itemType, int level) GetRandomItem()
    {
        // 0~3 사이 랜덤 정수로 부위 선택
        EquipType type = (EquipType)UnityEngine.Random.Range(0, 4);

        int level = _characterLevel; // 기본 아이템 레벨
        DropTable table = GetTable(type); // 해당 부위의 드롭 테이블 가져오기

        // 천장 시스템: 연속으로 낮은 레벨이 나오면 최고 레벨 지급
        if (pityCounter >= pityThreshold)
        {
            level = 10;          // 최고 레벨 아이템 보상
            pityCounter = 0;     // pity 초기화
        }
        else
        {
            // 확률 기반 레벨 뽑기
            level = table.GetWeightedItemLevel();

            // 높은 레벨(9 이상)이면 pity 초기화, 아니면 누적
            pityCounter = level >= 9 ? 0 : pityCounter + 1;
        }

        // 디버그 로그 출력
        Debug.Log($"[{type}] Lv.{level} 아이템 획득");

        return (type, level); // 결과 반환
    }

    /// <summary>
    /// 아이템 타입에 따른 해당 드롭 테이블 반환
    /// </summary>
    /// <param name="type">아이템 부위 타입</param>
    /// <returns>해당 부위의 드롭 테이블</returns>
    DropTable GetTable(EquipType type)
    {
        return type switch
        {
            EquipType.Helmet => helmetTable,
            EquipType.Armor => armorTable,
            EquipType.Weapon => weaponTable,
            EquipType.Back => Back,
            _ => helmetTable // 기본값은 헬멧
        };
    }

    #region 가챠아이템 리스트 추가
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

        // 딕셔너리 초기화
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
