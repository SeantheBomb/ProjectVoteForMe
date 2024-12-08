using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenBehaviour : MonoBehaviour
{

    public CitizenObject citizen;

    public GameObject Avatar;

    public AudioClip knock;

    public AudioSource audioSource;


    private void Start()
    {
        Avatar.SetActive(false);
        //CanvasDialogController.OnDialogueStart += OnDialogStart;
    }


    public void StartDialog()
    {
        Avatar.SetActive(true);
        audioSource.PlayOneShot(knock);
        //AudioSource.PlayClipAtPoint(knock, transform.position);
    }

    public void EndDialog()
    {
        Avatar?.SetActive(false);
    }


}
