using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadAssetBundle : MonoBehaviour
{
    [SerializeField] private string bundlePath = "jar:file://" + Application.dataPath + "!/assets/";
    [SerializeField] private int version = 1;

    private string loadBundleName = "cat";

    private Dictionary<string, GameObject> catBundleDictionary = new Dictionary<string, GameObject>();

    public void LoadFromLocal()
    {
        var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, loadBundleName));

        if ( myLoadedAssetBundle == null )
        {
            Debug.Log("Failed to load AssetBundle!");
            return;
        }

        foreach ( var prefab in myLoadedAssetBundle.LoadAllAssets<GameObject>() )
        {
            catBundleDictionary[prefab.name] = prefab;
        }
    }
}
