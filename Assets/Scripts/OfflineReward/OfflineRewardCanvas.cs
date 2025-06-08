using UnityEngine;
using UnityEngine.UI;


public class OfflineRewardCanvas : UIBinder
{
    [SerializeField] OfflineRewardManager _offlineRewardManager;
    [SerializeField] Slider _rewardAmountSlider;
    private void Start()
    {
        UpdateSlider();
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
        _rewardAmountSlider.value = (int)(_offlineRewardManager.calculatedSeconds * 360);
    }
}
