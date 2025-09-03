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
    private long downloadedSize; // ����� ����

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

    private long patchSize; // ��ġ ������ ũ��� Long

    private Dictionary<string, long> patchMap = new Dictionary<string, long>(); // Label�� pathchSize ������ Dictionary



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
        // ���� ��� ����Ʈ ����
        List<string> labels = new List<string>() {*//* GameObjLabel.labelString,*//* DefaultLabel.labelString };

        // patchSize ���� �ʱ�ȭ
        patchSize = default;

        // �� ����Ʈ ��ȸ�ϸ� �ٿ�ε������ �޾Ƽ� handle�� ����
        foreach (string label in labels)
        {
            var handle = Addressables.GetDownloadSizeAsync(label); // �޼��� ������� byte �����̱� ������ patchSize�� byte ����

            yield return handle; // ����� ����ɶ����� ��� 

            patchSize += handle.Result;
        }

        // ��ġ����� 0���� ũ�� ������Ʈ ������ �����ϴ� ��Ȳ
        if (patchSize > decimal.Zero)
        {
            WaitMessage.SetActive(false);
            DownMessage.SetActive(true);

            SizeInfoText.text = GefFileSize(patchSize); // sizeInfoText�� ��ġ������ �Ѱ��־� ǥ��
        }
        // �ٿ���� �ʿ� ���� ��Ȳ
        else
        {
            DownValueText.text = "100 %";
            DownSlider.value = 1f;
            yield return new WaitForSeconds(2f);
            LoadingManager.LoadScene("GameScene");
        }
    }

    // �޼��� ������� byte �����̱� ������ patchSize�� byte ����
    // size ��� ���� �з�
    private string GefFileSize(long byteCnt)
    {
        string size = "0 Bytes";

        // 10��(1GB, 1,073,741,824) �̻�,
        if (byteCnt >= 1073741824.0)
        {
            size = string.Format($"{0:##.##}", byteCnt / 1073741824.0 + " GB");
        }
        // 1MB �̻�,
        else if (byteCnt >= 1048576.0)
        {
            size = string.Format($"{0:##.##}", byteCnt / 1048576.0 + " MB");
        }
        // 1KB �̻�,
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
    /// ��ư ���� ���� ��ġ �����ϴ� �ڷ�ƾ
    /// </summary>
    IEnumerator PatchFiles()
    {
        // ���� ��� ����Ʈ ����
        List<string> labels = new List<string>() { *//*GameObjLabel.labelString, *//*DefaultLabel.labelString };

        // �� ����Ʈ ��ȸ�ϸ� �ٿ�ε������ �޾Ƽ� handle�� ����
        foreach (string label in labels)
        {
            var handle = Addressables.GetDownloadSizeAsync(label); // �޼��� ������� byte �����̱� ������ patchSize�� byte ����

            yield return handle; // ����� ����ɶ����� ��� 

            // handle�� ���� 0�� �ƴ϶�� �󺧿� �ִ� ���� �ٿ�ε�
            if (handle.Result != decimal.Zero)
            {
                StartCoroutine(DownLoadLabel(label));
            }
        }

        // �ٿ� ����Ǵ� ���� UI�� ��������� ǥ��
        yield return CheckDownLoad();
    }

    IEnumerator DownLoadLabel(string label)
    {
        // patchMap ��ųʸ��� ���ڰ� label�� Ű���ϰ� �ʱⰪ 0�� �߰�
        // �� ������ �� label�� ���� �ٿ�ε� ���¸� �����ϴµ� ���
        if (!patchMap.ContainsKey(label))
        {
            patchMap.Add(label, 0);
        }

        var handle = Addressables.DownloadDependenciesAsync(label, false);

        while (!handle.IsDone)
        {
            // patchMap�� �ٿ�ε� ������ DownLoadBytes�� ����
            patchMap[label] = handle.GetDownloadStatus().DownloadedBytes;

            // �� �����Ӿ� ����ϸ鼭 �ݺ����� �����Ͽ� �ʹ� ���� ������ ���� �ʰ� ��
            yield return new WaitForEndOfFrame();
        }

        // �ٿ�ε� �۾��� �Ϸ�Ǹ� �ٿ�ε� ������ TotalBytesfmf PatchMap�� ����
        patchMap[label] = handle.GetDownloadStatus().TotalBytes;
        // �ٿ�ε� �۾��� �ڵ��� ����
        Addressables.Release(handle);
    }

    IEnumerator CheckDownLoad()
    {
        float total = 0f;
        DownValueText.text = "0 %";

        // total�� patchMap�� ��� ���� ������
        // ������ ǥ����
        while (true)
        {
            total += patchMap.Sum(temp => temp.Value);

            DownSlider.value = total / patchSize;
            DownValueText.text = (int)((total / patchSize) * 100) + " %";

            // ��� �ٿ�ε� �Ϸ� ��
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
