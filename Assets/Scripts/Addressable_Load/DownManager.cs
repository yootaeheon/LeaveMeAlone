using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
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

        if (patchSize > 0)
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
            LoadingManager.LoadScene("GameScene");
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
}




/*using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
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
   *//* public AssetLabelReference GameObjLabel;*//*
    public AssetLabelReference DefaultLabel;

    private long patchSize; // 패치 사이즈 크기는 Long

    private Dictionary<string, long> patchMap = new Dictionary<string, long>(); // Label과 pathchSize 관리용 Dictionary



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
        // 라벨을 담는 리스트 생성
        List<string> labels = new List<string>() {*//* GameObjLabel.labelString,*//* DefaultLabel.labelString };

        // patchSize 변수 초기화
        patchSize = default;

        // 라벨 리스트 순회하며 다운로드사이즈 받아서 handle에 저장
        foreach (string label in labels)
        {
            var handle = Addressables.GetDownloadSizeAsync(label); // 메서드 결과값이 byte 형태이기 때문에 patchSize도 byte 형태

            yield return handle; // 결과가 저장될때까지 대기 

            patchSize += handle.Result;
        }

        // 패치사이즈가 0보다 크면 업데이트 파일이 존재하는 상황
        if (patchSize > decimal.Zero)
        {
            WaitMessage.SetActive(false);
            DownMessage.SetActive(true);

            SizeInfoText.text = GefFileSize(patchSize); // sizeInfoText에 패치사이즈 넘겨주어 표시
        }
        // 다운받을 필요 없는 상황
        else
        {
            DownValueText.text = "100 %";
            DownSlider.value = 1f;
            yield return new WaitForSeconds(2f);
            LoadingManager.LoadScene("GameScene");
        }
    }

    // 메서드 결과값이 byte 형태이기 때문에 patchSize도 byte 형태
    // size 출력 포맷 분류
    private string GefFileSize(long byteCnt)
    {
        string size = "0 Bytes";

        // 10억(1GB, 1,073,741,824) 이상,
        if (byteCnt >= 1073741824.0)
        {
            size = string.Format($"{0:##.##}", byteCnt / 1073741824.0 + " GB");
        }
        // 1MB 이상,
        else if (byteCnt >= 1048576.0)
        {
            size = string.Format($"{0:##.##}", byteCnt / 1048576.0 + " MB");
        }
        // 1KB 이상,
        else if (byteCnt >= 1024.0)
        {
            size = string.Format($"{0:##.##}", byteCnt / 1024.0 + " KB");
        }
        else if (byteCnt > 0 && byteCnt < 1024.0)
        {
            size = byteCnt.ToString() + " Bytes";
        }

        return size;
    }
    #endregion

    #region
  
    public void Button_DownLoad()
    {
        StartCoroutine(PatchFiles());
    }

    /// <summary>
    /// 버튼 눌러 파일 패치 시작하는 코루틴
    /// </summary>
    IEnumerator PatchFiles()
    {
        // 라벨을 담는 리스트 생성
        List<string> labels = new List<string>() { *//*GameObjLabel.labelString, *//*DefaultLabel.labelString };

        // 라벨 리스트 순회하며 다운로드사이즈 받아서 handle에 저장
        foreach (string label in labels)
        {
            var handle = Addressables.GetDownloadSizeAsync(label); // 메서드 결과값이 byte 형태이기 때문에 patchSize도 byte 형태

            yield return handle; // 결과가 저장될때까지 대기 

            // handle의 값이 0이 아니라면 라벨에 있는 것을 다운로드
            if (handle.Result != decimal.Zero)
            {
                StartCoroutine(DownLoadLabel(label));
            }
        }

        // 다운 진행되는 동안 UI에 진행사항을 표시
        yield return CheckDownLoad();
    }

    IEnumerator DownLoadLabel(string label)
    {
        // patchMap 딕셔너리에 인자값 label을 키로하고 초기값 0을 추가
        // 이 변수는 각 label에 대한 다운로드 상태를 저장하는데 사용
        if (!patchMap.ContainsKey(label))
        {
            patchMap.Add(label, 0);
        }

        var handle = Addressables.DownloadDependenciesAsync(label, false);

        while (!handle.IsDone)
        {
            // patchMap에 다운로드 상태의 DownLoadBytes를 저장
            patchMap[label] = handle.GetDownloadStatus().DownloadedBytes;

            // 한 프레임씩 대기하면서 반복문이 돌게하여 너무 많은 연산을 하지 않게 함
            yield return new WaitForEndOfFrame();
        }

        // 다운로드 작업이 완료되면 다운로드 상태의 TotalBytesfmf PatchMap에 대입
        patchMap[label] = handle.GetDownloadStatus().TotalBytes;
        // 다운로드 작업의 핸들을 해제
        Addressables.Release(handle);
    }

    IEnumerator CheckDownLoad()
    {
        float total = 0f;
        DownValueText.text = "0 %";

        // total에 patchMap의 모든 값을 더해줌
        // 총합을 표시함
        while (true)
        {
            total += patchMap.Sum(temp => temp.Value);

            DownSlider.value = total / patchSize;
            DownValueText.text = (int)((total / patchSize) * 100) + " %";

            // 모든 다운로드 완료 시
            if (total == patchSize)
            {
                LoadingManager.LoadScene("GameScene");
                break;
            }

            total = 0f;
            yield return new WaitForEndOfFrame();
        }
    }


    #endregion

}
*/
