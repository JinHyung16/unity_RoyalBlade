using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class ComboManager : PresenterSingleton<ComboManager>
{

    [SerializeField] private TMP_Text comboText;

    public int HitCombo { get { return this.curHitCombo; } }
    private int curHitCombo = 0;

    private void Start()
    {
        comboText.enabled = false;
    }
    public void SetHitCombo()
    {
        curHitCombo++;
        ShowCombo().Forget();
    }
    private async UniTaskVoid ShowCombo()
    {
        comboText.enabled = true;
        comboText.text = curHitCombo.ToString() + " ÄÞº¸";
        await UniTask.Delay(TimeSpan.FromSeconds(0.4f), cancellationToken: this.GetCancellationTokenOnDestroy());
        comboText.enabled = false;
        await UniTask.Yield(this.GetCancellationTokenOnDestroy());
    }

    public void ResetCombo()
    {
        curHitCombo = 0;
    }
}
