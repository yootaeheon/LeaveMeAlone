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

        // ���� ���� �ڵ� +1
        // �÷��̽���� �������� ���� ���
        //PlayerSettings.Android.bundleVersionCode++;

        // AAB ���� �ɼ�
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = scenes;
        buildPlayerOptions.locationPathName = "Builds/LeaveMeAloneTest.aab";
        buildPlayerOptions.target = BuildTarget.Android;
        buildPlayerOptions.options = BuildOptions.None;

        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }
}
