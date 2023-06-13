using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "RareCatData", menuName = "RareCatData", order = 2)]
public class RareCatData : ScriptableObject
{
    public int rareCatHp;

    [NonSerialized]
    public int enemyHpRuntime;

    public void OnAfterDeserialize()
    {
        enemyHpRuntime = rareCatHp;
    }

    public void OnBeforeSerialize() { }
}
