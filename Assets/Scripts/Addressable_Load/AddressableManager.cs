using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class AddressableManager : MonoBehaviour
{
    [SerializeField] GameObject _prefab;
    [SerializeField] Material _material;
    [SerializeField] AudioClip _audioClip;
    [SerializeField] Dictionary<string, GameObject> _prefabDict = new Dictionary<string, GameObject>();

    public static AddressableManager _instance;
    public static AddressableManager Instance { get { return _instance; } set { _instance = value; } }

    [SerializeField] private List<AssetLabelReference> _label;
    private List<string> _labels;
    private long _downSize;
    private Dictionary<string, long> _patchMap = new Dictionary<string, long>();

    [SerializeField] private float _delayToStartCheckDownLoad;
    [SerializeField] private float _delayTofinishDownLoad;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            Init();
            DontDestroyOnLoad(gameObject);
            Debug.Log("어드레서블 초기화!");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitAddressableAsync().Forget();
    }

    private void Init()
    {
        _labels = new List<string>();
        for (int i = 0; i < _label.Count; i++)
        {
            _labels.Add(_label[i].labelString);
        }
    }

    // 어드레서블 초기화 (UniTask)
    private async UniTaskVoid InitAddressableAsync()
    {
        var init = Addressables.InitializeAsync();
        await init.ToUniTask();
        Debug.Log("어드레서블 초기화 완료");
    }

    // 단순 Object 생성
    public async UniTask<GameObject> GetObjectAsync(AssetReferenceGameObject assetObject)
    {
        var handle = assetObject.InstantiateAsync();
        await handle.ToUniTask();
        return handle.Result;
    }

    // 단순 Object 생성 후 List에 저장
    public async UniTask GetObjectAndSaveAsync(AssetReferenceGameObject assetObject, List<GameObject> realObjects)
    {
        var handle = assetObject.InstantiateAsync();
        await handle.ToUniTask();
        realObjects.Add(handle.Result);
    }

    // List에 저장된 Object들 생성 후 List에 저장
    public async UniTask GetObjectsAndSaveAsync(List<AssetReferenceGameObject> assetObjects, List<GameObject> realObjects)
    {
        foreach (var assetObject in assetObjects)
        {
            var handle = assetObject.InstantiateAsync();
            await handle.ToUniTask();
            realObjects.Add(handle.Result);
        }
    }

    // Sound 가져오기
    public async UniTask LoadSoundAsync(AssetReferenceT<AudioClip> assetAudioClip, AudioSource audio)
    {
        var handle = assetAudioClip.LoadAssetAsync();
        await handle.ToUniTask();
        audio.clip = handle.Result;
    }

    // Sprite 가져와서 이미지에 참조
    public async UniTask LoadSpriteAsync(AssetReferenceSprite assetImageSprite, Image image)
    {
        var handle = assetImageSprite.LoadAssetAsync();
        await handle.ToUniTask();
        image.sprite = handle.Result;
        image.gameObject.SetActive(true);
    }

    // Sprite 가져와서 Sprite에 참조
    public async UniTask<Sprite> LoadOnlySpriteAsync(AssetReferenceSprite assetImageSprite)
    {
        var handle = assetImageSprite.LoadAssetAsync();
        await handle.ToUniTask();
        return handle.Result;
    }

    // 가져온 에셋 해제
    public void ReleaseObject(AssetReference asset)
    {
        asset.ReleaseAsset();
    }

    // 생성한 에셋 해제
    public void ReleaseInstance(GameObject assetObjects)
    {
        Addressables.ReleaseInstance(assetObjects);
    }

    public void ReleaseInstances(List<GameObject> assetObjects)
    {
        for (int i = assetObjects.Count; i > 0; i--)
        {
            Addressables.ReleaseInstance(assetObjects[i - 1]);
            assetObjects.RemoveAt(i - 1);
        }
    }

    // 다운받을 파일 여부 확인
    public async UniTask<long> CheckDownLoadFileAsync()
    {
        _downSize = 0;
        await UniTask.Delay(TimeSpan.FromSeconds(_delayToStartCheckDownLoad));

        foreach (string label in _labels)
        {
            var handle = Addressables.GetDownloadSizeAsync(label);
            await handle.ToUniTask();
            _downSize += handle.Result;
        }
        return _downSize;
    }

    // 파일 사이즈 단위 변환
    public StringBuilder GetFileSize(long byteCnt)
    {
        StringBuilder sb = new StringBuilder();
        Debug.Log($"총 사이즈: {byteCnt}");

        if ((byteCnt >= 1073741824.0))
        {
            sb.Append(string.Format("{0: ##.##}", byteCnt / 1073741824.0));
            sb.Append("Gb");
        }
        else if ((byteCnt >= 1048576.0))
        {
            sb.Append(string.Format("{0: ##.##}", byteCnt / 1048576.0));
            sb.Append("Mb");
        }
        else if ((byteCnt >= 1024.0))
        {
            sb.Append(string.Format("{0: ##.##}", byteCnt / 1024.0));
            sb.Append("Kb");
        }
        else if ((byteCnt > 0 && byteCnt < 1024.0))
        {
            sb.Append(byteCnt.ToString());
            sb.Append("Bytes");
        }
        return sb;
    }

    // 다운로드 시작
    public async UniTask DownLoadAsync(Slider downPercentSlider, TextMeshProUGUI downPercentText, Action<bool> callback)
    {
        foreach (string label in _labels)
        {
            var handle = Addressables.GetDownloadSizeAsync(label);
            await handle.ToUniTask();

            if (handle.Result != decimal.Zero)
            {
                await OnDownLoadPerLabelAsync(label);
            }
        }
        await OnCheckDownLoadStatusAsync(downPercentSlider, downPercentText, callback);
    }

    // 어드레서블 라벨 별로 다운로드 받기
    private async UniTask OnDownLoadPerLabelAsync(string label)
    {
        _patchMap.Add(label, 0);
        var handle = Addressables.DownloadDependenciesAsync(label, false);

        while (!handle.IsDone)
        {
            _patchMap[label] = handle.GetDownloadStatus().DownloadedBytes;
            await UniTask.Yield();
        }

        _patchMap[label] = handle.GetDownloadStatus().TotalBytes;
        Addressables.Release(handle);
        Debug.Log("하나의 Label 다운끝!");
    }

    // 현재 다운로드 상황 알려주기
    private async UniTask OnCheckDownLoadStatusAsync(Slider downPercentSlider, TextMeshProUGUI downPercentText, Action<bool> finishDownLoadCallback)
    {
        StringBuilder sb = new StringBuilder();
        long total = 0;
        bool isFinishDownLad;

        while (true)
        {
            total += _patchMap.Sum(tmp => tmp.Value);
            downPercentSlider.value = (float)total / (float)_downSize;

            int curPatchValue = (int)(downPercentSlider.value * 100);
            sb.Clear();
            sb.Append(curPatchValue);
            sb.Append(" %");
            downPercentText.SetText(sb);

            Debug.Log($"check 중! 현재 {downPercentSlider.value}%, {total}Size만큼 다운받음");

            if (total == _downSize)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_delayTofinishDownLoad));
                isFinishDownLad = true;
                finishDownLoadCallback(isFinishDownLad);
                Debug.Log("다운로드 끝!");
                break;
            }

            total = 0;
            await UniTask.Yield();
        }
    }
}
