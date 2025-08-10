using Firebase.Auth;
using Firebase.Database;
using System;
using UnityEngine;
// ���� SDK (��: Unity Ads ��� ��)
using GoogleMobileAds.Api;
using UnityEngine.Advertisements;

public class OfflineRewardManager : MonoBehaviour, IUnityAdsShowListener
{
    [SerializeField] OfflineRewardCanvas _rewardCanvas;

    private const string AD_UID = "rewardedVideo";

    private DatabaseReference dbRef;              // Firebase Realtime Database ����
    private int goldPerSecond = 10;               // �ʴ� ���� ���
    private readonly long maxRewardSeconds = 21600; // �ִ� ���� ���� �ð�: 6�ð� = 21,600��

    public long calculatedSeconds = 0;           // ���� ��� �ð� (��)
    private int baseReward = 0;                   // �⺻ ����

    private void Awake()
    {
        InitAdmob();
    }

    private void Start()
    {
        // ���� �ʱ�ȭ (�׽�Ʈ ID�� ���δ��� ID�� ��ú��忡�� Ȯ�� ����)
        if (!Advertisement.isInitialized)
        {
            Advertisement.Initialize("5871255", true); // true�� �׽�Ʈ ��� (������ �� false)
        }

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
        // AdMob �ʱ�ȭ (Google Mobile Ads SDK ��� ��)
        MobileAds.Initialize(initStatus => { });
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
        Advertisement.Show(AD_UID, this);
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

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId == AD_UID)
        {
            if (showCompletionState == UnityAdsShowCompletionState.COMPLETED)
            {
                Debug.Log("[����] ���� ��û �Ϸ� - 2�� ���� ����");
                GiveReward(baseReward * 2);
            }
            else
            {
                Debug.Log("[����] ���� ��ŵ �Ǵ� ���� - �⺻ ���� ����");
                GiveReward(baseReward);
            }
        }
    }
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) { }
    public void OnUnityAdsShowStart(string placementId) { }
    public void OnUnityAdsShowClick(string placementId) { }
}
