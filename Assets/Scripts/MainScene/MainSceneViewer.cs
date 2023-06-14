using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using TMPro;
using UnityEngine;

public class MainSceneViewer : MonoBehaviour
{
    [SerializeField] private TMP_Text gameStartText;

    private CancellationTokenSource tokenSource;
    private void Start()
    {
        Time.timeScale = 1;

        if (tokenSource != null)
        {
            tokenSource.Dispose();
        }
        tokenSource = new CancellationTokenSource();

        BlinkGameStartText().Forget();
    }

    private void OnDisable()
    {
        tokenSource.Cancel();
    }

    private void OnDestroy()
    {
        tokenSource.Dispose();
    }

    private async UniTaskVoid BlinkGameStartText()
    {
        while (true)
        {
            gameStartText.alpha = 0.0f;
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: tokenSource.Token);
            gameStartText.alpha = 1.0f;
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: tokenSource.Token);
        }
    }

    public void GameStartBtn()
    {
        SceneController.GetInstance.LoadScene("Game");
    }
}
