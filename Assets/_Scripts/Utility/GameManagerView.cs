using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerView : MonoBehaviour
{

    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
    }

    //[ContextMenu("Save")]
    //public void Save()
    //{
    //    GameManager.Instance.Save();
    //}

    //[ContextMenu("Load")]
    //public void Load()
    //{
    //    GameManager.Instance.Load();
    //}
}
