using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CanvasDialogController : MonoBehaviour
{


    bool isInitialized;
    public SelectBribeController bribeSelect;
    public SelectDialogController dialogSelect;
    public DisplayDialogController dialogDisplay;
    public PlayerCanvasObject playerCanvas;

    public Dictionary<CitizenObject, CitizenCanvasSession> sessions = new Dictionary<CitizenObject, CitizenCanvasSession>();

    public static System.Action OnDialogueStart, OnDialogueEnd;
    public static System.Action<int> OnDialogueFavor;

    public readonly int[,] OpinionMatrix = new int[,]
    {
        {2, 0, -1 },
        {1, 0, -1 },
        {0, 0, -2 }
    };


    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Show(CitizenObject citizen)
    {
        gameObject.SetActive (true);
        StartCoroutine(HandleDialog(citizen));
        OnDialogueStart?.Invoke();
    }

    public void Complete(CitizenCanvasSession session = null)
    {
        if(session != null)
        {
            GameManager.CurrentLevel.citizenSessions.Add(session);
            CitizenCanvasSession.OnSessionComplete?.Invoke(session);
        }
        //bribeSelect.gameObject.SetActive(false);
        //dialogSelect.gameObject.SetActive(false);
        //dialogDisplay.gameObject.SetActive(false);

        if(gameObject.activeSelf)
        {
            OnDialogueEnd?.Invoke();
        }

        gameObject.SetActive (false);
    }


    IEnumerator HandleDialog(CitizenObject citizen)
    {
        if (sessions.ContainsKey(citizen))
        {
            dialogDisplay.Show(GenerateDisplay(citizen, sessions[citizen].IsFavored() ? citizen.result.Yes : citizen.result.No));
            yield return new WaitUntil(() => dialogDisplay.IsComplete);
            Complete();
            yield break;
        }

        CitizenCanvasSession session = new CitizenCanvasSession(citizen);
        dialogDisplay.Show(GenerateDisplay(citizen, citizen.sentiment.intro));
        yield return new WaitUntil(() => dialogDisplay.IsComplete);
        for (int i = 0; i < playerCanvas.proposals.Count; i++)
        {
            var proposal = playerCanvas.proposals[i];
            dialogSelect.Show(proposal);
            yield return new WaitUntil(() => dialogSelect.IsSubmitted);
            //var response = citizen.proposals[i];
            int sentiment = ResolveProposal(citizen.proposals[i], citizen.sentiment, dialogSelect.option, dialogSelect.sentiment, ref session);
            OnDialogueFavor?.Invoke(sentiment);
            string response = GetSentimentResponse(citizen.proposals[i], sentiment);
            GameManager.CurrentSession.AddProposalOption(proposal, dialogSelect.option);
            dialogDisplay.Show(GenerateDisplay(citizen, response, sentiment));
            yield return new WaitUntil(() => dialogDisplay.IsComplete);
        }
        if(playerCanvas.money > 0)
        {
            bribeSelect.Show();
            yield return new WaitUntil(()=> bribeSelect.IsSubmitted);
            session.isBribed = bribeSelect.IsBribing;

        }
        sessions.Add(citizen, session);
        dialogDisplay.Show(GenerateDisplay(citizen, session.IsFavored() ? citizen.result.Yes : citizen.result.No));
        yield return new WaitUntil(()=>dialogDisplay.IsComplete);
        Complete(session);
    }


    int ResolveProposal(CitizenProposal proposal, CitizenSentiment attitude, ProposalOption option, ProposalSentiment sentiment, ref CitizenCanvasSession session)
    {

        int proposalOpinion;
        int sentimentOpinion;

        if(proposal.positive.Contains(option.id))
        {
            proposalOpinion = 0;
        }
        else if(proposal.negative.Contains(option.id))
        {
            proposalOpinion = 2;
        }
        else
        {
            proposalOpinion = 1;
        }

        if(attitude.positive.Contains(sentiment.ToString()))
        {
            sentimentOpinion = 0;
        }
        else if(attitude.negative.Contains(sentiment.ToString()))
        {
            sentimentOpinion = 2;
        }
        else
        {
            sentimentOpinion = 1;
        }

        int totalOpinion = OpinionMatrix[sentimentOpinion, proposalOpinion];

        Debug.Log($"CanvasDialog: Player selected proposal {option.id} with sentiment {sentiment}. " +
            $"Citizen favored proposal {proposal.positive} with sentiment {attitude.positive} and dislikes proposal {proposal.negative} or sentiment {attitude.negative}. " +
            $"Proposal opinion is {proposalOpinion}. Sentiment opinion is {sentimentOpinion}. Total opinion is {totalOpinion}");

        session.opinions.Add(totalOpinion);

        return totalOpinion;
    }

    string GetSentimentResponse(CitizenProposal proposal, int sentiment)
    {
        switch (sentiment)
        {
            case -2:
                return proposal.minusTwo;
            case -1:
                return proposal.minusOne;
            case 0:
                return proposal.zero;
            case 1:
                return proposal.plusOne;
            case 2:
                return proposal.plusTwo;
        }
        return "NULL";
    }


    //public bool IsFavored(CitizenCanvasSession session)
    //{
    //    return session.GetTotalOpinion() > 5;
    //}


    DisplayDialog GenerateDisplay(CitizenObject citizen, string dialog, int sentiment = 0)
    {
        DisplayDialog display = new DisplayDialog();
        display.title = citizen.bio.name;
        display.portrait = citizen.bio.portrait;
        display.isPlayer = false;
        display.dialog = dialog.Replace("[Name]", GameManager.CurrentSession.playerName).Split("[/n]");
        display.sentiment = sentiment;
        return display;
    }

    DisplayDialog GenerateDisplay(PlayerCanvasObject player, string dialog)
    {
        DisplayDialog display = new DisplayDialog();
        display.title = GameManager.CurrentSession.playerName;
        display.portrait = player.portrait;
        display.isPlayer = true;
        display.dialog = dialog.Split('\n');
        return display;
    }

}


[System.Serializable]
public class CitizenCanvasSession
{

    public static System.Action<CitizenCanvasSession> OnSessionComplete;


    public CitizenObject citizen;
    public List<int> opinions;
    public bool isBribed;


    public CitizenCanvasSession(CitizenObject citizen, bool isBribed = false)
    {
        this.citizen = citizen;
        this.isBribed = isBribed;
        opinions = new List<int>();
    }

    public int GetTotalOpinion()
    {
        int rawOpinion = Mathf.Clamp(citizen.sentiment.startingOpinion + opinions.Sum(), 0, 10);

        if (isBribed)
        {
            if(rawOpinion > 3 && rawOpinion < 6)
            {
                return 10;
            }
            if(rawOpinion > 5 && rawOpinion < 8)
            {
                return 0;
            }
        }

        return rawOpinion;
    }

    public bool IsFavored()
    {
        return GetTotalOpinion() > 5;
    }
}
