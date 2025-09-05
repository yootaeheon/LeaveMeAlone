using UnityEditor;
using UnityEngine;

public class BuildAutomator : MonoBehaviour
{
    [MenuItem("Build/Build Android (AAB)")]
    public static void Build()
    {
        string[] scenes = new string[] {
            "Assets/Scenes/LoginScene.unity",
            "Assets/Scenes/Loby.unity",
            "Assets/Scenes/Down.unity",
            "Assets/Scenes/Loading.unity",
            "Assets/Scenes/GameScene.unity"
        };

        // 번들 버전 코드 +1
        // 플레이스토어 버전업을 위한 기능
        //PlayerSettings.Android.bundleVersionCode++;

        // AAB 빌드 옵션
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = scenes;
        buildPlayerOptions.locationPathName = "Builds/LeaveMeAloneTest.aab";
        buildPlayerOptions.target = BuildTarget.Android;
        buildPlayerOptions.options = BuildOptions.None;

        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }
}
