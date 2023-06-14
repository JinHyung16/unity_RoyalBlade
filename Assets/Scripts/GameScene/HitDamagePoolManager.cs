using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using HughGeneric.Presenter;
public class HitDamagePoolManager : PresenterSingleton<HitDamagePoolManager>
{
    //HitDamageText Pool 관련 데이터
    [SerializeField] private GameObject hitDamagedTextPrefab;
    private IObjectPool<HitDamageText> hitDamangePool;

    private void Start()
    {
        InitPool();
    }

    private void InitPool()
    {
        hitDamangePool = new ObjectPool<HitDamageText>(CreateHitDamageText, OnGetHitDamageText, OnReleaseHitDamageText, OnDestroyHitDamageText, true, 10);
    }

    public void GetHitDamageText(Transform hitPos, string damage)
    {
        var hitDamgeText = hitDamangePool.Get();
        hitDamgeText.SetDamageText(damage);
        hitDamgeText.HitDamageMove(hitPos).Forget();
    }

    #region HitDamagedText Prefab Pooling
    private HitDamageText CreateHitDamageText()
    {
        HitDamageText hitDamaged = Instantiate(hitDamagedTextPrefab).GetComponent<HitDamageText>();
        hitDamaged.SetManagedPool(hitDamangePool);
        return hitDamaged;
    }

    private void OnGetHitDamageText(HitDamageText hitDamaged)
    {
        hitDamaged.gameObject.SetActive(true);
    }

    private void OnReleaseHitDamageText(HitDamageText hitDamaged)
    {
        hitDamaged.gameObject.SetActive(false);
    }

    private void OnDestroyHitDamageText(HitDamageText hitDamaged)
    {
        Destroy(hitDamaged.gameObject);
    }
    #endregion
}
