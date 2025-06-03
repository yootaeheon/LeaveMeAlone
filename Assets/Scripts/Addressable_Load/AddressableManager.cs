using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

/// 로드한 에셋은 사용하지 않을 시 Release해줘야 메모리 아낄 수 있음
/// ReleaseInstance() <-> InstantiateAsync
/// ReleaseAsset() <-> LoadAssetAsync
public class AddressableManager : MonoBehaviour
{
    [SerializeField] AssetReferenceGameObject[] _gameObjs;

   /* private AssetReferenceT<AudioClip> SoundBGM;
    private GameObject BGMobj;

    private AssetReferenceSprite FlagSprite;
    private Image flagImage;*/


    private List<GameObject> gameObjects = new List<GameObject>();

    // DownManager에서 로딩 해놓고
    // Star에서 바로 스폰
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
      

        /*// 게임오브젝트 형태가 아니라면
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
