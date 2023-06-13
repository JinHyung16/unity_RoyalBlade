using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject basicCatPrefab;
    [SerializeField] private GameObject rareCatPrefab;

    private Stack<GameObject> spawnObjStack;

    private IObjectPool<BasicCat> basicCatPool;
    private IObjectPool<RareCat> rareCatPool;

    [SerializeField] private Vector2 spawnPos;

    //UniTask token
    private CancellationTokenSource tokenSource;

    private void Start()
    {
        InitPool();

        if (tokenSource != null)
        {
            tokenSource.Cancel();
            tokenSource.Dispose();
        }
        tokenSource = new CancellationTokenSource();

        SpawnEnemyStart();
    }

    private void InitPool()
    {
        basicCatPool = new ObjectPool<BasicCat>(CreateBasicCat, OnGetBasicCat, OnReleaseBasicCat, OnDestroyBasicCat, maxSize: 10);
        rareCatPool = new ObjectPool<RareCat>(CreateRareCat, OnGetRareCat, OnReleaseRareCat, OnDestroyRareCat, maxSize: 10);
    }

    public void SpawnEnemyStart()
    {
        if (tokenSource != null)
        {
            tokenSource.Dispose();
        }
        tokenSource = new CancellationTokenSource();

        EnemySpawn().Forget();
    }

    public void SpawnEnemyStop()
    {
        tokenSource.Cancel();
    }

    private async UniTaskVoid EnemySpawn()
    {
        while (true)
        {
            for (int i = 0; i < 5; i++)
            {
                int randCats = UnityEngine.Random.Range(0, 2);
                if (randCats == 0)
                {
                    var enemy = basicCatPool.Get();
                    enemy.transform.position = spawnPos;
                }
                else
                {
                    var enemy = rareCatPool.Get();
                    enemy.transform.position = spawnPos;
                }
                Debug.Log(i);
            }
            float spawnTime = UnityEngine.Random.Range(5.0f, 10.0f);
            await UniTask.Delay(TimeSpan.FromSeconds(spawnTime), cancellationToken: tokenSource.Token);
        }
    }

    #region Basic Cat Object Pooling Functions
    private BasicCat CreateBasicCat()
    {
        BasicCat basicCat = Instantiate(basicCatPrefab).GetComponent<BasicCat>();
        basicCat.SetManagedPool(basicCatPool);
        return basicCat;
    }

    private void OnGetBasicCat(BasicCat basicCat)
    {
        basicCat.gameObject.SetActive(true);
    }

    private void OnReleaseBasicCat(BasicCat basicCat)
    {
        basicCat.gameObject.SetActive(false);
    }

    private void OnDestroyBasicCat(BasicCat basicCat)
    {
        Destroy(basicCat.gameObject);
    }
    #endregion

    #region Rare Cat Object Pooling Functions
    private RareCat CreateRareCat()
    {
        RareCat rareCat = Instantiate(rareCatPrefab).GetComponent<RareCat>();
        rareCat.SetManagedPool(rareCatPool);
        return rareCat;
    }

    private void OnGetRareCat(RareCat rareCat)
    {
        rareCat.gameObject.SetActive(true);
    }

    private void OnReleaseRareCat(RareCat rareCat)
    {
        rareCat.gameObject.SetActive(false);
    }

    private void OnDestroyRareCat(RareCat rareCat)
    {
        Destroy(rareCat.gameObject);
    }
    #endregion
}
