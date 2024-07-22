using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class LoadAssetBundle : MonoBehaviour
{
    public string bundleName;
    public string assetName;
    string path;
    AssetBundle bundle;
    IEnumerator Start()
    {
        path = System.IO.Path.Combine(Application.streamingAssetsPath, bundleName);
        Debug.Log("Loading AssetBundle from path: " + path);

        if (!System.IO.File.Exists(path))
        {
            Debug.LogError("AssetBundle file not found at path: " + path);
            yield break;
        }

        AssetBundleCreateRequest bundleRequest = AssetBundle.LoadFromFileAsync(path);
        yield return bundleRequest;

        bundle = bundleRequest.assetBundle;
        if (bundle == null)
        {
            Debug.LogError("Failed to load AssetBundle from path: " + path);
            yield break;
        }
        Debug.Log("Successfully loaded AssetBundle: " + bundleName);

        AssetBundleRequest assetRequest = bundle.LoadAssetAsync<GameObject>(assetName);
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

        bundle.Unload(false);
    }     
}
