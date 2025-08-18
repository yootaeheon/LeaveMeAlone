using UnityEngine;
using UnityEngine.UI;


public class OfflineRewardCanvas : UIBinder
{
    [SerializeField] OfflineRewardManager _offlineRewardManager;

    [SerializeField] Slider _rewardAmountSlider;

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
}
