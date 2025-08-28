using GoogleMobileAds.Api;
using System.Threading.Tasks;
using UnityEngine;

public class AdmobManager : MonoBehaviour
{
    public string adID = "ca-app-pub-3940256099942544/1033173712"; // �׽�Ʈ ���� ID

    public InterstitialAd loadedAD;

    [SerializeField] OfflineRewardManager _offlineRewardManager;
    public OfflineRewardManager OfflineRewardManager => _offlineRewardManager ??= FindAnyObjectByType<OfflineRewardManager>();

    private void Awake()
    {
        // Admob �ʱ�ȭ
        MobileAds.Initialize(OnInit);
    }

    private void Start()
    {
        LoadAd();
    }

    private void OnInit(InitializationStatus status)
    {
        // Google Mobile Ads SDK �ʱ�ȭ
        MobileAds.Initialize(initStatus =>
        {
            if (initStatus == null)
            {
                Debug.LogError("Google Mobile Ads SDK �ʱ�ȭ ����.");
                return;
            }

            Debug.Log("Google Mobile Ads SDK �ʱ�ȭ ����.");
        });
    }

    public void LoadAd()
    {
        AdRequest adRequest = new AdRequest();

        InterstitialAd.Load(adID, adRequest, (ad, error) =>
            {
                if (error != null)
                {
                    Debug.LogError("���� �ε� ����: " + error.GetMessage());
                    loadedAD = null;
                    return;
                }

                Debug.Log("���� �ε� ����.");
                loadedAD = ad;
                loadedAD.OnAdFullScreenContentClosed -= LoadAd;
                loadedAD.OnAdFullScreenContentClosed += LoadAd; // ���� ���� ��ü�� ��ȸ���̱� ������ ���� ������ ���ο� ���� �ε�
              
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
            Debug.Log("���� ǥ�õ�.");
        }
        else
        {
            Debug.LogWarning("���� �ε���� �ʾҽ��ϴ�. ���� ���� �ε��ϼ���.");
        }
    }

    private void HandleAdClosed()
    {
        if (OfflineRewardManager != null)
        {
            OfflineRewardManager.GiveReward(OfflineRewardManager.baseReward * 2); // ����: ���� ���� 2��
        }
        else
        {
            Debug.LogWarning("OfflineRewardManager ������ �����ϴ�.");
        }
    }
}
