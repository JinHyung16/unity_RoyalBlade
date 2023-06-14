using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private PlayerBehaviourController playerController;
    [SerializeField] private PlayerAudioController playAudioController;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            playAudioController.PlayDefenseSFX();
            ComboManager.GetInstance.ResetCombo();
            playerController.OnBounceByDefend();
            playerController.isDefend = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            playerController.isDefend = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            playerController.isDefend = false;
        }
    }
}
