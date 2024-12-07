using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DossierToggleButton : MonoBehaviour
{

    DossierMenuController menu;

    Button button;
    TMP_Text text;

    bool isShown;

    public void Setup(DossierMenuController menu)
    {
        button = GetComponent<Button>();
        text = GetComponentInChildren<TMP_Text>();

        button.onClick.AddListener(ToggleMenu);

        this.menu = menu;
    }

    public void ToggleMenu()
    {
        if(isShown)
        {
            menu.Hide();
        }
        else
        {
            menu.Show();
        }
    }


    public void SetShown()
    {
        text.text = "Hide Dossier";
        isShown = true;
    }

    public void SetHidden()
    {
        text.text = "Show Dossier";
        isShown = false;
    }
}
