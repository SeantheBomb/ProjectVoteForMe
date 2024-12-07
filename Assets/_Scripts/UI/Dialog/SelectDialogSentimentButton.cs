using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectDialogSentimentButton : MonoBehaviour
{

    Button button;
    TMP_Text text;

    public ProposalSentiment sentiment;


    public void Setup(SelectDialogController controller, ProposalSentiment sentiment)
    {
        button = GetComponent<Button>();
        text = GetComponentInChildren<TMP_Text>();
        controller.OnSentimentSelected += OnSentimentSelected;
        text.text = sentiment.ToString();
        this.sentiment = sentiment;
        button.onClick.AddListener(() => controller.SelectSentiment(this));
    }

    public void Show()
    {
        button.interactable = true;
    }

    void OnSentimentSelected(SelectDialogSentimentButton sentiment)
    {
        button.interactable = sentiment != this;
    }
}
