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


    // Start is called before the first frame update
    void Start()
    {
        citizensInRange = new List<CitizenBehaviour>();
        if(canvas == null)canvas = FindObjectOfType<CanvasDialogController>();
        interactPreview.SetActive(false);
    }

    private void FixedUpdate()
    {
        UpdateCitizenInRange();
        interactPreview.SetActive(isCitizenInRange);
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
        if(citizensInRange.Count == 0)
        {
            return;
        }

        canvas.Show(citizensInRange.First().citizen);
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
