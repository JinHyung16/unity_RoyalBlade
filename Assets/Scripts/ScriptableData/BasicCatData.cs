using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName ="BasicCatData", menuName = "BasicCatData", order = 1)]
public class BasicCatData : ScriptableObject
{
    public float basicCatHP;

    [NonSerialized]
    public float enemyHpRuntime;

    public void OnAfterDeserialize()
    {
        enemyHpRuntime = basicCatHP;
    }

    public void OnBeforeSerialize() { }
}
