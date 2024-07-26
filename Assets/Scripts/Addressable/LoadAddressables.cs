using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LoadAddressables : MonoBehaviour
{
    [SerializeField]
    private AssetReferenceGameObject _cube;
    [SerializeField]
    private AssetReferenceSprite _picture;

    private void Start()
    {
        StartCoroutine(LoadAndInstantiateCube());

        StartCoroutine(LoadAndInstantiatePicture());
    }

    private IEnumerator LoadAndInstantiateCube()
    {
        var handle = _cube.LoadAssetAsync<GameObject>();
        yield return handle;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject obj = handle.Result;
            Instantiate(obj, Vector3.zero, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Failed to load addressable asset: " + _cube.RuntimeKey);
        }
        Addressables.Release(handle);
    }

    private IEnumerator LoadAndInstantiatePicture()
    {
        var handle = _picture.LoadAssetAsync<Sprite>();
        yield return handle;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Sprite sprite = handle.Result;

            GameObject pictureObj = CreateAndSpawnPicture(sprite);
        }
        else
        {
            Debug.LogError("Failed to load addressable asset: " + _picture.RuntimeKey);
        }
        Addressables.Release(handle);
    }
    private GameObject CreateAndSpawnPicture(Sprite sprite)
    {
        GameObject pictureObj = new GameObject("Picture");
        SpriteRenderer renderer = pictureObj.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        pictureObj.transform.position = new Vector3(2, 0, 0);
        return pictureObj;
    }
}
