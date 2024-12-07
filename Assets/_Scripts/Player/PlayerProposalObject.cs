using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Proposal", menuName = "GameModels/Proposal")]
public class PlayerProposalObject : ScriptableObject
{

    public string header;

    public ProposalOption[] options;

}

[System.Serializable]
public class ProposalOption
{
    public string id;
    public string title;
    public List<ProposalOptionDialog> dialogs;
}

[System.Serializable]
public class ProposalOptionDialog
{
    public ProposalSentiment sentiment;

    [TextArea]
    public string dialog;
}

public enum ProposalSentiment
{
    Professional,
    Humerous,
    Intimidating,
    Casual
}
