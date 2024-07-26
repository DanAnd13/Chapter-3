using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class LoadAssetBundle : MonoBehaviour
{
    public string bundleName;
    public string assetName;
    private string _path;
    private AssetBundle _bundle;
    IEnumerator Start()
    {
        _path = System.IO.Path.Combine(Application.streamingAssetsPath, bundleName);
        Debug.Log("Loading AssetBundle from path: " + _path);

        if (!System.IO.File.Exists(_path))
        {
            Debug.LogError("AssetBundle file not found at path: " + _path);
            yield break;
        }

        AssetBundleCreateRequest bundleRequest = AssetBundle.LoadFromFileAsync(_path);
        yield return bundleRequest;

        _bundle = bundleRequest.assetBundle;
        if (_bundle == null)
        {
            Debug.LogError("Failed to load AssetBundle from path: " + _path);
            yield break;
        }
        Debug.Log("Successfully loaded AssetBundle: " + bundleName);

        AssetBundleRequest assetRequest = _bundle.LoadAssetAsync<GameObject>(assetName);
        yield return assetRequest;

        GameObject asset = assetRequest.asset as GameObject;
        if (asset != null)
        {
            Debug.Log("Successfully loaded asset: " + assetName);
            Instantiate(asset, Vector3.zero, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Failed to load asset from AssetBundle: " + assetName);
        }

        _bundle.Unload(false);
    }     
}
