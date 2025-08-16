using Firebase.Auth;
using Firebase.Database;
using System;
using UnityEngine;
// ���� SDK (��: Unity Ads ��� ��)
using GoogleMobileAds.Api;

public class OfflineRewardManager : MonoBehaviour
{
    [SerializeField] OfflineRewardCanvas _rewardCanvas;

    private const string AD_UID = "rewardedVideo";

    private DatabaseReference dbRef;              // Firebase Realtime Database ����
    private int goldPerSecond = 10;               // �ʴ� ���� ���
    private readonly long maxRewardSeconds = 21600; // �ִ� ���� ���� �ð�: 6�ð� = 21,600��

    public long calculatedSeconds = 0;           // ���� ��� �ð� (��)
    private int baseReward = 0;                   // �⺻ ����

    // �������� ���ͽ�Ƽ�� ���� ��ü
    private RewardedInterstitialAd _rewardedInterstitialAd;

#if UNITY_ANDROID
    private const string AD_UNIT_ID = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
    private const string AD_UNIT_ID = "ca-app-pub-3940256099942544/1712485313";
#else
    private const string AD_UNIT_ID = "unexpected_platform";
#endif



    private void Awake()
    {
        InitAdmob();
    }

    private void Start()
    {
        // Firebase Database �ʱ�ȭ
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;

        // �α��ε� ������ ��� �������� ���� üũ
        if (FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            CheckOfflineReward();
        }
    }

    private void InitAdmob()
    {
        // Google Mobile Ads SDK �ʱ�ȭ
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // SDK �ʱ�ȭ�� �Ϸ�� �� ȣ��Ǵ� �ݹ�
        });
    }

    // �������� ���ͽ�Ƽ�� ���� �ε�
    public void LoadRewardedInterstitialAd()
    {
        // ���ο� ���� �ε��ϱ� ���� ���� ���� ����
        if (_rewardedInterstitialAd != null)
        {
            _rewardedInterstitialAd.Destroy();
            _rewardedInterstitialAd = null;
        }

        Debug.Log("�������� ���ͽ�Ƽ�� ���� �ε��մϴ�.");

        // ���� ��û ��ü ����
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        // ���� �ε� ��û
        RewardedInterstitialAd.Load(AD_UNIT_ID, adRequest,
            (RewardedInterstitialAd ad, LoadAdError error) =>
            {
                // ������ �߻��ߴ��� Ȯ��
                if (error != null || ad == null)
                {
                    Debug.LogError("�������� ���ͽ�Ƽ�� ���� �ε� ����, ����: " + error);
                    return;
                }

                Debug.Log("�������� ���ͽ�Ƽ�� ���� �ε� ����: " + ad.GetResponseInfo());
                _rewardedInterstitialAd = ad;
            });
    }

    // ���� ��׶���� ��ȯ�� �� �α׾ƿ� �ð� ����
    private void OnApplicationPause(bool pause)
    {
        if (pause)
            SaveLogoutTime();
    }

    // ���� ������ ����� �� �α׾ƿ� �ð� ����
    void OnApplicationQuit()
    {
        SaveLogoutTime();
    }

    /// <summary>
    /// ���� UTC �ð��� ���н� Ÿ�ӽ������� ���� (�α׾ƿ� �ð�)
    /// UTC �ð� : 1970�� 1�� 1�� 00:00:00 UTC�� �������� ���ݱ��� �帥 �ð��� '�� ����'�� ��Ÿ�� ����
    /// �ð� �񱳰� ���� ������ ������ ���
    /// ��� ��� : ���� �ð�(���н� Ÿ�ӽ�����) - ������ ���� �ð�(���н� Ÿ�ӽ�����)
    /// </summary>
    private void SaveLogoutTime()
    {
        string uid = FirebaseAuth.DefaultInstance.CurrentUser?.UserId;
        if (string.IsNullOrEmpty(uid)) return;

        // ���н� Ÿ�ӽ����� �������� �ٲ� �ð��� ���ڷ� ǥ��
        long nowUnixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        dbRef.Child("users").Child(uid).Child("lastLogoutTime").SetValueAsync(nowUnixTime);
    }

    /// <summary>
    /// ����� �α׾ƿ� �ð��� ���� �ð��� ���Ͽ� �������� ���� ���
    /// </summary>
    public void CheckOfflineReward()
    {
        string uid = FirebaseAuth.DefaultInstance.CurrentUser?.UserId;
        if (string.IsNullOrEmpty(uid)) return;

        long nowUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        // Firebase���� ������ �α׾ƿ� �ð� ��������
        FirebaseDatabase.DefaultInstance
            .GetReference("users").Child(uid).Child("lastLogoutTime")
            .GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted && task.Result.Exists)
                {
                    long lastLogoutUnix = Convert.ToInt64(task.Result.Value);

                    // ���� ��� �ð� ���
                    calculatedSeconds = nowUnix - lastLogoutUnix;

                    // �ִ� ���� �ð�(6�ð�)�� �ʰ��ϸ� �߶�
                    if (calculatedSeconds > maxRewardSeconds)
                        calculatedSeconds = maxRewardSeconds;

                    // ���� ��� (�ʴ� goldPerSecond ���)
                    baseReward = (int)(calculatedSeconds * goldPerSecond);

                    Debug.Log($"[�������� ����] ��� �ð�: {calculatedSeconds}��, �⺻ ����: {baseReward}");

                    // ����ڿ��� ���� ���� ���θ� ���� �˾� ǥ��
                    _rewardCanvas.Button_Show();
                }
                else
                {
                    Debug.Log("[�������� ����] �α׾ƿ� ����� ���ų� �����͸� �ҷ����� ���߽��ϴ�.");
                }
            });
    }


    //===============
    //    ����
    //===============

    /// <summary>
    /// ����ڰ� ���� ��û�� ���� �ʰ� �׳� ������ ���� ���
    /// </summary>
    public void Button_NoWatchingAD()
    {
        GiveReward(baseReward);
    }

    /// <summary>
    /// ����ڰ� ���� ��û�� �������� ���
    /// </summary>
    public void Button_WatchAD()
    {
        Debug.Log("[�������� ����] ���� ��û�� �����߽��ϴ�.");
        ShowRewardedInterstitialAd();
    }

    /// <summary>
    /// ���� ��� ���� ���� �� UI �˸�
    /// </summary>
    void GiveReward(int rewardAmount)
    {
        GameManager.Instance.Gold += (rewardAmount);                      
        Debug.Log($"[�������� ����] ���� ����: {rewardAmount} ���");
        _rewardCanvas.Button_Hide();
    }

    // �������� ���ͽ�Ƽ�� ���� ǥ��
    public void ShowRewardedInterstitialAd()
    {
        const string rewardMsg =
            "�������� ���ͽ�Ƽ�� ����� ������ ������ �޾ҽ��ϴ�. ����: {0}, ����: {1}.";

        // ���� ǥ���� �� �ִ��� Ȯ��
        if (_rewardedInterstitialAd != null && _rewardedInterstitialAd.CanShowAd())
        {
            _rewardedInterstitialAd.Show((Reward reward) =>
            {
                // TODO: ����ڿ��� ������ ����
                Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
            });
        }
    }
}
