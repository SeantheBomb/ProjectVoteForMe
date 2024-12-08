using System;
using UnityEngine;

public class AmbientMusicController : MonoBehaviour
{

    public LevelMusicDataObject music;


    [SerializeField] private AudioSource audioSource1;
    [SerializeField] private AudioSource audioSource2;

    [SerializeField] private float fadeDuration = 2.0f; // Duration of the fade in/out

    private AudioSource activeAudioSource;
    private AudioSource inactiveAudioSource;

    private void Awake()
    {
        // Initialize the active and inactive audio sources
        activeAudioSource = audioSource1;
        inactiveAudioSource = audioSource2;

        // Ensure both audio sources start with zero volume
        audioSource1.volume = 0f;
        audioSource2.volume = 0f;

        // Ensure only one is playing initially
        //activeAudioSource.Play();
    }

    private void Start()
    {
        CanvasDialogController.OnDialogueStart += OnDialogStart;
        CanvasDialogController.OnDialogueEnd += OnDialogEnd;
        CanvasDialogController.OnDialogueFavor += OnDialogFavor;
        SetOverworldMusic();
    }

    private void OnDestroy()
    {
        CanvasDialogController.OnDialogueStart -= OnDialogStart;
        CanvasDialogController.OnDialogueEnd -= OnDialogEnd;
        CanvasDialogController.OnDialogueFavor -= OnDialogFavor;

    }

    private void OnDialogFavor(int favor)
    {
        if(favor > 0)
        {
            TransitionToTrack(music.positiveSentiment);
        }
        else if(favor < 0)
        {
            TransitionToTrack(music.negativeSentiment);
        }
        else
        {
            TransitionToTrack(music.neutralSentiment);
        }
    }

    void OnDialogStart()
    {
        SetDialogMusic();
    }

    void OnDialogEnd()
    {
        SetOverworldMusic();
    }

    public void SetOverworldMusic()
    {
        TransitionToTrack(music.overworldMusic);
    }

    public void SetDialogMusic()
    {
        TransitionToTrack(music.dialogMusic);
    }


    public void TransitionToTrack(AudioClip newClip)
    {
        Debug.Log("Transition to track " + newClip);

        // Assign the new clip to the inactive audio source
        inactiveAudioSource.clip = newClip;

        // Start crossfade coroutine
        StartCoroutine(CrossfadeTracks());
    }

    private System.Collections.IEnumerator CrossfadeTracks()
    {
        float timer = 0f;

        // Start the inactive source if not already playing
        if (!inactiveAudioSource.isPlaying)
        {
            inactiveAudioSource.Play();
        }

        // Perform the crossfade
        while (timer < fadeDuration)
        {
            float progress = timer / fadeDuration;
            activeAudioSource.volume = Mathf.Lerp(1f, 0f, progress);
            inactiveAudioSource.volume = Mathf.Lerp(0f, 1f, progress);

            timer += Time.deltaTime;
            yield return null;
        }

        // Ensure volumes are set correctly after fading
        activeAudioSource.volume = 0f;
        inactiveAudioSource.volume = 1f;

        // Stop the now inactive audio source
        activeAudioSource.Stop();

        // Swap active and inactive sources
        var temp = activeAudioSource;
        activeAudioSource = inactiveAudioSource;
        inactiveAudioSource = temp;
    }
}
