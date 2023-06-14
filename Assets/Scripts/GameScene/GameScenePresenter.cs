using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScenePresenter : PresenterSingleton<GameScenePresenter>
{
    [SerializeField] private GameSceneViewer gameSceneViewer;

    private int score = 0;

    private void Start()
    {
        Time.timeScale = 1;
        score = 0;
    }

    public void UpdateScore()
    {
        score += (ComboManager.GetInstance.HitCombo * 2);
        gameSceneViewer.UpdateScore(score);
    }

    public void UpdatePlayerHP(int hp)
    {
        gameSceneViewer.UpdateHP(hp);
        if (hp <= 0)
        {
            EnemySpawnManager.GetInstance.SpawnEnemyStop();
            gameSceneViewer.GameOver();
        }
    }
}
