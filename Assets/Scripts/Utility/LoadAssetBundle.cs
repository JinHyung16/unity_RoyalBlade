using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Hugh.Utility
{
    public class LoadAssetBundle : MonoBehaviour
    {
        private List<GameObject> prefabs = new List<GameObject>();


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
            AssetBundle asset = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/StreamingAssets", loadBundle));
#elif UNITY_ANDROID
            AssetBundle asset = AssetBundle.LoadFromFile(Path.Combine("jar:file://" + Application.dataPath + "!/assets", loadBundle));
#endif
            if ( asset == null )
            {
                Debug.Log("에셋이 없다는데");
                yield break;
            }

            foreach ( var prefab in asset.LoadAllAssets<GameObject>() )
            {
                Debug.Log(prefab.name);
                prefabs.Add(prefab);
            }

            yield return new WaitUntil(() => isLoadDone == true);
            asset.Unload(true);
            Debug.Log("다 불러온 뒤: " + prefabs.Count);
        }
    }
}
