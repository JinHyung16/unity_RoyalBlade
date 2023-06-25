using Hugh.Utility;
using HughGeneric.Presenter;
using System.Collections.Generic;
using UnityEngine;

namespace Hugh.PoolSystem
{
    public class PoolManager : PresenterSingleton<PoolManager>
    {
        [SerializeField] private LoadAssetBundle loadAssetBundle;

        private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

        private Dictionary<int, GameObject> poolObjNameDict = new Dictionary<int, GameObject>();
        public Dictionary<int, GameObject> PoolObjNameDict
        {
            get
            {
                return this.poolObjNameDict;
            }
        }

        protected override void OnAwake()
        {
            poolObjNameDict = new Dictionary<int, GameObject>();

        }

        public void LoadPrefabsWhereBundle(string bundleName)
        {
            loadAssetBundle.LoadBundleFromLocalAsync(bundleName);
        }

        public void Pooling(int poolCnt = 30)
        {
            List<GameObject> list = loadAssetBundle.PrefabList;
            if ( list.Count < 1 || list == null )
            {
                Debug.Log("불러온 에셋 리스트가 없습니다.");
                return;
            }

            for ( int i = 0; i < list.Count; i++ )
            {
                poolObjNameDict.Add(i, list[i]);
                poolDictionary.Add(list[i].name, new Queue<GameObject>());
            }

            foreach ( var prefab in list )
            {
                for ( int i = 0; i < poolCnt; i++ )
                {
                    GameObject obj = Instantiate(prefab);
                    obj.name = prefab.name;
                    poolDictionary[prefab.name].Enqueue(obj);
                    obj.SetActive(false);
                }
            }
        }

        private GameObject CreateNewObjecct(GameObject gameObject)
        {
            GameObject obj = Instantiate(gameObject);
            obj.name = gameObject.name;
            poolDictionary[obj.name].Enqueue(obj);
            return obj;
        }

        public GameObject GetObject(GameObject gameObject)
        {
            if ( poolDictionary.TryGetValue(gameObject.name, out Queue<GameObject> objQueue) )
            {
                if ( objQueue.Count < 1 )
                {
                    return CreateNewObjecct(gameObject);
                }
                else
                {
                    GameObject obj = objQueue.Dequeue();
                    obj.SetActive(true);
                    return obj;
                }
            }
            else
            {
                return CreateNewObjecct(gameObject);
            }
        }

        public void ReturnObject(GameObject gameObject)
        {
            if ( poolDictionary.TryGetValue(gameObject.name, out Queue<GameObject> objQueue) )
            {
                objQueue.Enqueue(gameObject);
            }
            else
            {
                Queue<GameObject> newObjQueue = new Queue<GameObject>();
                newObjQueue.Enqueue(gameObject);
                poolDictionary.Add(gameObject.name, newObjQueue);
            }

            gameObject.SetActive(false);
        }

        public void ResetPool()
        {
            loadAssetBundle.isLoadDone = true;

            poolDictionary.Clear();
            poolObjNameDict.Clear();

            poolDictionary = null;
            poolObjNameDict = null;
        }

    }
}
