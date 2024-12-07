using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectDialogController : MonoBehaviour
{

    public PlayerProposalObject proposal;

    public System.Action<SelectDialogProposalButton> OnProposalSelected;
    public System.Action<SelectDialogSentimentButton> OnSentimentSelected;

    public System.Action<ProposalOption, ProposalSentiment> OnProposalSubmitted;

    public SelectDialogProposalButton[] proposalButtons;
    public SelectDialogSentimentButton[] sentimentButtons;
    public Button submitButton;
    public TMP_Text header;

    public bool IsSubmitted;

    bool hasOption, hasSentiment;

    public ProposalOption option;
    public ProposalSentiment sentiment;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        proposalButtons = GetComponentsInChildren<SelectDialogProposalButton>();
        sentimentButtons = GetComponentsInChildren<SelectDialogSentimentButton>();

        for (int i = 0; i < sentimentButtons.Length; i++)
        {
            sentimentButtons[i].Setup(this, (ProposalSentiment)i);
        }
        for (int i = 0; i < proposalButtons.Length; i++)
        {
            proposalButtons[i].Setup(this);
        }

        submitButton.onClick.AddListener(Submit);

        //Show(proposal);
    }


    public void Show(PlayerProposalObject proposal)
    {
        foreach(var s in sentimentButtons)
        {
            s.Show();
        }
        for (int i = 0;i < proposalButtons.Length; i++)
        {
            proposalButtons[i].Show(proposal.options[i]);
        }
        header.text = proposal.header;
        gameObject.SetActive(true);
        hasSentiment = false;
        hasOption = false;
        IsSubmitted = false;
        UpdateSubmitButton();
    }

    public void SelectProposal(SelectDialogProposalButton proposal)
    {
        OnProposalSelected?.Invoke(proposal);
        option = proposal.proposal;
        hasOption = true;
        UpdateSubmitButton();
    }

    public void SelectSentiment(SelectDialogSentimentButton sentiment)
    {
        OnSentimentSelected?.Invoke(sentiment);
        this.sentiment = sentiment.sentiment;
        hasSentiment = true;
        UpdateSubmitButton();
    }

    public void Submit()
    {
        OnProposalSubmitted?.Invoke(option, sentiment);
        IsSubmitted = true;
        gameObject.SetActive(false);
    }

    void UpdateSubmitButton()
    {
        submitButton.interactable = hasOption && hasSentiment;
    }


}
