using UnityEngine;
using UnityEngine.UI;


public class OfflineRewardCanvas : UIBinder
{
    [SerializeField] OfflineRewardManager _offlineRewardManager;

    [SerializeField] Slider _rewardAmountSlider;

    public TMPro.TMP_Text _rewardGoldText;
    public TMPro.TMP_Text _rewardGemText;

    private void Awake()
    {
        if (_rewardGoldText == null)
        {
            _rewardGoldText = GetUI<TMPro.TMP_Text>("_rewardGoldText");
        }

        if (_rewardGemText == null)
        {
            _rewardGemText = GetUI<TMPro.TMP_Text>("_rewardGemText");
        }
    }

    public void Button_Show()
    {
        gameObject.SetActive(true);
    }

    public void Button_Hide()
    {
        gameObject.SetActive(false);
    }

    public void UpdateSlider()
    {
        _rewardAmountSlider.value = (int)_offlineRewardManager.calculatedSeconds / 3600f;
        Debug.Log($"[오프라인 보상] 슬라이더 업데이트: {_rewardAmountSlider.value}시간");
    }

    public void UpdateRewardText()
    {
       float rewardAmout = (int)_offlineRewardManager.calculatedSeconds / 3600f;
         _rewardGoldText.text = $"{_offlineRewardManager.baseReward}";
        _rewardGemText.text = $"{(int)(rewardAmout)}";
    }
}
