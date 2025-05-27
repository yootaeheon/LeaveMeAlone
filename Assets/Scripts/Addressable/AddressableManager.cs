using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class AddressableManager : MonoBehaviour
{

    [SerializeField] AssetReferenceGameObject _characterObj;
    [SerializeField] AssetReferenceGameObject[] _monsterObjs;

   /* private AssetReferenceT<AudioClip> SoundBGM;
    private GameObject BGMobj;

    private AssetReferenceSprite FlagSprite;
    private Image flagImage;*/


    private List<GameObject> gameObjects = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(InitAddressable());
    }

    IEnumerator InitAddressable()
    {
        var init = Addressables.InitializeAsync();
        yield return init;
    }

    public void Button_SpawnObj()
    {
        _characterObj.InstantiateAsync().Completed += (obj) =>
        {
            gameObjects.Add(obj.Result);
        };

        for (int i = 0; i < _monsterObjs.Length; i++)
        {
            _monsterObjs[i].InstantiateAsync().Completed += (obj) =>
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

    public void Button_ReleaseObj()
    {
       /* SoundBGM.ReleaseAsset();
        FlagSprite.ReleaseAsset();*/

        if (gameObjects.Count == 0)
            return;

        var index = gameObjects.Count - 1;
        Addressables.ReleaseInstance(gameObjects[index]);
        gameObjects.RemoveAt(index);

    }

    // ReleaseInstance() <-> InstantiateAsync
    // ReleaseAsset() <-> LoadAssetAsync

}
