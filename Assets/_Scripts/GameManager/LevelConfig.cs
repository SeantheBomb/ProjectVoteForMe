using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "LevelConfig", menuName = "GameData/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    public List<LevelData> levels;
}


[System.Serializable]
public class LevelData
{
    public CitizenCohortObject cohort;
}
