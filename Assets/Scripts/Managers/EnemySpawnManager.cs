using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using HughGeneric.Presenter;
using Hugh.PoolSystem;

public class EnemySpawnManager : PresenterSingleton<EnemySpawnManager>
{
    //[SerializeField] private GameObject basicCatPrefab;
    //[SerializeField] private GameObject rareCatPrefab;

    [SerializeField] private List<Transform> spawnPosList;

    private int SpawnObjectCount = 0;
    public bool IsSpawnCan { get; set; }

    private void Start()
    {
        IsSpawnCan = true;
        PoolManager.GetInstance.Pooling();
        SpawnObjectCount = PoolManager.GetInstance.PoolObjNameDict.Count;

        SpawnEnemyStart();
    }


    public void SpawnEnemyStart()
    {
        EnemySpawn().Forget();
    }

    public void SpawnEnemyStop()
    {
        IsSpawnCan = false;
        PoolManager.GetInstance.ResetPool();
    }

    private async UniTaskVoid EnemySpawn()
    {
        while (IsSpawnCan)
        {
            int spawnPosIndex = UnityEngine.Random.Range(5, spawnPosList.Count);
            for (int i = 0; i < spawnPosIndex; i++)
            {
                int randCats = UnityEngine.Random.Range(0, SpawnObjectCount);
                PoolManager.GetInstance.PoolObjNameDict.TryGetValue(randCats, out GameObject catName);
                GameObject obj = PoolManager.GetInstance.GetObject(catName);
                obj.transform.position = spawnPosList[i].position;
                obj.SetActive(true);
            }
            float spawnTime = UnityEngine.Random.Range(5.0f, 12.0f);
            await UniTask.Delay(TimeSpan.FromSeconds(spawnTime), cancellationToken: this.GetCancellationTokenOnDestroy());
        }
    }
}
