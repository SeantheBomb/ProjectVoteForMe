using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLevel : MonoBehaviour
{

    public int level;

    // Start is called before the first frame update
    void Awake()
    {
        if(GameManager.CurrentSession.CurrentLevelIndex == -1)
        {
            GameManager.CurrentSession.StartLevel(level);
        }
    }

}
