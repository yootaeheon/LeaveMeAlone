using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
// ���� SDK (��: Unity Ads ��� ��)
using GoogleMobileAds.Api;
using System;
using UnityEngine;

public class OfflineRewardManager : MonoBehaviour
{
    [SerializeField] OfflineRewardCanvas _rewardCanvas;

    private AdmobManager _admobManager;

    private DatabaseReference userDataRef;              // Firebase Realtime Database ����
    private int goldPerSecond = 10;               // �ʴ� ���� ���
    private readonly long maxRewardSeconds = 21600; // �ִ� ���� ���� �ð�: 6�ð� = 21,600��

    public long calculatedSeconds = 0;           // ���� ��� �ð� (��)
    public int baseReward = 0;                   // �⺻ ����

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
        _admobManager = GetComponent<AdmobManager>();
    }


    private void Start()
    {
        // Firebase Database �ʱ�ȭ
        string userId = BackendManager.Auth?.CurrentUser?.UserId;
        userDataRef = FirebaseDatabase.DefaultInstance.RootReference.Child(userId);

        // �α��ε� ������ ��� �������� ���� üũ
        if (FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            Debug.Log("[�������� ����] �α��ε� ������ �ֽ��ϴ�. �������� ���� üũ ����.");
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
        userDataRef.Child("gameData").Child("lastLogoutTime").SetValueAsync(nowUnixTime);
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
        userDataRef.Child("gameData").Child("lastLogoutTime").GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    long lastLogoutUnix = Convert.ToInt64(task.Result.Value);

                    // ���� ��� �ð� ���
                    calculatedSeconds = nowUnix - lastLogoutUnix;
                    _rewardCanvas.UpdateSlider();
                    Debug.Log($"[�������� ����] ������ �α׾ƿ� �ð�: {lastLogoutUnix}, ���� �ð�: {nowUnix}, ��� �ð�: {calculatedSeconds}��");

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
       /* ShowRewardedInterstitialAd();*/
       _admobManager.ShowAd();
    }

    /// <summary>
    /// ���� ��� ���� ���� �� UI �˸�
    /// </summary>
    public void  GiveReward(int rewardAmount)
    {
        GameManager.Instance.Gold += (rewardAmount);
        Debug.Log($"[�������� ����] ���� ����: {rewardAmount} ���");
        _rewardCanvas.Button_Hide();
    }
}
