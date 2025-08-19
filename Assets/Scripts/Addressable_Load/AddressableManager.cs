using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

/// �ε��� ������ ������� ���� �� Release����� �޸� �Ƴ� �� ����
/// ReleaseInstance() <-> InstantiateAsync
/// ReleaseAsset() <-> LoadAssetAsync
public class AddressableManager : MonoBehaviour
{
    [SerializeField] GameObject _prefab;
    [SerializeField] Material _material;
    [SerializeField] AudioClip _audioClip;
    [SerializeField] Dictionary<string, GameObject> _prefabDict = new Dictionary<string, GameObject>();

    private void Test()
    {
        // prefab = Resources.Load<GameObject>("Prefabs/Player");
        _prefab = Addressables.LoadAssetAsync<GameObject>("Playe").WaitForCompletion();
        Addressables.LoadAssetAsync<GameObject>("Playe").Completed += OnCompleted;
    }


    // ����: private Action OnCompleted()
    // ����: Completed �̺�Ʈ �ڵ鷯�� Action<AsyncOperationHandle<GameObject>> �ñ״�ó���� ��
    private void OnCompleted(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
        {
            // ���������� �ε��
            GameObject loadedObj = handle.Result;
            // �ʿ��� ó�� �ۼ�
        }
        else
        {
            // �ε� ���� ó��
            Debug.LogError("Addressable �ε� ����: " + handle.OperationException);
        }
    }

    //=====================================================================================================================
    // ���� �׽�Ʈ�� �ڵ�
    //=====================================================================================================================


    [SerializeField] AssetReferenceGameObject[] _gameObjs;

   /* private AssetReferenceT<AudioClip> SoundBGM;
    private GameObject BGMobj;

    private AssetReferenceSprite FlagSprite;
    private Image flagImage;*/


    private List<GameObject> gameObjects = new List<GameObject>();

    // DownManager���� �ε� �س���
    // Star���� �ٷ� ����
    private void Start()
    {
        Button_SpawnObj();
    }
  
    public void Button_SpawnObj()
    {
       /* _gameObj.InstantiateAsync().Completed += (obj) =>
        {
            gameObjects.Add(obj.Result);
        };*/

        for (int i = 0; i < _gameObjs.Length; i++)
        {
            _gameObjs[i].InstantiateAsync().Completed += (obj) =>
            {
                gameObjects.Add(obj.Result);
            };
        }
      

        /*// ���ӿ�����Ʈ ���°� �ƴ϶��
        SoundBGM.LoadAssetAsync().Completed += (clip) =>
        {
            AudioSource bgmSound = BGMobj.GetComponent<AudioSource>();
            bgmSound.clip = clip.Result;
            bgmSound.loop = true;
            bgmSound.Play();
        };

        FlagSprite.LoadAssetAsync().Completed += (img) =>
        {
            Image image = flagImage.GetComponent<Image>();
            image.sprite = img.Result;
        };*/
    }

   /* public void Button_ReleaseObj()
    {
       *//* SoundBGM.ReleaseAsset();
        FlagSprite.ReleaseAsset();*//*

        if (gameObjects.Count == 0)
            return;

        var index = gameObjects.Count - 1;
        Addressables.ReleaseInstance(gameObjects[index]);
        gameObjects.RemoveAt(index);
    }*/
}
