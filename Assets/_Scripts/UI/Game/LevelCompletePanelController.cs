using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelCompletePanelController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CitizenCanvasSession.OnSessionComplete += OnSessionComplete;
        Hide();
    }

    private void OnDestroy()
    {
        CitizenCanvasSession.OnSessionComplete -= OnSessionComplete;
    }

    private void OnSessionComplete(CitizenCanvasSession session)
    {
        if(GameManager.CurrentLevel.citizenSessions.Count >= GameManager.CurrentSession.CurrentCohort.citizens.Count)
        {
            Show();
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void ReturnToMacro()
    {
        SceneLoader.LoadNextLevelScene();
    }
}
