using Firebase.Database;
using UnityEngine;

public class ProgressSaver : MonoBehaviour
{
    public ProgressSO progressSO;

    public void SaveProgressToFirebase()
    {
        string uid = Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser?.UserId;
        if (string.IsNullOrEmpty(uid)) return;

        ProgressDataDTO data = new ProgressDataDTO(
            progressSO.Chapter,
            progressSO.Stage,
            progressSO.KillCount
        );

        string json = JsonUtility.ToJson(data);
        FirebaseDatabase.DefaultInstance
            .RootReference
            .Child("users")
            .Child(uid)
            .Child("_progressData")
            .SetRawJsonValueAsync(json)
            .ContinueWith(task =>
            {
                if (task.IsCompleted)
                    Debug.Log("진행도 저장 완료!");
                else
                    Debug.LogWarning("진행도 저장 실패: " + task.Exception);
            });
    }
}
