using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class HitDamageText : MonoBehaviour
{
    [SerializeField] private Transform hitDmgTxtTransform;
    [SerializeField] private TMP_Text damageText;
    private IObjectPool<HitDamageText> managedPool;


    public async UniTaskVoid HitDamageMove(Transform hitPos)
    {
        float yAxis = hitPos.transform.position.y + 0.2f;
        await hitDmgTxtTransform.DOMoveY(yAxis, 0.3f).WithCancellation(cancellationToken: this.GetCancellationTokenOnDestroy());
        DestroyManagedPool();
    }

    public void SetDamageText(string damage)
    {
        damageText.text = damage;
    }

    #region UnityEngine.Pool Functions
    public void SetManagedPool(IObjectPool<HitDamageText> poolObj)
    {
        managedPool = poolObj;
    }

    public void DestroyManagedPool()
    {
        managedPool.Release(this);
    }
    #endregion
}
