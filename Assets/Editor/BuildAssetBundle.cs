using UnityEditor;
using System.IO;

public class BuildAssetBundle
{
    [MenuItem("Assets/Build AssetBundles")]
    private static void BuildAssetBundles()
    {
        //string assetBundleDirectory = "Assets/AssetBundles";
        
        string assetBundleDirectory = "Assets/StreamingAssets"; //StreamingAsset에 두어야 android 빌드 시 로컬 데이터로 사용 가능
        if ( !Directory.Exists(assetBundleDirectory) )
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }

        BuildPipeline.BuildAssetBundles (assetBundleDirectory, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
        EditorUtility.DisplayDialog("에셋 번들 빌드", "에셋 번들 빌드를 완료했습니다", "빌드 완료");
    }
}
