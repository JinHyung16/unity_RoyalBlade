using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Hugh.Utility
{
    public class LoadAssetBundle : MonoBehaviour
    {
        private List<GameObject> prefabs = new List<GameObject>();
        private AssetBundle asset;

        [HideInInspector] public bool isLoadDone = false;

        private string loadBundle;
        public List<GameObject> PrefabList
        {
            get
            {
                return this.prefabs;
            }

        }

        private void Awake()
        {
            LoadBundleFromLocalAsync("cat");
            Debug.Log("Awake: Load Asset Bundle");
        }

        public void LoadBundleFromLocalAsync(string bundleName)
        {
            if ( 0 < prefabs.Count )
            {
                prefabs.Clear();
            }
            isLoadDone = false;
            this.loadBundle = bundleName;
            StartCoroutine(LoadBundleFromLocal());
        }


        private IEnumerator LoadBundleFromLocal()
        {
#if UNITY_EDITOR || UNITY_EDITOR_WIN
            asset = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/StreamingAssets", loadBundle));
#elif UNITY_ANDROID
            asset = AssetBundle.LoadFromFile(Path.Combine("jar:file://" + Application.dataPath + "!/assets", loadBundle));
#endif
            if ( asset == null )
            {
                Debug.Log("Load해온 에셋이 없다.");
                yield break;
            }

            foreach ( var prefab in asset.LoadAllAssets<GameObject>() )
            {
                Debug.Log(prefab.name);
                prefabs.Add(prefab);
            }

            yield return new WaitUntil(() => isLoadDone == true);
            asset.Unload(true);
            Debug.Log("Unload하여 정리 완료 ");
        }
    }
}
