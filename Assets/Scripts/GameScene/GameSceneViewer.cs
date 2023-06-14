using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneViewer : MonoBehaviour
{
    [SerializeField] private List<Image> hpImages = new List<Image>();
    [SerializeField] private TMP_Text scoreTxt;

    //GameResult Canvas의 Text
    [SerializeField] private Canvas gameResultCanvas;
    [SerializeField] private TMP_Text gameResultText;
    [SerializeField] private GameObject clostBtn;

    private void Start()
    {
        scoreTxt.text = "";
        for (int i = 0; i < hpImages.Count; i++)
        {
            hpImages[i].enabled = true;
        }
        gameResultCanvas.enabled = false;
    }
    public void UpdateScore(int score)
    {
        scoreTxt.text = score.ToString();
    }

    public void UpdateHP(int index)
    {
        hpImages[index].enabled = false;
    }


    #region GameSceneViewer 하위 Button Event Functions
    /// <summary>
    /// Puase Button에 연결할 이벤트 함수
    /// </summary>
    public void OnPuaseButton()
    {
        clostBtn.SetActive(true);
        gameResultText.text = "Pause Game";
        gameResultCanvas.enabled = true;
        Time.timeScale = 0;
    }

    public void GameOver()
    {
        clostBtn.SetActive(false);
        gameResultText.text = "Game Over";
        gameResultCanvas.enabled = true;
        Time.timeScale = 0;
    }

    public void ReGame()
    {
        SceneController.GetInstance.LoadScene("Game");
    }

    public void ExitGame()
    {
        AudioManager.GetInstance.BGMStop();
        SceneController.GetInstance.LoadScene("Main");
    }

    public void CloseBtn()
    {
        gameResultCanvas.enabled = false;
        Time.timeScale = 1;
    }
    #endregion
}
