using Hugh.Utility;
using HughGeneric;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Hugh.PoolSystem
{
    public class PoolManager : Singleton<PoolManager>
    {
        [SerializeField] private LoadAssetBundle loadAssetBundle;
        private Dictionary<string, int> loadPrefabList = new Dictionary<string, int>(); //poolDictionary에 들어온 prefab의 이름에 맞춰 순서를 적어놓는다.

        private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

        private void Start()
        {
        }
        private void OnDisable()
        {
            loadAssetBundle.isLoadDone = true;
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
                loadPrefabList.Add(list[i].name, i);
                poolDictionary.Add(list[i].name, new Queue<GameObject>());
            }

            foreach ( var prefab in list )
            {
                GameObject obj = prefab;
                for ( int i = 0; i < poolCnt; i++ )
                {

                    Instantiate(obj);
                    obj.name = prefab.name;
                    poolDictionary[prefab.name].Enqueue(obj);
                    obj.SetActive(false);
                }
            }
        }

        private GameObject CreateNewObjecct(string name)
        {
            List<GameObject> list = loadAssetBundle.PrefabList;
            GameObject obj = Instantiate(list[loadPrefabList[name]]);
            obj.name = name;
            return obj;
        }

        public GameObject GetObject(string name)
        {
            if ( poolDictionary.TryGetValue(name, out Queue<GameObject> objQueue) )
            {
                if ( objQueue.Count < 1 )
                {
                    return CreateNewObjecct(name);
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
                return CreateNewObjecct(name);
            }
        }

        public void ReturnObject(GameObject obj)
        {
            if ( poolDictionary.TryGetValue(obj.name, out Queue<GameObject> objQueue) )
            {
                objQueue.Enqueue(obj);
            }
            else
            {
                Queue<GameObject> newObjQueue = new Queue<GameObject>();
                newObjQueue.Enqueue(obj);
                poolDictionary.Add(obj.name, newObjQueue);
            }

            obj.SetActive(false);
        }

    }
}
