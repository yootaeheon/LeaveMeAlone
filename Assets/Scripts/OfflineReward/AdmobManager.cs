using GoogleMobileAds.Api;
using UnityEngine;

public class AdmobManager : MonoBehaviour
{
    public string adID = "ca-app-pub-3940256099942544/1033173712"; // 테스트 광고 ID

    public InterstitialAd loadedAD;

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
            }
        );
    }

    public void ShowAd()
    {
        if (loadedAD != null && loadedAD.CanShowAd())
        {
            loadedAD.Show();
            Debug.Log("광고 표시됨.");
        }
        else
        {
            Debug.LogWarning("광고가 로드되지 않았습니다. 먼저 광고를 로드하세요.");
        }
    }
}
