using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Citizen", menuName = "GameModels/Citizen")]
public class CitizenObject : ScriptableObject
{

    public CitizenBio bio;
    public CitizenSentiment sentiment;
    public List<CitizenProposal> proposals;
    public CitizenResult result;


}


[System.Serializable]
public class CitizenBio
{

    public string name, portrait, avater;

    [TextArea] 
    public string description, dossier;

}


[System.Serializable]
public class CitizenSentiment
{
    public string positive, negative;
}


[System.Serializable]
public class CitizenProposal
{
    public string positive, negative;

    [TextArea]
    public string plusOne, plusTwo, zero, minusOne, minusTwo;
}

[System.Serializable]
public class CitizenResult
{
    [TextArea]
    public string Yes;

    [TextArea]
    public string No;
}
