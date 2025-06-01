using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public static string nextScene;

    public Slider loadingBar;

    private void Start()
    {
        StartCoroutine(StartLoadingScene());
    }

    /// <summary>
    /// 로딩 시작 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator StartLoadingScene()
    {
        yield return null;

        // nextScene에 저장된 이름의 씬을 비동기로 로딩
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false; // 씬 전환을 수동으로 하기 위함

        float timer = 0;

        while (!op.isDone)
        {
            yield return null;

            timer += Time.deltaTime;

            // 로딩 진행중
            if (op.progress < 0.9f)
            {
                // 러프 사용하여 부드럽게 로딩 표시 증가
                loadingBar.value = Mathf.Lerp(loadingBar.value, op.progress, timer);

                if (loadingBar.value >= op.progress)
                {
                    timer = 0f;
                }
            }
            // 로딩 완료 시
            else
            {
                loadingBar.value = Mathf.Lerp(loadingBar.value, 1f, timer);

                if (loadingBar.value == 1f)
                {
                    yield return new WaitForSeconds(2f);
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("Loading");
    }
}
