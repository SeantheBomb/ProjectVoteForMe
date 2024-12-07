using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    
    static SceneLoader _instance;
    public static SceneLoader Instance
    {
        get
        {
            if(_instance == null)
            {
                GameObject go = new GameObject("[SceneLoader]");
                _instance = go.AddComponent<SceneLoader>();
                _instance.data = Resources.Load<SceneLoaderData>("Data/SceneLoaderData");
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    SceneLoaderData data;


    public static void LoadMainMenu()
    {
        SceneManager.LoadScene(Instance.data.MainMenuScene);
    }

    public static void LoadNextLevelScene()
    {
        SceneManager.LoadScene(Instance.data.NextLevelScene);
    }

    public static void LoadLevel(int level)
    {
        SceneManager.LoadScene(Instance.data.levelScenes[level]);
    }

}
