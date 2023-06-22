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
        protected override void OnAwake()
        {
            //base.OnAwake();
            Pooling();
        }

        private Dictionary<PoolableType, Stack<PoolObject>> poolDictionary = new Dictionary<PoolableType, Stack<PoolObject>>();

        /// <summary>
        /// Dictionary내 PoolabeType의 개수에 맞춰 초기화해서 공간을 확보해둔다.
        /// </summary>
        private void Pooling()
        {
            foreach ( PoolableType type in Enum.GetValues(typeof(PoolableType)) )
            {
                poolDictionary.Add(type, new Stack<PoolObject>());
            }

        }

        private PoolObject CreatePoolObject(PoolableType tpye, string name)
        {
            GameObject obj = Resources.Load(PrefabPath(tpye) + name, typeof(GameObject)) as GameObject;

            if ( obj == null )
            {
                return null;
            }

            obj = Instantiate(obj);

            if ( obj.TryGetComponent<PoolObject>(out PoolObject poolObj) )
            {
                poolObj.Name = name;
                return poolObj;
            }
            else
            {
                poolObj = obj.AddComponent<PoolObject>();
                poolObj.Name = name;
                return poolObj;
            }
        }

        /// <summary>
        /// Pooling할 오브젝트를 프리팹화 시켜 Resources 폴더 내 저장해둔 위치를 읽어온다.
        /// </summary>
        /// <param name="type"> pooling할 오브젝트 타입</param>
        /// <returns> Resources폴더 내 해당 prefab 경로 반환</returns>
        private string PrefabPath(PoolableType type)
        {
            switch ( type )
            {
                case PoolableType.None:
                    break;
                case PoolableType.BasicCat:
                    return "Prefab/Enemy/";
                case PoolableType.RareCat:
                    return "Prefab/Enemy/";
            }
            return "Prefab/";
        }

        public GameObject GetPrefab(PoolableType type, string name)
        {

            return null;
        }

        public void DespawnObject(PoolableType _type, GameObject obj)
        {
            if ( obj.TryGetComponent<PoolObject>(out PoolObject poolObj) )
            {
            }
        }

    }

    public enum PoolableType
    {
        None = 0,

        BasicCat,
        RareCat,
    }
}
