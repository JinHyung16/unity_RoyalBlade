using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using HughGeneric.Presenter;
using Hugh.PoolSystem;

public class EnemySpawnManager : PresenterSingleton<EnemySpawnManager>
{
    [SerializeField] private GameObject basicCatPrefab;
    [SerializeField] private GameObject rareCatPrefab;

    private IObjectPool<BasicCat> basicCatPool;
    private IObjectPool<RareCat> rareCatPool;

    [SerializeField] private List<Transform> spawnPosList;

    public bool IsSpawnCan { get; set; }

    protected override void OnAwake()
    {
        InitPool();
    }

    private void Start()
    {
        IsSpawnCan = true;
        PoolManager.GetInstance.Pooling();
        //SpawnEnemyStart();
    }

    private void Update()
    {
        if ( Input.GetKeyDown(KeyCode.A) )
        {
            for ( int i = 0; i < 10; i++ )
            {
                GameObject obj = PoolManager.GetInstance.GetObject("BasicCat");
                obj.SetActive(true);
            }
        }
    }
    public void InitPool()
    {
        basicCatPool = new ObjectPool<BasicCat>(CreateBasicCat, OnGetBasicCat, OnReleaseBasicCat, OnDestroyBasicCat, maxSize: 100);
        rareCatPool = new ObjectPool<RareCat>(CreateRareCat, OnGetRareCat, OnReleaseRareCat, OnDestroyRareCat, maxSize: 100);
    }

    public void SpawnEnemyStart()
    {
        EnemySpawn().Forget();
    }

    public void SpawnEnemyStop()
    {
        IsSpawnCan = false;
    }

    private async UniTaskVoid EnemySpawn()
    {
        while (IsSpawnCan)
        {
            int spawningIndex = UnityEngine.Random.Range(5, spawnPosList.Count);
            for (int i = 0; i < spawningIndex; i++)
            {
                int randCats = UnityEngine.Random.Range(0, 2);
                if (randCats == 0)
                {
                    var enemy = basicCatPool.Get();
                    enemy.transform.position = spawnPosList[i].position;
                    enemy.gameObject.SetActive(true);
                }
                else
                {
                    var enemy = rareCatPool.Get();
                    enemy.transform.position = spawnPosList[i].position;
                    enemy.gameObject.SetActive(true);
                }
            }
            float spawnTime = UnityEngine.Random.Range(5.0f, 12.0f);
            await UniTask.Delay(TimeSpan.FromSeconds(spawnTime), cancellationToken: this.GetCancellationTokenOnDestroy());
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
        //Get()실행시 우선 꺼두고 위치 바꾸고 켜는 형식으로 변경
        //basicCat.gameObject.SetActive(true);
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
        //Get()실행시 우선 꺼두고 위치 바꾸고 켜는 형식으로 변경
        //rareCat.gameObject.SetActive(true);
        rareCat.gameObject.SetActive(false);
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
