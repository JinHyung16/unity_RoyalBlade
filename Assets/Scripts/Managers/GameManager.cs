using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HughGeneric;
public class GameManager : Singleton<GameManager>
{

    [SerializeField] private GameObject quitPanel;
    [SerializeField] private GameObject pausePanel;

    private void Start()
    {
        quitPanel.SetActive(false);
        pausePanel.SetActive(false);
    }

    private void Update()
    {
        MobileInput();
    }

    private void MobileInput()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Home))
            {
                PauseGame();
            }
            if (Input.GetKeyDown(KeyCode.Menu))
            {
                PauseGame();
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                QuitGame();
            }
        }
    }

    private void QuitGame()
    {
        if (pausePanel.activeSelf)
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            quitPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void PauseGame()
    {
        if (quitPanel.activeSelf)
        {
            quitPanel.SetActive(false);
        }
        pausePanel.SetActive(true);
        Time.timeScale = 0;

    }

    #region Button Functions
    public void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void CacellQuit()
    {
        quitPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void OnPlay()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }
#endregion
}
