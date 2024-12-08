using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class MacroMenuMusicController : MonoBehaviour
{

    public AudioClip winningTrack, losingTrack;

    public AudioSource source;

    public MacroMenuController macro;



    // Start is called before the first frame update
    void Start()
    {
        if (macro.IsWinning() || GameManager.CurrentSession.CurrentLevelIndex == -1)
        {
            source.clip = winningTrack;
        }
        else
        {
            source.clip = losingTrack;
        }
        source.loop = true;
        source.Play();
    }

    
}
