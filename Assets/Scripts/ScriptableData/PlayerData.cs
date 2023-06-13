using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData", order = 0)]
public class PlayerData : ScriptableObject
{
    public int characterHP;
    public int characterPower;

    public float jumpPower;
    public float jumpSpecialPower;
    public float bouncePower;

    [NonSerialized] public int playerHpRuntime;

    [NonSerialized] public int playerPowerRuntime;

    public void OnAfterDeserialize()
    {
        playerHpRuntime = characterHP;
        playerPowerRuntime = characterPower;
    }

    public void OnBeforeSerialize() { }
}