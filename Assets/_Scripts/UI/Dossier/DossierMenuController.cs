using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DossierMenuController : MonoBehaviour
{

    public GameObject rootPanel;
    public Transform navigationPanel;
    public DossierToggleButton[] toggle;
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
        foreach(var t in toggle)
            t.Setup(this);
        isInitialized = true;
    }

    public void Show()
    {
        bioDisplay.Hide();
        foreach (var t in toggle)
            t.SetShown();
        rootPanel.SetActive(true);
    }


    public void Hide()
    {
        foreach (var t in toggle)
            t.SetHidden();
        rootPanel.SetActive(false);
    }

    public void SelectCitizen(CitizenObject citizen)
    {
        bioDisplay.Show(citizen);
    }
}
