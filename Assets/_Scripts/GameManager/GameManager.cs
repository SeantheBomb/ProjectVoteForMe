using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class GameManager
{ 

    static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null) _instance = new GameManager();
            return _instance;
        }
    }

    GameSession _currentSession;

    public static GameSession CurrentSession
    {
        get
        {
            if(Instance._currentSession == null)
                Instance._currentSession = new GameSession();
            return Instance._currentSession;
        }
    }

    public static LevelSession CurrentLevel => CurrentSession.CurrentLevel;


    static LevelConfig _config;

    public static LevelConfig Config
    {
        get
        {
            if (_config == null) _config = Resources.Load<LevelConfig>("Data/LevelConfig");
            return _config;
        }
    }

    public void CreateSession()
    {
        _currentSession = new GameSession();
    }

    //public void Load()
    //{
    //    if (_currentSession != null)
    //    {
    //        return;
    //    }

    //    string json = PlayerPrefs.GetString("GameSession", null);
    //    if (string.IsNullOrWhiteSpace(json))
    //    {
    //        CreateSession();
    //    }
    //    else
    //    {
    //        _currentSession = GameSession.LoadSave(json);
    //    }
    //}

    //public void Save()
    //{
    //    if(_currentSession == null)
    //    {
    //        return;
    //    }

    //    PlayerPrefs.SetString("GameSession", _currentSession.Save());
    //}

    //public static string[] GetSaveSlots(int count = 3)
    //{
    //    string[] slots = new string[count];
    //    for (int i = 0; i < count; i++)
    //    {
    //        string 
    //    }
    //}

}



[System.Serializable]
public class GameSession
{

    //public DateTime creationDate;

    public List<LevelSession> levelSessions;
    public List<ProposalHistory> proposalHistory;

    int _currentLevelIndex = -1;
    public LevelSession CurrentLevel
    {
        get
        {
            return levelSessions[_currentLevelIndex];
        }
    }

    public int CurrentLevelIndex => _currentLevelIndex;

    public CitizenCohortObject CurrentCohort
    {
        get
        {
            return GameManager.Config.levels[_currentLevelIndex].cohort;
        }
    }

    //public int saveSlot;

    public GameSession()
    {
        levelSessions = new List<LevelSession>();
        proposalHistory = new List<ProposalHistory>();
        //creationDate = DateTime.Now;
        //this.saveSlot = saveSlot;
    }

    public int StartNewLevel()
    {
        levelSessions.Add(new LevelSession());
        _currentLevelIndex += 1;
        return _currentLevelIndex;
    }

    public void StartLevel(int index)
    {
        for (int i = levelSessions.Count; i <= index; i++)
        {
            levelSessions.Add(new LevelSession());
        }
        _currentLevelIndex = index;
    }

    //public static GameSession LoadSave(string json)
    //{
    //    return JsonUtility.FromJson<GameSession>(json);
    //}

    //public string Save()
    //{
    //    string json = JsonUtility.ToJson(this);
    //    Debug.Log($"GameManager: Saving session\n{json}");
    //    return json;
    //}

    public void AddProposalOption(PlayerProposalObject proposal, ProposalOption option)
    {
        ProposalHistory history = proposalHistory.Where((p) => p.proposal.Equals(proposal.id)).FirstOrDefault();
        if(history == default)
        {
            history = new ProposalHistory();
            history.proposal = proposal.id;
            proposalHistory.Add(history);
        }
        history.AddOptionUse(option);
    }


}

[System.Serializable]
public class LevelSession
{
    public List<CitizenCanvasSession> citizenSessions;

    public LevelSession()
    {
        citizenSessions = new List<CitizenCanvasSession>();
    }

}

[System.Serializable]
public class ProposalHistory
{
    public string proposal;
    public List<ProposalHistoryOption> options;


    public ProposalHistory()
    {
        options = new List<ProposalHistoryOption>();
    }

    public float GetConsistencyScore()
    {
        ProposalHistoryOption maxOption = options.OrderByDescending((o)=>o.count).FirstOrDefault();
        int total = options.Sum((o)=>o.count);
        return maxOption.count / total;
    }

    public void AddOptionUse(ProposalOption option)
    {
        ProposalHistoryOption history = options.Where((o)=>o.option == option.id).FirstOrDefault();
        if(history == default)
        {
            history = new ProposalHistoryOption();
            history.option = option.id;
            options.Add(history);
        }
        history.count += 1;
        Debug.Log($"GameManager: Track use of option {option.id} in proposal {proposal}");
    }
}

[System.Serializable]
public class ProposalHistoryOption
{
    public string option;
    public int count;
}
