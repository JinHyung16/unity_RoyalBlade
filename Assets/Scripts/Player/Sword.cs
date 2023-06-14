using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;

    private float swordPowerMultiple = 1.0f;

    private void Start()
    {
        playerData.OnAfterDeserializePower();
        swordPowerMultiple = UnityEngine.Random.Range(0.5f, 100.0f);
        playerData.playerPowerRuntime = Mathf.RoundToInt(playerData.playerPowerRuntime * swordPowerMultiple);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AttackDamageCacul();

        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<BaseEnemy>().OnDamge(playerData.playerPowerRuntime);
            ComboManager.GetInstance.SetHitCombo();
        }
    }

    private void AttackDamageCacul()
    {
        playerData.OnAfterDeserializePower();
        swordPowerMultiple = UnityEngine.Random.Range(1.0f, 100.0f);
        playerData.playerPowerRuntime = Mathf.RoundToInt(playerData.playerPowerRuntime * swordPowerMultiple);

    }
}
