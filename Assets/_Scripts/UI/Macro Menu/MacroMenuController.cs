using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MacroMenuController : MonoBehaviour
{

    public PollingResults[] StartingResults = new PollingResults[]
    {
        new PollingResults(20,20),
        new PollingResults (10,10),
        new PollingResults(30,30)
    };

    public PollingResults[] GetCurrentResults()
    {
        LevelSession[] levels = GameManager.CurrentSession.levelSessions.ToArray();
        PollingResults[] results = new PollingResults[StartingResults.Length];
        float consistencyScore = GameManager.CurrentSession.GetConsistensyScore();
        for (int i = 0; i < results.Length; i++)
        {
            if(i < levels.Length)
            {
                results[i] = new PollingResults(StartingResults[i], levels[i].citizenSessions);
                //results[i].playerTally = Mathf.RoundToInt(results[i].playerTally * consistencyScore);
            }
            else
            {
                results[i] = StartingResults[i];
            }
        }
        return results;
    }

    public PollingResults GetTotalResults()
    {
        int playerTally = 0;
        int oppositionTally = 0;
        foreach(var r in GetCurrentResults())
        {
            playerTally += r.playerTally;
            oppositionTally += r.opposingTally;
        }
        return new PollingResults(playerTally, oppositionTally);
    }


    public void GoToNextLevel()
    {
        SceneLoader.LoadLevel(GameManager.CurrentSession.StartNewLevel());
    }

    public void GoToMainMenu()
    {
        SceneLoader.LoadMainMenu();
    }
}


[System.Serializable]
public class PollingResults
{
    public int playerTally, opposingTally;

    public PollingResults(int playerTally, int opposingTally)
    {
        this.playerTally = playerTally;
        this.opposingTally = opposingTally;
    }

    public PollingResults(PollingResults start, List<CitizenCanvasSession> sessions)
    {
        this.playerTally = start.playerTally;
        this.opposingTally= start.opposingTally;
        playerTally += sessions.Count((s) => s.IsFavored());
        opposingTally += sessions.Count((s) => s.IsFavored() == false);
    }
}
