using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{

    public TMP_InputField input;
    public Button button;


    private void Update()
    {
        button.interactable = input.text.Length > 2;
    }



    public void StartGame()
    {
        GameManager.Instance.CreateSession(input.text);
        SceneLoader.LoadNextLevelScene();
    }
}
