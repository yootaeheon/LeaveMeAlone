/*using System;
using UnityEngine;
using Firebase.Auth;
using Firebase.Database;

// 광고 SDK (예: Unity Ads 사용 시)
using UnityEngine.Advertisements;

public class OfflineRewardManager : MonoBehaviour
{
    private DatabaseReference dbRef;              // Firebase Realtime Database 참조
    private int goldPerSecond = 10;               // 초당 보상 골드
    private readonly long maxRewardSeconds = 21600; // 최대 보상 가능 시간: 6시간 = 21,600초

    private long calculatedSeconds = 0;           // 실제 경과 시간 (초)
    private int baseReward = 0;                   // 기본 보상량

    void Start()
    {
        // Firebase Database 초기화
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;

        // 로그인된 유저일 경우 오프라인 보상 체크
        if (FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            CheckOfflineReward();
        }
    }

    // 앱이 백그라운드로 전환될 때 로그아웃 시간 저장
    void OnApplicationPause(bool pause)
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
    /// </summary>
    void SaveLogoutTime()
    {
        string uid = FirebaseAuth.DefaultInstance.CurrentUser?.UserId;
        if (string.IsNullOrEmpty(uid)) return;

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
                    UIManager.Instance.ShowOfflineRewardPopup(baseReward, OnAdWatchSelected, OnAdDeclined);
                }
                else
                {
                    Debug.Log("[오프라인 보상] 로그아웃 기록이 없거나 데이터를 불러오지 못했습니다.");
                }
            });
    }

    /// <summary>
    /// 사용자가 광고 시청을 하지 않고 그냥 보상을 받을 경우
    /// </summary>
    void OnAdDeclined()
    {
        GiveReward(baseReward);
    }

    /// <summary>
    /// 사용자가 광고 시청을 선택했을 경우
    /// </summary>
    void OnAdWatchSelected()
    {
        if (Advertisement.IsReady("rewardedVideo"))
        {
            Advertisement.Show("rewardedVideo", new ShowOptions
            {
                resultCallback = HandleAdResult
            });
        }
        else
        {
            Debug.Log("[광고] 광고가 준비되지 않았습니다. 기본 보상을 지급합니다.");
            GiveReward(baseReward);
        }
    }

    /// <summary>
    /// 광고 시청 결과에 따라 보상 처리
    /// </summary>
    void HandleAdResult(ShowResult result)
    {
        if (result == ShowResult.Finished)
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

    /// <summary>
    /// 실제 골드 보상 지급 및 UI 알림
    /// </summary>
    void GiveReward(int rewardAmount)
    {
        GameManager.Instance.AddGold(rewardAmount);                        // 골드 증가
        UIManager.Instance.ShowFinalRewardToast(rewardAmount);            // UI로 보상 알림 표시
        Debug.Log($"[오프라인 보상] 최종 지급: {rewardAmount} 골드");
    }

    public void ShowOfflineRewardPopup(int baseReward, Action onAdWatch, Action onDecline)
    {
        // 예시:
        // "6시간 동안 3600 골드를 벌었습니다!"
        // [광고 보고 2배 받기] → onAdWatch()
        // [그냥 받기] → onDecline()
    }

    // 최종 보상 지급 후 토스트 메시지 또는 팝업
    public void ShowFinalRewardToast(int reward)
    {
        // 예시: "총 7200 골드를 획득했습니다!"
    }
}
*/