using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DossierBioController : MonoBehaviour
{

    public Image portrait;
    public TMP_Text title;
    public TMP_Text description;
    public TMP_Text dossier;



    public void Show(CitizenObject citizen)
    {
        if (citizen == null)
        {
            Hide();
            return;
        }
        portrait.sprite = PortraitLoader.GetPortrait(citizen.bio.portrait);
        title.text = citizen.bio.name;
        description.text = citizen.bio.description;
        dossier.text = citizen.bio.dossier;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
