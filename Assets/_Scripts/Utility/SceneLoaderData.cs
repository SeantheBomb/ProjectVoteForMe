using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SceneLoaderData", menuName = "GameData/SceneLoader")]
public class SceneLoaderData : ScriptableObject
{
    public string MainMenuScene;

    public string NextLevelScene;

    public string[] levelScenes;
}
