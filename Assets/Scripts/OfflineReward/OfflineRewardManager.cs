using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
// 광고 SDK (예: Unity Ads 사용 시)
using GoogleMobileAds.Api;
using System;
using UnityEngine;

public class OfflineRewardManager : MonoBehaviour
{
    [SerializeField] OfflineRewardCanvas _rewardCanvas;

    private AdmobManager _admobManager;

    private DatabaseReference userDataRef;              // Firebase Realtime Database 참조
    private int goldPerSecond = 10;               // 초당 보상 골드
    private readonly long maxRewardSeconds = 21600; // 최대 보상 가능 시간: 6시간 = 21,600초

    public long calculatedSeconds = 0;           // 실제 경과 시간 (초)
    public int baseReward = 0;                   // 기본 보상량

    // 리워드형 인터스티셜 광고 객체
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
        // Firebase Database 초기화
        string userId = BackendManager.Auth?.CurrentUser?.UserId;
        userDataRef = FirebaseDatabase.DefaultInstance.RootReference.Child(userId);

        // 로그인된 유저일 경우 오프라인 보상 체크
        if (FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            Debug.Log("[오프라인 보상] 로그인된 유저가 있습니다. 오프라인 보상 체크 시작.");
            CheckOfflineReward();
        }
    }

    private void InitAdmob()
    {
        // Google Mobile Ads SDK 초기화
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // SDK 초기화가 완료된 후 호출되는 콜백
        });
    }

    // 리워드형 인터스티셜 광고 로드
    public void LoadRewardedInterstitialAd()
    {
        // 새로운 광고를 로드하기 전에 기존 광고를 정리
        if (_rewardedInterstitialAd != null)
        {
            _rewardedInterstitialAd.Destroy();
            _rewardedInterstitialAd = null;
        }

        Debug.Log("리워드형 인터스티셜 광고를 로드합니다.");

        // 광고 요청 객체 생성
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        // 광고 로드 요청
        RewardedInterstitialAd.Load(AD_UNIT_ID, adRequest,
            (RewardedInterstitialAd ad, LoadAdError error) =>
            {
                // 오류가 발생했는지 확인
                if (error != null || ad == null)
                {
                    Debug.LogError("리워드형 인터스티셜 광고 로드 실패, 오류: " + error);
                    return;
                }

                Debug.Log("리워드형 인터스티셜 광고 로드 성공: " + ad.GetResponseInfo());
                _rewardedInterstitialAd = ad;
            });
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
        userDataRef.Child("gameData").Child("lastLogoutTime").SetValueAsync(nowUnixTime);
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
        userDataRef.Child("gameData").Child("lastLogoutTime").GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    long lastLogoutUnix = Convert.ToInt64(task.Result.Value);

                    // 실제 경과 시간 계산
                    calculatedSeconds = nowUnix - lastLogoutUnix;
                    _rewardCanvas.UpdateSlider();
                    Debug.Log($"[오프라인 보상] 마지막 로그아웃 시간: {lastLogoutUnix}, 현재 시간: {nowUnix}, 경과 시간: {calculatedSeconds}초");

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
        Debug.Log("[오프라인 보상] 광고 시청을 선택했습니다.");
       /* ShowRewardedInterstitialAd();*/
       _admobManager.ShowAd();
    }

    /// <summary>
    /// 실제 골드 보상 지급 및 UI 알림
    /// </summary>
    public void  GiveReward(int rewardAmount)
    {
        GameManager.Instance.Gold += (rewardAmount);
        Debug.Log($"[오프라인 보상] 최종 지급: {rewardAmount} 골드");
        _rewardCanvas.Button_Hide();
    }
}
