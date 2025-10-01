using UnityEngine;
using UnityEngine.UI;


public class OfflineRewardCanvas : UIBinder
{
    [SerializeField] OfflineRewardManager _offlineRewardManager;
    [SerializeField] AdmobManager _admobManager;

    [SerializeField] Slider _rewardAmountSlider;

    [SerializeField] Button _rewardButton;
    [SerializeField] Button _reward_Ad_Button;

    public TMPro.TMP_Text _rewardGoldText;
    public TMPro.TMP_Text _rewardGemText;

    private void Awake()
    {
        BindAll();

        if (_rewardGoldText == null)
        {
            _rewardGoldText = GetUI<TMPro.TMP_Text>("_rewardGoldText");
        }

        if (_rewardGemText == null)
        {
            _rewardGemText = GetUI<TMPro.TMP_Text>("_rewardGemText");
        }

        if (_rewardAmountSlider == null)
        {
            _rewardAmountSlider = GetUI<Slider>("RewardAmount_Slider");
        }

        if (_rewardButton == null)
        {
            _rewardButton = GetUI<Button>("GetReward_Button");
        }
        if (_reward_Ad_Button == null)
        {
            _reward_Ad_Button = GetUI<Button>("GetReward*2_Button");
        }
    }

    private void OnEnable()
    {
        _rewardButton.onClick.AddListener(() => _offlineRewardManager.GiveReward(_offlineRewardManager.baseReward));
        _reward_Ad_Button.onClick.AddListener(() => _admobManager.ShowAd());
    }

    private void OnDisable()
    {
        _rewardButton.onClick.RemoveAllListeners();
        _reward_Ad_Button.onClick.RemoveAllListeners();
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
