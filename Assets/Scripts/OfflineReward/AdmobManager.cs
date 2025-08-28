using GoogleMobileAds.Api;
using System.Threading.Tasks;
using UnityEngine;

public class AdmobManager : MonoBehaviour
{
    public string adID = "ca-app-pub-3940256099942544/1033173712"; // 테스트 광고 ID

    public InterstitialAd loadedAD;

    [SerializeField] OfflineRewardManager _offlineRewardManager;
    public OfflineRewardManager OfflineRewardManager => _offlineRewardManager ??= FindAnyObjectByType<OfflineRewardManager>();

    private void Awake()
    {
        // Admob 초기화
        MobileAds.Initialize(OnInit);
    }

    private void Start()
    {
        LoadAd();
    }

    private void OnInit(InitializationStatus status)
    {
        // Google Mobile Ads SDK 초기화
        MobileAds.Initialize(initStatus =>
        {
            if (initStatus == null)
            {
                Debug.LogError("Google Mobile Ads SDK 초기화 실패.");
                return;
            }

            Debug.Log("Google Mobile Ads SDK 초기화 성공.");
        });
    }

    public void LoadAd()
    {
        AdRequest adRequest = new AdRequest();

        InterstitialAd.Load(adID, adRequest, (ad, error) =>
            {
                if (error != null)
                {
                    Debug.LogError("광고 로드 실패: " + error.GetMessage());
                    loadedAD = null;
                    return;
                }

                Debug.Log("광고 로드 성공.");
                loadedAD = ad;
                loadedAD.OnAdFullScreenContentClosed -= LoadAd;
                loadedAD.OnAdFullScreenContentClosed += LoadAd; // 전면 광고 객체는 일회성이기 때문에 광고가 닫히면 새로운 광고 로드
              
            }
        );
    }

    public void ShowAd()
    {
        if (loadedAD != null && loadedAD.CanShowAd())
        {
            loadedAD.OnAdFullScreenContentClosed -= HandleAdClosed;
            loadedAD.OnAdFullScreenContentClosed += HandleAdClosed;
            loadedAD.Show();
            Debug.Log("광고 표시됨.");
        }
        else
        {
            Debug.LogWarning("광고가 로드되지 않았습니다. 먼저 광고를 로드하세요.");
        }
    }

    private void HandleAdClosed()
    {
        if (OfflineRewardManager != null)
        {
            OfflineRewardManager.GiveReward(OfflineRewardManager.baseReward * 2); // 예시: 광고 보상 2배
        }
        else
        {
            Debug.LogWarning("OfflineRewardManager 참조가 없습니다.");
        }
    }
}
