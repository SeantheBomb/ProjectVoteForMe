using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="CampaignManager", menuName ="GameModels/CampaignManager")]
public class CampaignManagerObject : ScriptableObject
{
    [TextArea]
    public string[] intro;

    public CampaignManagerInterstitial[] interstitials;

}

[System.Serializable]
public class CampaignManagerInterstitial
{
    [TextArea]
    public string[] winningDialog;
    [TextArea]
    public string[] losingDialog;
}
