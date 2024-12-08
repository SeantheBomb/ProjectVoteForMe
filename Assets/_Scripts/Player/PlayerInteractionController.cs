using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{

    public CanvasDialogController canvas;
    public GameObject interactPreview;
    public float range;
    bool isCitizenInRange;
    List<CitizenBehaviour> citizensInRange;
    CitizenBehaviour currentCitizen;


    // Start is called before the first frame update
    void Start()
    {
        citizensInRange = new List<CitizenBehaviour>();
        if(canvas == null)canvas = FindObjectOfType<CanvasDialogController>(true);
        interactPreview.SetActive(false);
        CanvasDialogController.OnDialogueEnd += OnDialogEnd;
    }

    private void OnDestroy()
    {
        CanvasDialogController.OnDialogueEnd -= OnDialogEnd;
    }

    private void OnDialogEnd()
    {
        if (currentCitizen != null)
            currentCitizen.EndDialog();
    }

    private void FixedUpdate()
    {
        UpdateCitizenInRange();
        interactPreview.SetActive(isCitizenInRange);
        if(citizensInRange.Contains(currentCitizen) == false)
        {
            canvas.Complete();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartInteraction();
        }
    }

    void StartInteraction()
    {
        if (canvas.gameObject.activeSelf)
            return;
        if(citizensInRange.Count == 0)
        {
            return;
        }
        currentCitizen = citizensInRange.First();
        canvas.Show(currentCitizen.citizen);
        currentCitizen.StartDialog();
    }


    void UpdateCitizenInRange()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range);
        citizensInRange.Clear();
        foreach(var c in colliders)
        {
            if(c.TryGetComponent(out CitizenBehaviour citizen))
            {
                citizensInRange.Add(citizen);
            }
        }
        isCitizenInRange = citizensInRange.Count > 0;
    }
}
