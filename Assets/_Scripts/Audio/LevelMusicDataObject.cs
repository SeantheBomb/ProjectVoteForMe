using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelMusic", menuName = "AssetData/Music")]
public class LevelMusicDataObject : ScriptableObject
{

    public AudioClip overworldMusic;
    public AudioClip dialogMusic;

    public AudioClip positiveSentiment;
    public AudioClip negativeSentiment;
    public AudioClip neutralSentiment;

}
