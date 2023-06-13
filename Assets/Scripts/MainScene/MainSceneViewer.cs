using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;

public class MainSceneViewer : MonoBehaviour
{
    [SerializeField] private TMP_Text gameStartText;

    private void Start()
    {
        BlinkGameStartText().Forget();
    }

    private async UniTaskVoid BlinkGameStartText()
    {
        while (true)
        {
            gameStartText.alpha = 0.0f;
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: this.GetCancellationTokenOnDestroy());
            gameStartText.alpha = 1.0f;
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: this.GetCancellationTokenOnDestroy());
        }
    }

    public void GameStartBtn()
    {
        SceneController.GetInstance.LoadScene("Game");
    }
}
