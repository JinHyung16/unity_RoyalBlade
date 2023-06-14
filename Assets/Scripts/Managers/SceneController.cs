using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using HughGeneric;
public class SceneController : Singleton<SceneController>
{

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
