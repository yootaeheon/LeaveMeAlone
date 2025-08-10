using Firebase.Auth;
using Firebase.Database;
using System;
using UnityEngine;
// 광고 SDK (예: Unity Ads 사용 시)
using GoogleMobileAds.Api;
using UnityEngine.Advertisements;

public class OfflineRewardManager : MonoBehaviour, IUnityAdsShowListener
{
    [SerializeField] OfflineRewardCanvas _rewardCanvas;

    private const string AD_UID = "rewardedVideo";

    private DatabaseReference dbRef;              // Firebase Realtime Database 참조
    private int goldPerSecond = 10;               // 초당 보상 골드
    private readonly long maxRewardSeconds = 21600; // 최대 보상 가능 시간: 6시간 = 21,600초

    public long calculatedSeconds = 0;           // 실제 경과 시간 (초)
    private int baseReward = 0;                   // 기본 보상량

    private void Awake()
    {
        InitAdmob();
    }

    private void Start()
    {
        // 광고 초기화 (테스트 ID와 프로덕션 ID는 대시보드에서 확인 가능)
        if (!Advertisement.isInitialized)
        {
            Advertisement.Initialize("5871255", true); // true는 테스트 모드 (릴리즈 시 false)
        }

        // Firebase Database 초기화
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;

        // 로그인된 유저일 경우 오프라인 보상 체크
        if (FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            CheckOfflineReward();
        }
    }

    private void InitAdmob()
    {
        // AdMob 초기화 (Google Mobile Ads SDK 사용 시)
        MobileAds.Initialize(initStatus => { });
    }

    // 앱이 백그라운드로 전환될 때 로그아웃 시간 저장
    private void OnApplicationPause(bool pause)
    {
        if (pause)
            SaveLogoutTime();
    }

    // 앱이 완전히 종료될 때 로그아웃 시간 저장
    void OnApplicationQuit()
    {
        SaveLogoutTime();
    }

    /// <summary>
    /// 현재 UTC 시간을 유닉스 타임스탬프로 저장 (로그아웃 시간)
    /// UTC 시간 : 1970년 1월 1일 00:00:00 UTC를 기준으로 지금까지 흐른 시간을 '초 단위'로 나타낸 숫자
    /// 시간 비교가 쉽고 빠르기 때문에 사용
    /// 사용 방법 : 현재 시간(유닉스 타임스탬프) - 마지막 접속 시간(유닉스 타임스탬프)
    /// </summary>
    private void SaveLogoutTime()
    {
        string uid = FirebaseAuth.DefaultInstance.CurrentUser?.UserId;
        if (string.IsNullOrEmpty(uid)) return;

        // 유닉스 타임스탬프 형식으로 바꿔 시간을 숫자로 표현
        long nowUnixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        dbRef.Child("users").Child(uid).Child("lastLogoutTime").SetValueAsync(nowUnixTime);
    }

    /// <summary>
    /// 저장된 로그아웃 시간과 현재 시간을 비교하여 오프라인 보상 계산
    /// </summary>
    public void CheckOfflineReward()
    {
        string uid = FirebaseAuth.DefaultInstance.CurrentUser?.UserId;
        if (string.IsNullOrEmpty(uid)) return;

        long nowUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        // Firebase에서 마지막 로그아웃 시간 가져오기
        FirebaseDatabase.DefaultInstance
            .GetReference("users").Child(uid).Child("lastLogoutTime")
            .GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted && task.Result.Exists)
                {
                    long lastLogoutUnix = Convert.ToInt64(task.Result.Value);

                    // 실제 경과 시간 계산
                    calculatedSeconds = nowUnix - lastLogoutUnix;

                    // 최대 보상 시간(6시간)을 초과하면 잘라냄
                    if (calculatedSeconds > maxRewardSeconds)
                        calculatedSeconds = maxRewardSeconds;

                    // 보상 계산 (초당 goldPerSecond 골드)
                    baseReward = (int)(calculatedSeconds * goldPerSecond);

                    Debug.Log($"[오프라인 보상] 경과 시간: {calculatedSeconds}초, 기본 보상: {baseReward}");

                    // 사용자에게 광고 보기 여부를 묻는 팝업 표시
                    _rewardCanvas.Button_Show();
                }
                else
                {
                    Debug.Log("[오프라인 보상] 로그아웃 기록이 없거나 데이터를 불러오지 못했습니다.");
                }
            });
    }


    //===============
    //    광고
    //===============

    /// <summary>
    /// 사용자가 광고 시청을 하지 않고 그냥 보상을 받을 경우
    /// </summary>
    public void Button_NoWatchingAD()
    {
        GiveReward(baseReward);
    }

    /// <summary>
    /// 사용자가 광고 시청을 선택했을 경우
    /// </summary>
    public void Button_WatchAD()
    {
        Advertisement.Show(AD_UID, this);
    }

    /// <summary>
    /// 실제 골드 보상 지급 및 UI 알림
    /// </summary>
    void GiveReward(int rewardAmount)
    {
        GameManager.Instance.Gold += (rewardAmount);                      
        Debug.Log($"[오프라인 보상] 최종 지급: {rewardAmount} 골드");
        _rewardCanvas.Button_Hide();
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId == AD_UID)
        {
            if (showCompletionState == UnityAdsShowCompletionState.COMPLETED)
            {
                Debug.Log("[광고] 광고 시청 완료 - 2배 보상 지급");
                GiveReward(baseReward * 2);
            }
            else
            {
                Debug.Log("[광고] 광고 스킵 또는 실패 - 기본 보상 지급");
                GiveReward(baseReward);
            }
        }
    }
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) { }
    public void OnUnityAdsShowStart(string placementId) { }
    public void OnUnityAdsShowClick(string placementId) { }
}
