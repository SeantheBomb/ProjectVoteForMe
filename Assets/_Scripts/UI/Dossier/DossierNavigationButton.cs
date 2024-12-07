using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DossierNavigationButton : MonoBehaviour
{

    public CitizenObject citizen;

    public DossierMenuController menu;

    Button button;

    TMP_Text text;

    public Image passIcon, failIcon;


    public void Setup(CitizenObject citizen, DossierMenuController menu)
    {
        this.menu = menu;
        this.citizen = citizen;
        button = GetComponent<Button>();
        text = GetComponentInChildren<TMP_Text>();
        button.onClick.AddListener(Select);
        text.text = citizen.bio.name;
        passIcon.gameObject.SetActive(false);
        failIcon.gameObject.SetActive(false);
        CitizenCanvasSession.OnSessionComplete += OnSessionComplete;
    }

    private void OnDestroy()
    {
        CitizenCanvasSession.OnSessionComplete -= OnSessionComplete;
    }

    public void Select()
    {
        menu.SelectCitizen(citizen);
    }

    void OnSessionComplete(CitizenCanvasSession session)
    {
        if (session.citizen != this.citizen)
            return;

        bool result = session.IsFavored();
        passIcon.gameObject.SetActive(result);
        failIcon.gameObject.SetActive(!result);
    }

}
