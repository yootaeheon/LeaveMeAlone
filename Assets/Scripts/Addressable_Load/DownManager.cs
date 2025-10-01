using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class DownManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject WaitMessage;
    public GameObject DownMessage;

    public Slider DownSlider;
    public TMP_Text SizeInfoText;
    public TMP_Text DownValueText;
    public TMP_Text ButtonText;

    [Header("Label")]
    public AssetLabelReference DefaultLabel;

    private long patchSize;
    private long downloadedSize; // 진행률 계산용

    private void Start()
    {
        WaitMessage.SetActive(true);
        DownMessage.SetActive(false);

        StartCoroutine(InitAndCheck());
    }

    IEnumerator InitAndCheck()
    {
        // Addressables 초기화
        var init = Addressables.InitializeAsync();
        yield return init;

        // 1) 카탈로그 업데이트 확인
        var checkCatalogs = Addressables.CheckForCatalogUpdates(false);
        yield return checkCatalogs;

        if (checkCatalogs.Status == AsyncOperationStatus.Succeeded && checkCatalogs.Result.Count > 0)
        {
            var updateHandle = Addressables.UpdateCatalogs(checkCatalogs.Result);
            yield return updateHandle;
        }

        // 2) 다운로드 필요 용량 확인
        var sizeHandle = Addressables.GetDownloadSizeAsync(DefaultLabel.labelString);
        yield return sizeHandle;

        patchSize = sizeHandle.Result;

        if (patchSize > 0 && patchSize > 1024) // 1KB 이상일 때만 다운로드창 표시
        {
            WaitMessage.SetActive(false);
            DownMessage.SetActive(true);
            SizeInfoText.text = GetFileSize(patchSize);
        }
        else
        {
            DownValueText.text = "100 %";
            DownSlider.value = 1f;
            ButtonText.text = "GameStart";
            SizeInfoText.gameObject.SetActive(false);
            yield return new WaitForSeconds(1f);

            LoadingManager.LoadScene("GameScene"); // ? 프로젝트 전용 로딩 매니저 사용
        }
    }

    private string GetFileSize(long byteCnt)
    {
        if (byteCnt >= 1073741824.0)
            return $"{(byteCnt / 1073741824.0):F2} GB";
        if (byteCnt >= 1048576.0)
            return $"{(byteCnt / 1048576.0):F2} MB";
        if (byteCnt >= 1024.0)
            return $"{(byteCnt / 1024.0):F2} KB";
        if (byteCnt > 0)
            return byteCnt + " Bytes";

        return "0 Bytes";
    }

    #region Download
    public void Button_DownLoad()
    {
        StartCoroutine(PatchFiles());
    }

    IEnumerator PatchFiles()
    {
        var handle = Addressables.DownloadDependenciesAsync(DefaultLabel.labelString, false);

        while (!handle.IsDone)
        {
            var status = handle.GetDownloadStatus();
            downloadedSize = status.DownloadedBytes;

            DownSlider.value = (float)downloadedSize / patchSize;
            DownValueText.text = (int)((downloadedSize / (float)patchSize) * 100) + " %";

            yield return null;
        }

        DownSlider.value = 1f;
        DownValueText.text = "100 %";

        Addressables.Release(handle);

        LoadingManager.LoadScene("GameScene"); // ? 다운로드 완료 후 로딩 매니저 호출
    }
    #endregion
}


/*using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DownManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject WaitMessage;
    public GameObject DownMessage;

    public Slider DownSlider;
    public TMP_Text SizeInfoText;
    public TMP_Text DownValueText;

    [Header("Label")]
    public AssetLabelReference DefaultLabel;

    private long patchSize;
    private long downloadedSize; // 진행률 계산용

    private void Start()
    {
        WaitMessage.SetActive(true);
        DownMessage.SetActive(false);

        StartCoroutine(InitAddressable());
        StartCoroutine(CheckUpdateFiles());
    }

    IEnumerator InitAddressable()
    {
        var init = Addressables.InitializeAsync();
        yield return init;
    }

    #region Check Down
    IEnumerator CheckUpdateFiles()
    {
        patchSize = 0;

        var handle = Addressables.GetDownloadSizeAsync(DefaultLabel.labelString);
        yield return handle;

        patchSize = handle.Result;

        if (patchSize > 0 && patchSize > 1024) // 1KB 이상일 때만 다운로드창 표시
        {
            WaitMessage.SetActive(false);
            DownMessage.SetActive(true);
            SizeInfoText.text = GefFileSize(patchSize);
        }
        else
        {
            DownValueText.text = "100 %";
            DownSlider.value = 1f;
            yield return new WaitForSeconds(2f);
            *//*LoadingManager.LoadScene("GameScene");*//*
            SceneManager.LoadScene("GameScene");
        }
    }

    private string GefFileSize(long byteCnt)
    {
        string size = "0 Bytes";

        if (byteCnt >= 1073741824.0)
            size = $"{(byteCnt / 1073741824.0):F2} GB";
        else if (byteCnt >= 1048576.0)
            size = $"{(byteCnt / 1048576.0):F2} MB";
        else if (byteCnt >= 1024.0)
            size = $"{(byteCnt / 1024.0):F2} KB";
        else if (byteCnt > 0)
            size = byteCnt + " Bytes";

        return size;
    }
    #endregion

    #region Download
    public void Button_DownLoad()
    {
        StartCoroutine(PatchFiles());
    }

    IEnumerator PatchFiles()
    {
        var handle = Addressables.DownloadDependenciesAsync(DefaultLabel.labelString, false);

        while (!handle.IsDone)
        {
            var status = handle.GetDownloadStatus();
            downloadedSize = status.DownloadedBytes;

            DownSlider.value = (float)downloadedSize / patchSize;
            DownValueText.text = (int)((downloadedSize / (float)patchSize) * 100) + " %";

            yield return null;
        }

        DownSlider.value = 1f;
        DownValueText.text = "100 %";

        Addressables.Release(handle);

        LoadingManager.LoadScene("GameScene");
    }
    #endregion
}*/