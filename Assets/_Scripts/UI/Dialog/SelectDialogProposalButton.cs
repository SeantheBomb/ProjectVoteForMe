using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectDialogProposalButton : MonoBehaviour
{
    Button button;
    TMP_Text text;

    public ProposalOption proposal;


    public void Setup(SelectDialogController controller)
    {
        button = GetComponent<Button>();
        text = GetComponentInChildren<TMP_Text>();
        controller.OnProposalSelected += OnProposalSelected;
        button.onClick.AddListener(() => controller.SelectProposal(this));
    }

    public void Show(ProposalOption proposal)
    {
        button.interactable = true;
        this.proposal = proposal;
        text.text = proposal.title;
    }

    void OnProposalSelected(SelectDialogProposalButton proposal)
    {
        button.interactable = proposal != this;
    }
}
