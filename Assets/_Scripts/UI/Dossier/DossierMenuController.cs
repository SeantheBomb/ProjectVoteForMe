using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DossierMenuController : MonoBehaviour
{

    public GameObject rootPanel;
    public Transform navigationPanel;
    public DossierToggleButton toggle;
    public DossierNavigationButton navPrefab;
    public DossierBioController bioDisplay;

    public CitizenCohortObject cohort;

    bool isInitialized;


    // Start is called before the first frame update
    void Start()
    {
        cohort = GameManager.CurrentSession.CurrentCohort;
        Initialize(cohort);
        Hide();
    }

    void Initialize(CitizenCohortObject cohort)
    {
        if(isInitialized) return;

        this.cohort = cohort;

        foreach(var citizen in cohort.citizens)
        {
            DossierNavigationButton navButton = Instantiate(navPrefab, navigationPanel);
            navButton.Setup(citizen, this);
        }
        toggle.Setup(this);
        isInitialized = true;
    }

    public void Show()
    {
        bioDisplay.Hide();
        toggle.SetShown();
        rootPanel.SetActive(true);
    }


    public void Hide()
    {
        toggle.SetHidden();
        rootPanel.SetActive(false);
    }

    public void SelectCitizen(CitizenObject citizen)
    {
        bioDisplay.Show(citizen);
    }
}
